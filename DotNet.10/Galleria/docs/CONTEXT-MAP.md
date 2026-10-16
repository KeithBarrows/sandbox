# Galleria — Context Map

| Source | Authority | Freshness | Notes / exclusions |
|---|---|---|---|
| `docs/Architecture.md` | Primary — current target design | Actively being extended; re-read in full each session | Supersedes `Basic Overview.md` wherever the two conflict |
| `docs/Basic Overview.md` | Foundational intent, not current spec | Static, written before this redesign | Use for original vision/wording only; defer to Architecture.md on any conflict |
| `src/` (existing code) | Legacy reference only, not a design source | Stale — likely does not build | Salvage candidates only: `ImageHandler.cs`, `TagBucketing.cs`, the `VideoFrameExtractor` in `GetGallery.cs`, SHA256 logic in `IdentifyDuplicates` |
| Claude memory: `galleria-architecture.md` | Pointer, not content | Written 2026-10-14 | Tells a future session this file exists and where the real doc is — don't treat its summary as more current than Architecture.md itself |

**Retrieval trigger**: any question about the target design, data model, or UI spec -> read
`docs/Architecture.md` fresh, don't rely on a memory of a past session's summary of it.
