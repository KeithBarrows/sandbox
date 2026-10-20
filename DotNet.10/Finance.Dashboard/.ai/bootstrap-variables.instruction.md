---
title: Bootstrap Variables - Finance.Dashboard
description: "Per-repo scope variables, loaded by bootstrap.instruction.md and engram-prefix.instructions.md. This is the only file that should differ between coding repos."
version: 1
status: active
owner: Keith Barrows
scope: repository
---

# Bootstrap Variables

## VARIABLES - EDIT THIS SECTION BEFORE USE

~~~
REPO    = Finance.Dashboard
PROJECT =
~~~

> **Instructions:** `REPO` is fixed for this repo — do not change it. `PROJECT` names a single project folder under `src/` (e.g. `FinanceDashboard.App`) and is only meaningful as a child of this `REPO`, never on its own; leave it blank until a project actually exists there. Unlike the fiction-side template this is based on, `ROLE` is deliberately **not** set here — it changes too fast within a single working session to be a fixed session variable. It is asked fresh for every engram at capture time (see `engram-prefix.instructions.md`) and, for retrieval, is queried across all six known roles by default (see `bootstrap.instruction.md`, Step 2).
