# Finance.Dashboard — Journal

Reverse-chronological: most recent entry at the top. See `.ai/startup.instruction.md` for how this file gets read at the start of a session. The curated narrative lives here; the mechanical, complete commit-by-commit record lives alongside it in this same folder (e.g. `2026-07.md`).

---

## 2026-07-22 — Coding-side OB1 scope system

Built the AI instruction/scope system for coding work: a Mandatory -> Role -> Role|Repo -> Role|Repo|Project ladder across six roles (Architect, Developer, Tester, PM, DevOps, Consultant), wired into `.ai/bootstrap.instruction.md`, `.ai/engram-prefix.instructions.md`, and `.ai/bootstrap-variables.instruction.md`, with thin per-tool shims (`CLAUDE.md`, `AGENTS.md`, `.github/copilot-instructions.md`) pointing into `.ai/`. Seeded OB1 with Mandatory directives, Architect Fingerprint entries, the Consultant/PM role boundary, and a set of single-purpose recipes covering repo onboarding, architecture/prototype workflow, commit messages, and logging/journaling.

*Git pointer: `git checkout milestone/ai-scope-system-v1` (a0cca59)*

---

## 2026-07-21 — Kickoff artifacts

Produced the initial planning artifacts for the project: an Executive Brief/Overview and an Initial Project Plan, both as `.docx` files in `docs/`. These captured the three-phase concept (Balance Guardian, Insights Advisor, Categorizer) and the architecture decisions made before any code existed — WinForms over a Windows Service, credit limits parsed from YNAB account notes, secrets in Windows Credential Manager, among others — all recorded before writing a line of code.

*Git pointer: `7b9e549`*

---

## 2026-07-21 — Stood up the repo

Stood up `Finance.Dashboard` at `git@github.com:KeithBarrows/Finance.Dashboard.git` (local: `D:\Repos\KeithBarrows\Finance.Dashboard`). Created `src/`, `tests/`, `docs/`; seeded `README.md`, `LICENSE` (MIT), `global.json` (pinned to the installed .NET SDK), `.editorconfig`. This is the origin point everything else in this journal builds on.

*Git pointer: `b9376b3`*
