# Galleria — Architecture

Status: proposed. The current source under `src/` does not fully implement this and, per the 2026-10
review, likely does not build. This document describes the target design. Whether existing code is
salvaged or the project is rebuilt from scratch is a separate decision (see "Path forward" at the end).

## 1. Purpose

A personal photo/video library viewer. Files live in a folder tree on `I:\`, where folder names encode
tags (see `Basic Overview.md`). Galleria indexes that tree into a database, lets the user browse by tag,
view images/video full-screen, add or remove tags by hand, and find duplicate files.

## 2. Shape: one app, not two

Replace the current `GalleriaService.Api` + `Galleria.Web` split with a single ASP.NET Core project:

- Blazor Server, same pattern as the WeatherStation app (`UseWindowsService`, Serilog, EF Core +
  Npgsql, hosted background services for long-running work).
- No separate API project. No network hop between "frontend" and "backend" — they're the same process.
- Runs as a Windows Service, LocalSystem, auto-start, same as WeatherStation. (If it also needs to run
  interactively for local dev, `UseWindowsService` is a no-op when not launched by the SCM, so `dotnet run`
  still works unchanged.)

## 3. The filesystem is the source of truth for tags — Postgres is an index

This is the single biggest design decision, and it reverses the first draft of this doc: **tags are not
a database concept with a filesystem side-effect. The folder path IS the tag set.** Postgres holds a
queryable *index* of what the filesystem already says, rebuilt by scanning, not an independent store that
can drift from it. Adding or removing a tag means physically moving the file on disk; the database row
just follows along.

Why: this guarantees a rescan (whether triggered by the app, or because the user reorganized folders by
hand in Explorer) always finds the true, current tag set at the source. There's no "manual tag" living
only in a database row that a rescan could silently clobber or forget about.

### Root folder structure (confirmed from the real tree)

```
I:\                              ← root, no files ever live here
 └─ Person                       ← type bucket, no files live here either
     └─ p-Chloe                  ← a tag folder — files CAN start living from here down
         └─ c-Platinum           ← files can live even deeper; tags nest arbitrarily
             └─ h-Back
                 └─ [files]
 └─ Album
 └─ Studio
 └─ Category
 └─ Highlight
 └─ Gallery
```

- One type-bucket folder per tag type (`Person`, `Studio`, `Album`, `Category`, `Highlight`, `Gallery`),
  directly under the root. Files never live in the root or in a bucket folder — only from one level
  below a bucket (a real tag folder) downward.
- **Tag folders can repeat in a single path**, including the same prefix twice (e.g. two `p-` folders
  nested, for a photo of two people; two `a-` folders, for a file that's in two albums). Confirmed from
  the real tree at `F:\IO\Album\a-Cam-Girl\p-Frances Bentley\p-Martina Smeraldi\a-FFM`.
- **Tag order within a path is not fixed and not meaningful.** The existing tree mixes bucket choice and
  nesting order freely (the same `p-Frances Bentley` folder is reused as a shared ancestor for several
  different branches below it). Reading a file's tags means walking from the file up to `I:\` and
  collecting every folder name that matches a tag pattern — order doesn't matter, only the set does.

### Core tables (names illustrative, not final)

- **File** — one row per real file on disk. Path, size, hash (for dedup), kind (image/video),
  `created_at`, `last_seen_scan_id` (see §4), thumbnail path.
- **Tag** — one row per *logical* tag (`name` + `type`: Person / Studio / Album / Category / Highlight /
  Gallery). A logical tag can be realized by more than one physical folder in the tree (e.g. `p-Frances
  Bentley` may exist as a folder under several different ancestors) — the Tag row represents the name,
  not any one physical location. Also holds `CoverFileId` (nullable FK to `File`) — see §6, "Two gallery
  levels."
- **FileTag** — join table, File ↔ Tag. No `source` column needed anymore (that was the old,
  database-is-authoritative design) — every row here is derived from, and must match, the file's actual
  folder path. The scan job is what keeps this table in sync with disk; it is never hand-edited
  independently of a physical move.
- **Duplicate** — pairs (or groups) of File rows flagged as likely duplicates, plus how they were
  matched (hash match vs perceptual/pixel similarity), and a resolved/ignored status so the same pair
  doesn't keep resurfacing after the user has looked at it once.

## 4. Editing a tag = moving the file

Adding or removing a tag on a file (or a gallery) is implemented as a physical file move, not a database
write:

1. Compute the file's **full remaining tag set** (current tags, plus the one being added, or minus the
   one being removed).
2. Find an existing folder in the tree whose own ancestor-chain tag set exactly matches that target set,
   or create one if none exists.
3. Move the file there. This never touches or renames a folder other files still depend on — a shared
   ancestor folder is only ever added *to* (a new child created under it), never renamed or deleted out
   from under its other children.
4. Update the `File` and `FileTag` rows to match, in the same operation — the user sees the new tag
   immediately, without waiting for a rescan.

**Open design item — tag type precedence:** when a file ends up with tags of more than one type (common
case — most files have several), something has to decide which type's bucket folder is the root of the
newly-built path, since there's no fixed nesting order to copy from the existing tree (see §3). This
needs its own rules table (tag type → precedence, plus some special-case exceptions, not just a flat
ranked list) and has **not been designed yet**. Flagging it here rather than guessing at rules nobody's
asked for.

## 5. Scanning: background job, DB-only reads for the UI

- The user clicks "Rescan." This enqueues a job; it does not run inline on the request.
- A hosted background service (same `BackgroundService` pattern as WeatherStation's
  `HistoricalBackfillService`) picks up the job and walks the tree under `I:\`, reconciling the `File` and
  `FileTag` tables against what's actually there. This is also what catches tag changes a user made by
  hand in Explorer, outside the app.
- Every page in the UI reads only from Postgres — never touches the filesystem directly on render. This
  was explicitly requested: the app stays responsive while a scan is running, because rendering a page
  and scanning disk are now unrelated operations.
- A file that existed in the DB but is no longer found on disk is marked gone (don't hard-delete —
  preserves its dedup history in case it reappears, e.g. a drive remount order issue).
- Duplicate detection can run as its own background job (same pattern), separate from the folder scan,
  since hashing/comparing file contents is a different (and slower) kind of work than reading folder
  names.

## 6. Web UI

Two screens per the overview doc:

1. **Gallery** — left area is the image/video grid for the current tag; right column (~25% width when
   open) is the tag cloud. Full browser width, 10px body margin.
2. **Single item** — one image/video, full width, no tag cloud, a "back to gallery" control.

Plus a header bar listing top-level tag types (Albums, Categories, Highlights, People, Studios,
Galleries); picking one shows that type's tags.

### Tag cloud panel: collapsible, in two layers

The right-column tag cloud collapses in two independent ways:

1. **Per tag type, accordion-style.** When several tag types have entries worth showing at once, each
   type is its own collapsible section inside the column (expand/collapse per type), not one giant mixed
   list.
2. **The whole column, as a drawer.** The entire right column can slide out to the right edge of the
   browser, leaving only a thin strip with a left-pointing arrow as its only visible remnant. Clicking
   that arrow slides it back open.
- When the column is collapsed, the gallery grid is not left with dead space on the right — it reflows
  to use the full browser width, same as if the column were never there. Expanding the column again
  reflows the grid back down to its ~75% share.

### Two gallery levels

Picking a type in the header doesn't show every file of that type in one flat grid — it shows **one card
per tag** of that type first, and drilling into a specific tag is what shows that tag's files. Two
levels, same card component, different data behind it:

1. **Type-overview gallery** (header → "Person"): one card per `Tag` row of that type. The card's image
   is that tag's **cover photo** — `Tag.CoverFileId` — not a tag-filtered grid of thumbnails. The tag
   band under the card holds exactly one link: the tag's own title (e.g. "Chloe"). Clicking the image
   *or* that link both do the same thing here — drill into the tag.
2. **Tag-detail gallery** (clicked into "Chloe"): the familiar file-level grid from before — one card
   per file tagged `p-Chloe`, each with its full tag band and the usual image-vs-tag-link click split.

Every tag type works this way — Person, Studio, Album, Category, Highlight, Gallery all get a
type-overview level before their file-level grid.

**Setting a cover photo**: some file belonging to a tag needs to become that tag's
`CoverFileId`. Exposed as an action on a file (e.g. "Set as cover photo for Chloe" — named per-tag,
since a file can carry several tags and needs to say which one it's setting the cover for). Until a tag
has one set, the type-overview card falls back to some file of that tag (e.g. the first one found) so the
card never shows blank.

Startup: pick a random tag, a random starting item within it, then let the user step through the rest of
that tag's items in order.

Tag editing (add/remove on a file, or on a gallery) triggers the move described in §4 — normal Blazor
Server interaction, no page reload, no separate API call, since UI and data layer are in the same
process.

Thumbnails: generated once per file (image: resize; video: frame extraction, e.g. FFMpegCore, which the
current code already sketches in `GetGallery.cs`), cached to disk, path stored on the `File` row, served
as a static file. Generation belongs in the scan job, not on first page view, so the gallery grid never
blocks on the first thumbnail request.

### Gallery card — confirmed from the visual mockup

- **Default size**: 300px tall. Width flexes per item to show the image/video's true aspect ratio
  (Pinterest/Google-Photos style — fixed height, variable width), capped at 480px wide; anything wider
  (e.g. a panorama) crops instead of blowing out the row. At 300px cards, a 1440px-tall viewport fits 4
  full rows with room to spare (confirmed: ~116px of header/padding/gap overhead + 4×300px = ~1316px of
  1440px). Monitor *width* only changes how many cards fit per row, never the row count — row count is a
  height-only calculation.
- **Hover**: scales up 10% in both directions, elevated above its neighbors, to highlight the card under
  the cursor.
- **Bottom tag band**: every tag on the file/gallery, shown as small pill-style links (prefix letter
  stripped, title-cased — `a-Cam-Girl` → "Cam Girl"). Wraps to more rows as needed; total card height
  stays fixed, so the image area shrinks to make room for a taller band on heavily-tagged files. These
  links appear on **both** the gallery card and the single-item page.
- **Video thumbnail**: a frame grab at the 5-minute mark (or the 10% mark for anything under 50 minutes),
  a play-button overlay, and film-sprocket rails down the left/right edges so a video card is
  unmistakable at a glance.
- **Click behavior**: clicking the image/video area opens the single-item page. Clicking a tag link in
  the band instead filters the gallery to that tag and stays on the gallery page.

### Settings

Card height (and whatever else turns up — this is the first of what will likely be several runtime
knobs) needs to be adjustable without a code change or redeploy. A small **Settings** feature:

- A `Setting` table (key/value, Postgres) holding the current values; a settings menu in the UI (e.g. a
  gear icon in the header) to view and edit them.
- Values apply immediately — Blazor Server re-renders on change, no app restart needed.
- Starts with card height and max card width; expected to grow as more "this should really be
  adjustable" moments come up during build-out, rather than each one getting its own one-off mechanism.

## 7. What to carry forward vs drop

From the current code:

- **Keep the idea of:** folder-prefix tag convention, vertical-slice feature folders as a code
  organization style, FFMpegCore for video frame extraction, SHA256-based duplicate hashing as a first
  pass.
- **Drop:** the API/Web split, flat-JSON storage, the stub endpoints that return canned strings, the
  default MVC scaffold in `Galleria.Web`, hardcoded `I:\` paths in source (move to config, same as
  WeatherStation's `appsettings.json` pattern).

## 8. Path forward

Given how little of the current code is real (most feature endpoints are stubs with the actual logic
commented out, and `GalleriaService.Api/Program.cs` calls two extension methods that don't exist), a
rewrite is likely less work than retrofitting this architecture onto the existing split. Candidates worth
lifting as-is into a new project: `ImageHandler.cs`, `TagBucketing.cs`, the `VideoFrameExtractor` in
`GetGallery.cs`, and the SHA256 comparison logic in `IdentifyDuplicates`. Everything else — routing,
DI wiring, storage, the Web project — gets rebuilt rather than patched.
