---
title: Bootstrap - Embedded Scope Parameters
description: "Repo-agnostic bootstrap for coding-repo AI sessions. Variables live in a separate per-repo file, bootstrap-variables.instruction.md, so this file can be reused unchanged across every coding repo."
version: 1
status: active
owner: Keith Barrows
scope: repository
---

# Bootstrap: Embedded Scope Parameters

---

## Variables

This file is repo-agnostic. Before running any step below, read `bootstrap-variables.instruction.md` from the current repo and use its `REPO` and `PROJECT` values for the entire session. Do not ask the user to confirm or re-enter them. If that file is missing from the repo, say so plainly and ask the user to add it before continuing.

`ROLE` is **not** read from the variables file — it is not a session-long constant here the way `REPO`/`PROJECT` are. By default, Step 2 builds context across all six known roles (`Architect`, `Developer`, `Tester`, `PM`, `DevOps`, `Consultant`) for the current REPO/PROJECT, since coding work cycles roles rapidly within a single session and loading only one role's slice would miss adjacent-hat context you're likely to need anyway. If the user states they're specifically working as one role right now, narrow to that role alone and say so plainly.

---

## Terminology

- **OB1 / Obi Wan / Open Brain** — the MCP tool connecting to OpenBrain, hosted in Supabase. Use `search_thoughts` to query it.
- **Engram** — a single Thought stored in OB1.
- **Role** — which hat is active: `Architect`, `Developer`, `Tester`, `PM`, `DevOps`, or `Consultant`. Functions as the top rung of the scope ladder (see Marker Reference item 7 for the separate, rarer `Mandatory` tier that sits above it).
- **Fingerprint** — a structured, evolving set of engrams that together describe how something should be built or done: a role fingerprint (`Architect Fingerprint`, `Developer Fingerprint`, `Tester Fingerprint`, `PM Fingerprint`, `DevOps Fingerprint`, `Consultant Fingerprint`), or a generic `skill-fingerprint`, `instruction-fingerprint`, `agent-fingerprint`, or any future type. Each fingerprint type has its own category shape, but all fingerprints are assembled the same way (see Step 3). Note: there is deliberately no `voice-fingerprint` equivalent here — code style/formatting already lives in machine-enforced files (`.editorconfig`, `Directory.Build.props`), not in OB1.

---

## Marker Reference

OB1 engrams in this system follow one of seven templates, distinguished by the literal marker text immediately following the scope prefix. Any AI reading this file, regardless of which product or chat tool it runs in, should recognize and apply these patterns consistently.

**1. Context engrams** (Step 2 — plain scoped knowledge, no special marker)
`[Scope] | [content]`

**2. Process fragments** (Step 3 — capture-process pieces, assembled into a runnable prompt via the `prompt for` marker)
`[Scope] | prompt for | [fingerprint-type] | [numbered slot heading, e.g. "## 3. Process:"] [content]`

**3. Content fragments** (Step 4 — actual fingerprint rules, compiled into a directive)
`[Scope] | [Fingerprint Type Name] | [Category] | Status: [new/override/refinement] | [the rule]. Why: [reasoning]. Example: [if useful].`

**4. Fixed instructions** (reusable as-is, no diff/process needed, via the `instruction for` marker)
`[Scope] | instruction for | [fingerprint-type] | [fixed content]`

**5. Recipes** (Step 5 — via the `recipe for` marker). Two valid shapes on the coding side:
   - *Bundle form* — names which fingerprints a task should assemble: `[Scope] | recipe for | [task-name] | [which fingerprint types to compile and how to combine them]`
   - *Runbook form* — the task doesn't decompose cleanly into "which fingerprints to combine"; the recipe instead embeds the complete, ready-to-execute procedure directly: `[Scope] | recipe for | [task-name] | [the full runbook content]`. Use this shape when the content is already a coherent, self-contained process (e.g. a multi-phase repo-onboarding runbook) rather than a pointer to other fingerprints.

**6. To-Do / loose notes** (not part of the fingerprint system, just tracked items)
`[Scope] | To-Do | [headline sections per scope, numbered items]`

**7. Mandatory directives** (Step 1 — always-on, highest-precedence rules loaded before any other context; a dedicated top-level scope that sits ABOVE the entire ROLE -> PROJECT ladder and is never overridden by any narrower scope. Here the marker word is itself the top scope segment, not a sub-marker under another scope)
`Mandatory | [directive]`

When searching OB1 for any of types 2, 4, or 5, always apply the corresponding Marker Filter (see Steps 3-5) before treating a result as that type — semantic rank alone is not sufficient, since vocabulary overlap can surface the wrong type of engram.

---

## Step 1: Load Mandatory Directives

Before building any acting context, load the non-negotiable directives that must be present in every session, regardless of role, repo, or project. These are the first thing placed into the AI's working context — ahead of Step 2's scope-ladder retrieval.

Run `search_thoughts` against OB1 with query `"Mandatory"`, using a widened `limit` (30) and lowered `threshold` (0.2) per the Search Reliability note. `Mandatory` is a deliberately unique token — no other scope uses it — so it ranks matching directives at or near the top instead of burying them among role-scoped engrams. Apply the Marker Filter: keep only results whose scope prefix contains the literal segment `Mandatory |`. Discard everything else.

Load every surviving directive verbatim and treat it as the highest-precedence layer of the session:

- Mandatory directives are non-overridable. Unlike Step 2 context, a narrower scope never supersedes a `Mandatory` rule. They sit above the entire ScopeOrder ladder.
- They apply for the whole session and are re-asserted, not re-decided, whenever context becomes uncertain.
- If a Mandatory directive conflicts with any later instruction, that is itself a conflict — halt and ask rather than silently resolving it.
- Expect this tier to be rare. If it stops being rare, that's a signal things are being marked non-negotiable that are really just strong defaults, and belong at the Role level instead.

If zero `Mandatory` engrams are found, say so plainly before continuing to Step 2, so a missing mandatory layer is visible rather than silently skipped. Do not fabricate or infer mandatory rules that were not retrieved.

---

The `REPO`/`PROJECT` variables loaded from `bootstrap-variables.instruction.md` are pre-resolved. Treat them as authoritative for the session. Do not ask the user to confirm or re-enter them.

Derive the following:

- `ScopeOrder` (per role): `ROLE -> REPO -> PROJECT`
- `Roles`: `Architect`, `Developer`, `Tester`, `PM`, `DevOps`, `Consultant`
- `HasProject`: true when `PROJECT` is non-empty in the variables file

---

## Step 2: Query OB1 and Build Acting Context

Unlike a single fixed identity, ROLE is not resolved from the variables file. By default, run the `ScopeOrder` ladder **once per role** (all six), for the current REPO/PROJECT, and merge everything into one Resolved Effective Context:

For each role in `Architect`, `Developer`, `Tester`, `PM`, `DevOps`, `Consultant`:
1. **ROLE** — query: `"[Role]"` — engrams that apply to this role across every repo and project.
2. **ROLE + REPO** — query: `"[Role] | [REPO]"`. Overrides any conflicts with level 1, within this role.
3. **ROLE + REPO + PROJECT** *(only when `HasProject`)* — query: `"[Role] | [REPO] | [PROJECT]"`. Overrides any conflicts with level 2, within this role.

That's up to 18 queries (6 roles × 3 levels, fewer when `HasProject` is false), merged into one context. Conflicts resolve narrower-wins **within each role's own ladder** — a Role-level entry loses to a Role+Repo entry for that same role. Roles never override each other: an Architect entry and a Developer entry on a related topic both stay in context side by side, since they represent different hats, not competing versions of the same fact.

If the user states they're specifically working as one role right now (e.g. "I'm doing Architect work"), narrow to that single role's 3-level ladder instead of all six, and say so plainly.

At each level, apply this retrieval intent:
*Gather everything relevant to this scope. I need a handoff summary that restores working context from last session: history, standing preferences, constraints and guardrails, open decisions, active threads, unresolved items, and next actions. Prioritize recent thoughts, but include older governing context when still relevant.*

The result after all queries complete is **one unified Resolved Effective Context** — not a separate result per role or per level.

---

## Resolution Rules

- Remove unrelated semantic matches and test-only entries unless explicitly requested.
- Deduplicate repeated engrams.
- Merge compatible engrams.
- Resolve conflicts with narrower scope taking precedence, within the same role.

---

## Final Output (Step 2)

Return a **Resolved Effective Context** brief with:

1. Working scope metadata (repo, project if set, which roles were loaded)
2. History and current state, grouped by role where it matters
3. Standing preferences and collaboration rules
4. Constraints and guardrails
5. Active threads and unresolved items
6. Next actions
7. Explicit vs inferred notes
8. Override notes (what narrower scope overrode, within which role)

---

## Step 3: Assemble a Runnable Prompt (On Demand)

Unlike Step 2, this step does not run automatically at session start. Run it only when explicitly asked to build, assemble, or update a prompt for a given fingerprint type, e.g. "build me the Architect fingerprint prompt" or "let's capture any Developer pieces that will alter the fingerprint."

`FINGERPRINT_TYPE` is whatever the user names: one of the six role fingerprints (`Architect`, `Developer`, `Tester`, `PM`, `DevOps`, `Consultant`), or a generic `skill-fingerprint`, `instruction-fingerprint`, `agent-fingerprint`, or any other type that has prompt-construction fragments stored in OB1. This step operates on a single role at a time — a fingerprint is inherently role-scoped, unlike Step 2's context load.

Run the 3-level `ScopeOrder` ladder for that one role, querying for prompt-construction fragments instead of context:

1. **ROLE** — query: `"[Role] prompt for [FINGERPRINT_TYPE]"`
2. **ROLE + REPO** — query: `"[Role] | [REPO] prompt for [FINGERPRINT_TYPE]"`
3. **ROLE + REPO + PROJECT** *(only when `HasProject`)* — query: `"[Role] | [REPO] | [PROJECT] prompt for [FINGERPRINT_TYPE]"`

#### Marker Filter (required before merging)

Semantic search returns results ranked by similarity, not by type. A content engram (e.g. an actual fingerprint rule) can rank highly against a prompt-assembly query purely because it shares vocabulary like the fingerprint type name. Before using any result, discard anything that does not contain the literal substring `| prompt for |` immediately following the scope prefix. Use semantic rank only to order matches within this filtered set — never as the sole basis for inclusion.

#### Slot Parsing and Merge (replaces generic "merge compatible fragments")

Each prompt-construction fragment that survives the Marker Filter contains one or more numbered slots in the form of a markdown heading: `## [N]. [Slot Name]`. Parse every filtered result for these headings.

- A slot heading present at a broader scope but marked as a placeholder (e.g. `## 2. [TBD - reserved slot, filled by narrower scope: ...]`) signals that the slot exists and expects content from a narrower scope.
- When two results provide content for the same slot number, the result from the narrower scope wins and replaces the broader scope's content for that slot, including replacing a placeholder.
- When a slot number appears in only one result, use it as-is.
- Assemble the final prompt by laying out all slots in ascending numeric order, regardless of which scope level supplied each one.

### Final Output (Step 3)

A single **Resolved Runnable Prompt** for `[Role] [FINGERPRINT_TYPE]`, assembled from all matching fragments across that role's scope ladder, ready to execute immediately against the current session. This output is a prompt to run, not a context brief to read.

If no fragments are found at any level, say so plainly and ask whether to start building that fingerprint type's Role-level process chunk from scratch, rather than silently returning an empty or guessed prompt.

---

## Step 4: Retrieve and Compile a Fingerprint's Content (On Demand)

Step 3 assembles the *process* for capturing new findings into a fingerprint. Step 4 does the inverse: it retrieves everything already captured *into* a fingerprint and compiles it into one usable directive an AI can apply immediately, with no further lookups required.

Run this only when explicitly asked, e.g. "pull the Architect fingerprint" or "give me the compiled Developer directive."

Run the 3-level `ScopeOrder` ladder for the one role named, querying for content fragments instead of prompt-construction fragments:

1. **ROLE** — query: `"[Role] Fingerprint"` (or `"[Role] [FINGERPRINT_TYPE]"` for a generic type, matching whatever marker word that type uses, e.g. `Skill Fingerprint`, `Instruction Fingerprint`)
2. **ROLE + REPO** — query: `"[Role] | [REPO] Fingerprint"`
3. **ROLE + REPO + PROJECT** *(only when `HasProject`)* — query: `"[Role] | [REPO] | [PROJECT] Fingerprint"`

#### Marker Filter (required before compiling)

Discard any result that does not contain the literal substring `| [Role] Fingerprint |` (or the equivalent type marker) immediately following the scope prefix. This excludes `prompt for` fragments and unrelated context engrams that merely mention similar vocabulary.

#### Category Grouping and Conflict Resolution

Group all filtered results by their category (the segment immediately following the type marker, e.g. `Design Philosophy`, `Debugging Discipline`).

Within each category:
- If multiple entries describe the same underlying rule using different wording (a true duplicate, not a refinement), keep only the most recently captured version.
- If an entry's `Status` is `override`, it replaces the entry it conflicts with; drop the older one entirely.
- If an entry's `Status` is `refinement`, keep both the original and the refinement; the refinement adds nuance rather than replacing.
- If an entry's `Status` is `new` and nothing else addresses the same point, keep it as-is.
- Entries at a narrower scope (e.g. PROJECT) take precedence over broader-scope entries (ROLE/ROLE+REPO) only when they directly conflict; otherwise both apply, since broader rules are general defaults and narrower rules are additive specializations.

#### Final Output (Step 4)

A single **Compiled Fingerprint Directive**: one document, organized by category, each category listing its active rules in plain directive form (rule plus its Why; omit Status and Example unless the example is genuinely useful for calibration). This is meant to be handed directly to an AI as "work according to this" — not a diff, not a process, a ready-to-use specification for that role.

If a category has no entries at any scope level, state plainly that the category is empty rather than omitting it silently, so gaps in the fingerprint are visible.

---

## Step 5: Run a Recipe (On Demand)

A Recipe names a task and either (a) which fingerprints to assemble together for it, or (b) embeds the complete runbook directly when the task doesn't decompose into fingerprint pieces (see Marker Reference item 5). Run this only when explicitly asked to perform a task that has a known recipe, e.g. "onboard onto this repo."

`RECIPE_NAME` is whatever the user names or implies, e.g. `principal-engineer-repo-onboarding`.

A Recipe can live at any scope level — most will sit at the ROLE level (repo-independent), since that's where most role-guardrail and process content naturally lives; ROLE+REPO or ROLE+REPO+PROJECT recipes are the rare case, same as content fragments. Search across all three levels for the stated (or most likely) role:

1. **ROLE** — query: `"[Role] recipe for [RECIPE_NAME]"`
2. **ROLE + REPO** — query: `"[Role] | [REPO] recipe for [RECIPE_NAME]"`
3. **ROLE + REPO + PROJECT** *(only when `HasProject`)* — query: `"[Role] | [REPO] | [PROJECT] recipe for [RECIPE_NAME]"`

If the role isn't stated and isn't obvious, search across all six roles at the ROLE level first, since that's where most recipes will be found.

#### Marker Filter

Discard any result that does not contain the literal substring `| recipe for |` immediately following the scope prefix.

#### Execution

- **Bundle-form recipe**: for each fingerprint type named, run Step 4 to produce that fingerprint's Compiled Fingerprint Directive, then combine all resulting directives into a single working specification.
- **Runbook-form recipe**: the recipe's own content already is the specification — follow it directly, no Step 4 compilation needed.

Either way, proceed with the requested task using the resulting specification.

If a bundle-form Recipe references a fingerprint type with no entries at any scope, note the gap plainly rather than silently proceeding with an incomplete specification.

### Final Output (Step 5)

The requested task, produced using either the combined Compiled Fingerprint Directives or the recipe's own embedded runbook as the governing specification.

---

## Step 6: Trigger a Recipe From Chat (On Demand)

This step defines how a Recipe (Step 5) gets invoked in an ordinary chat turn, across any AI client reading this file. It does not change what a Recipe is or how it is compiled — it only defines the trigger surface sitting in front of Step 5.

`::` is used as the shorthand prefix (not `/` or `//`), since a leading `/` is reserved by several clients (Cowork, Copilot, and likely others) for their own built-in command syntax and would otherwise be intercepted before reaching the model.

### Recognized Trigger Forms

1. **Double-colon shorthand** — a message beginning with `::[recipe-name]` followed by an optional argument, e.g. `::principal-engineer-repo-onboarding`, `::architect-module finance dashboard categorizer`. Treat `[recipe-name]` as the exact `RECIPE_NAME` to search for in Step 5, and treat everything after it as the task argument.
2. **Natural language** — a request that clearly matches a known recipe's task, phrased normally, e.g. "let's onboard onto this repo like a principal engineer." Match by intent against the known recipe names for the current scope, not by exact wording.

Both forms trigger the same Step 5 execution. Double-colon shorthand is a fast, unambiguous path; natural language is the fallback and remains available even when the user doesn't know or use the shorthand.

### Recipe Name Discovery

Before matching a trigger, determine which recipe names actually exist rather than assuming a fixed list — this file must stay unchanged across every coding repo, so it cannot hardcode recipe names. Run a `search_thoughts` query for `"recipe for"` at each applicable level — ROLE (across all six roles if the role isn't stated), ROLE+REPO, and ROLE+REPO+PROJECT when `HasProject` — filter to results containing the literal substring `| recipe for |`, and collect the distinct `RECIPE_NAME` values found, along with which role each belongs to. Treat this as the working registry of known recipes for the current session; refresh it if the user references a recipe name not yet in the registry. (The `::r` shortcut in Step 7 runs this same discovery on demand and prints the registry directly.)

### Argument Handling

Whatever follows the recipe name (double-colon shorthand) or is stated as the task specifics (natural language) becomes the task argument passed into Step 5's "requested task" execution. Apply any repo- or project-specific normalization a recipe's own fixed instructions define rather than passing the raw argument through unexamined.

### No Implicit Chaining

Triggering one recipe does not automatically trigger another. Recipes are discrete, chat-triggered steps — after one completes, the user explicitly triggers the next (e.g., running `::architect-module` and reviewing it before separately running `::prototype-module`). This step does not define scheduled or automatic recipe execution; it only covers a recipe fired from within a live conversation, once, on request.

### If No Match Is Found

If the trigger doesn't match any recipe in the working registry, say so plainly and ask whether the user wants to name an existing recipe, build a new one, or proceed without a recipe (running the task ad hoc). Do not silently guess which recipe was meant.

---

## Step 7: List Known Items By Type (Shortcut On Demand)

Shortcut triggers for browsing what already exists in OB1 as a directory listing, without retrieving or compiling full content. Recognize these exact double-colon shortcuts:

- `::r` — list all known Recipes
- `::i` — list all known Instruction Fingerprints (by category)
- `::s` — list all known Skill Fingerprints (by category)
- `::a` — list all known Agent Fingerprints
- `::p` — list all known Prompt Fingerprints (process-capture fragments — rare; most repos won't have any yet)
- `::f` — list all known Role Fingerprint categories, per role (`Architect Fingerprint`, `Developer Fingerprint`, etc.)
- `::?` or `::help` — print this shortcut menu itself

### Scope Constraint

Every listing shortcut is constrained to the current `ScopeOrder` ladder — ROLE (all six unless one is specified), ROLE|REPO, and ROLE|REPO|PROJECT when `HasProject` — the same ladder Steps 2-5 already use. Never include another repo's items in a listing. If the user wants to check a different repo, that requires switching to that repo's own `bootstrap-variables.instruction.md` first, not a listing shortcut.

### Execution

For the requested type, run `search_thoughts` at each applicable scope level using a query built from that type's marker word:
- `::r` -> `"[scope] recipe for"`, Marker Filter: literal substring `| recipe for |`
- `::i` -> `"[scope] Instruction Fingerprint"`, Marker Filter: literal substring `| Instruction Fingerprint |` (also include fixed instructions matching `| instruction for |`)
- `::s` -> `"[scope] Skill Fingerprint"`, Marker Filter: literal substring `| Skill Fingerprint |`
- `::a` -> `"[scope] Agent Fingerprint"`, Marker Filter: literal substring `| Agent Fingerprint |`
- `::p` -> `"[scope] prompt for"`, Marker Filter: literal substring `| prompt for |`
- `::f` -> `"[Role] Fingerprint"` for each role, Marker Filter: literal substring `| [Role] Fingerprint |`

From the filtered results at every applicable scope level, extract the distinct name — `RECIPE_NAME` for recipes, `Category` for fingerprints, the fixed instruction's own name for `instruction for` entries — and the scope level (and role, where relevant) at which each was found.

#### Search Reliability (tuning note, confirmed on first live use in this repo)

A single semantic `search_thoughts` call at default settings (`limit: 10`, `threshold: 0.5`) is **not reliable enough to enumerate every item** — confirmed directly in this repo: a query for a known, distinctively-worded engram only surfaced it at position 9 of 30 once `limit` was widened to 30 and `threshold` lowered to 0.2; at default settings it would likely have been missed or reported as "not found." When running Step 7 (or Step 6's Recipe Name Discovery, or Steps 2-5's own scope queries), use a higher `limit` (25-30) and a lower `threshold` (0.2-0.3) than the tool defaults, and treat the marker-filtered result set as a floor, not a ceiling — if the count seems low relative to what's expected, re-run with an even lower threshold or a more targeted query before concluding an item doesn't exist. Do not report "not found" from a single default-settings query.

#### Superseded Entries

Some engrams carry `Status: override` specifically to mark an earlier entry (often at the wrong scope) as superseded. When listing, do not present an `override`-status entry as if it were an active item in its own right — either omit it from the flat list, or show it only as a note under the entry it supersedes. The goal is that a listing never makes a superseded entry look like a second, competing item.

### Output

A flat list grouped by scope level (and role, for anything not already role-specific in its query), each item labeled with where it was found. This is a directory listing, not a resolution: do not deduplicate across scopes, compile content, or resolve conflicts (that is Step 4's job, not Step 7's). If the requested type has zero entries anywhere, say so plainly rather than returning an empty list silently.

Example output shape for `::r` (as of the first recipe captured in this repo):
```
Recipes:
- principal-engineer-repo-onboarding — Consultant
```
