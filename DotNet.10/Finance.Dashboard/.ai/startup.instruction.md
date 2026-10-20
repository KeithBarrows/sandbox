---
title: Startup Sequence
description: "The concrete steps any AI client should run at the start of a session in this repo, before doing anything else. Ties bootstrap.instruction.md, engram-prefix.instructions.md, and journal/ together into one checklist."
version: 1
status: active
owner: Keith Barrows
scope: repository
---

# Startup Sequence

Run this once at the start of a session, before taking on any task, role, or recipe. This is preparation, not action — finishing this checklist does not itself imply starting any particular piece of work.

---

## 1. Confirm where we are

Read `REPO` and `PROJECT` from `bootstrap-variables.instruction.md` in this same `.ai/` folder. State them plainly (e.g. "Repo: Finance.Dashboard, Project: none set yet") rather than assuming they're already known.

## 2. Confirm scope of access

This AI has access to the repo root and every subfolder beneath it — not just `.ai/` or the current working directory. Don't artificially narrow investigation to one folder unless told to.

## 3. Engage OB1

Run Step 1 (Load Mandatory Directives) and Step 2 (Query OB1 and Build Acting Context) from `bootstrap.instruction.md`. This retrieves the Mandatory tier plus the Resolved Effective Context across all six roles for the current `REPO`/`PROJECT` — the "always used" fingerprints and recipes, not just a placeholder concept.

## 4. Check git status before assuming a clean slate

Run a plain git status check. Uncommitted work, an in-progress branch, or staged-but-uncommitted changes all change what "starting fresh" should mean here. Never assume a clean working tree without checking.

## 5. Review the journal

Read `journal/JOURNEY.md`, most recent entry first (entries are reverse-chronological). This is the standup equivalent — what happened last session, where things were left, what's still open. A repo with no journal yet is itself worth noting, not silently skipped past.

## 6. Surface unresolved items and ask before proceeding

Step 2's Resolved Effective Context and the journal review will often surface active threads or unresolved items (an in-progress `architect-module` pressure-test, an open decision, a half-finished recipe run). Name these explicitly and ask whether to resume one of them, rather than silently starting something new as if the slate were blank.

## 7. Stop

Once 1-6 are done, stop and wait for the user's actual direction. Do not default into coding, a specific role, or a recipe just because startup completed — per the Mandatory bounded-scope rule, "session started" is not itself a bounded unit of work.
