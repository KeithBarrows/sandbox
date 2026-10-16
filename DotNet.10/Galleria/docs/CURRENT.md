# Galleria — Current State

**Phase:** Architecture design, discussion-first. No code written against this design yet.
**Next outcome:** Design the tag-type precedence rules (see Open Questions below).

## What's settled
- One app: Blazor Server, no separate API project. Runs as a Windows Service (LocalSystem, auto-start),
  same pattern as the WeatherStation app.
- Postgres in Docker is an *index*, not the source of truth. The filesystem is the source of truth for
  tags — editing a tag means physically moving the file; the DB follows.
- Root structure: `I:\` (no files) -> type-bucket folders (Person/Studio/Album/Category/Highlight/Gallery,
  no files) -> tag folders (files live here down). Tags can nest in any order and repeat (two `p-`
  folders for two people in one photo).
- Gallery has two levels: pick a type -> one card per tag (using a cover photo); click a tag -> that
  tag's files, one card each.
- Gallery card: 300px tall, width flexes to true aspect ratio (capped 480px wide), hover grows 10%,
  bottom tag band of pill links, video thumbnails get a frame grab (5 min mark, or 10% mark under 50 min)
  + play icon + sprocket rails.
- Tag cloud (right column): accordion per tag type, plus the whole column can collapse to an edge arrow;
  gallery grid reflows to fill the space when it's collapsed.
- Settings: generic key/value table + a settings menu, starting with card height/width, built to grow.

## Open questions (not decided, not guessed at)
- **Tag-type precedence**: when a file has tags of more than one type, what decides which type's bucket
  becomes the root of a newly-built folder path? Needs its own rules table — flat ranking plus some
  special-case exceptions. Not started.

## Not yet discussed
- Duplicate-detection UI/workflow details (resolve/ignore flow).
- Gallery-type tag specifics (a `g-` gallery tag's own membership editing).
- Thumbnail generation pipeline mechanics (where files get cached, retry behavior).
- Whether/how the existing `src/` code gets replaced vs. deleted outright.
