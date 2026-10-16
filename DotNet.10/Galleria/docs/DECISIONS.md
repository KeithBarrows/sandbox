# Galleria — Decisions Log

## Filesystem is the source of truth for tags (not the database)
**Date:** 2026-10-14
**Decision:** Editing a tag = physically moving the file to a folder path matching its full tag set.
Postgres indexes the filesystem; it does not hold tags independently of it.
**Why:** An earlier draft had tags as DB rows with a Structural-vs-Manual source split. Keith rejected
that shape directly — he wants a rescan to always find the true tag set at the source (the filesystem),
with no DB-only tag that a rescan could silently diverge from or clobber.
**Status:** Settled.

## Multi-tag-of-same-type handled by nested repeated folders, not hardlinks
**Date:** 2026-10-14
**Decision:** A file with two tags of the same type (e.g. two people) gets two nested folders of that
prefix in its path, not an NTFS hardlink/junction duplicating the file.
**Why:** Confirmed against the real tree (`F:\IO\Album\a-Cam-Girl\p-Frances Bentley\p-Martina
Smeraldi\a-FFM`) — this is already the existing convention. Hardlinks were considered and rejected: same-
drive only, and backup/sync/antivirus tools handle reparse points inconsistently.
**Status:** Settled.

## Gallery card: 300px height, flexible width, no cropping
**Date:** 2026-10-14
**Decision:** Cards are 300px tall; width flexes to the item's true aspect ratio, capped at 480px before
cropping kicks in.
**Why:** Verified visually with a Design-canvas mockup at three heights (260/300/340px) against a
1440px-tall viewport; 300px was the one Keith picked, confirmed to fit 4 full rows with room to spare.
**Status:** Settled.

## Tag-type precedence rules — deferred
**Date:** 2026-10-14
**Decision:** Not decided. When a file has tags of multiple types, which type's bucket becomes the root
of a new path is unresolved.
**Why:** Keith flagged this himself as needing its own rules table (ranking plus special cases), not a
guess. Rather than invent rules, this is logged as open.
**Status:** Open — next concrete outcome.
