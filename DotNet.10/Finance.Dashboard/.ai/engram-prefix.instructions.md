---
title: Engram Scope Prefix
description: "Repo-agnostic engram prefix instruction for coding work. Variables live in a separate per-repo file, bootstrap-variables.instruction.md, so this file can be reused unchanged across every coding repo."
version: 1
status: active
owner: Keith Barrows
scope: repository
---

# Engram Scope Prefix

Execute this instruction **every time** an engram is about to be saved to OB1 via `capture_thought`. Never assume or auto-select a role or scope — always ask.

---

## Variables

This file is repo-agnostic. Read `bootstrap-variables.instruction.md` from the current repo and use its `REPO` and `PROJECT` values whenever this instruction substitutes a value into a prefix option below. If that file is missing from the repo, say so plainly and ask the user to add it before continuing. `ROLE` is never read from a variables file — it is asked fresh every time, per Prefix Selection below.

---

## Terminology

- **OB1 / Obi Wan / Open Brain** — the MCP tool connecting to OpenBrain, hosted in Supabase. Use `search_thoughts` to query it.
- **Engram** — a single Thought stored in OB1.
- **Role** — which hat is active for this engram: `Architect`, `Developer`, `Tester`, `PM`, `DevOps`, or `Consultant`.

---

## Prefix Selection

Present the following to the user before saving each engram. Substitute the actual `REPO`/`PROJECT` values into the option labels.

> **Which role, and what scope, does this engram belong to?**

Always ask for **Role** first — `Architect`, `Developer`, `Tester`, `PM`, `DevOps`, or `Consultant` — then show these scope options:

1. **ROLE** — applies to this role across every repo and project, e.g. `Architect`
2. **ROLE | REPO** — applies to this role, scoped to this repo only, e.g. `Architect | Finance.Dashboard`
3. **ROLE | REPO | PROJECT** — applies to this role, scoped to one project inside this repo, e.g. `Architect | Finance.Dashboard | Categorizer-API`

Only offer option 3 once `PROJECT` is non-empty in the variables file, or the user names a project directly — PROJECT is never valid without a REPO ahead of it.

There is a separate, rare fourth tier — `MANDATORY` — for non-negotiable rules that override every scope below and are never superseded by a narrower one. Never offer it as a routine option; use it only when the user explicitly says the engram is a Mandatory directive.

---

## Applying the Prefix

Prepend the selected Role (and scope, if narrower than Role alone) to the engram content using ` | ` as the separator before calling `capture_thought`.

**Format:**
```
[Selected Scope] | [engram content]
```

**Examples:**
```
Mandatory | Never commit an API token or secret to a public repo.
Consultant | Repo discovery precedes picking a technical hat for any new engagement.
Architect | Prefer vertical slices over Clean/Onion architecture in 95% of cases.
Architect | Finance.Dashboard | WinForms desktop app, not a Windows Service — services run in Session 0 and cannot show UI on the interactive desktop.
Architect | Finance.Dashboard | Categorizer-API | Chose Rust over C# for this project specifically for its stronger guarantees around the email-parsing boundary.
```

---

## Rules

- Never skip this step. Every engram saved to OB1 must have a Role and a scope prefix.
- Never show variable names or placeholder text to the user. Always substitute real values from the variables section into the option labels.
- If the user does not provide a valid role and scope selection — dismisses the prompt, responds with something unrecognized, or asks a follow-up question instead of choosing — do not call `capture_thought`. Re-present the prompt and state: "A role and scope selection is required before this engram can be saved. Please choose one of the options above."
- Never guess the role or scope. If unclear, ask the user.
- If saving multiple engrams in one session, ask for each one individually unless the user explicitly says "use the same role and scope for all." Role in particular should be expected to change frequently even within one session — never assume it carries over from the previous engram.
