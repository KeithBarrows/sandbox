# Finance Dashboard

A Windows desktop assistant that reads an existing [YNAB](https://www.ynab.com/) budget and turns it into proactive alerts and advice, instead of a ledger that only gets reviewed after the fact.

**Status:** Planning / pre-architecture. See [`docs/Executive_Brief_Overview.docx`](docs/Executive_Brief_Overview.docx) for the one-page pitch and [`docs/Initial_Project_Plan.docx`](docs/Initial_Project_Plan.docx) for full details, open decisions, and milestones.

## What it does

The project is split into three phases, each addressing a different gap in day-to-day budget management:

1. **Balance Guardian** — polls credit card balances every 6 hours and raises a tiered, color-coded alert (yellow at 50%, orange at 75%, red at 85% of the card's limit) before a statement closes or a limit is hit.
2. **Insights Advisor** — a slower-cadence (weekly/monthly) pass over the full budget history that surfaces category spending trends, flags where to cut back, evaluates whether income headroom supports larger loan payments, and produces a debt payoff plan (snowball/avalanche) across all tracked debts.
3. **Categorizer** *(concept — needs further design before any code)* — matches Amazon order confirmation emails to the corresponding YNAB transaction and proposes a corrected category (or split), since card-only data leaves every order tagged with whatever category was last used.

## Where AI fits in

AI is used deliberately, and only where it's actually the right tool — for **judgment and language tasks**, not arithmetic:

- **Trend summarization & prioritization** (Insights Advisor) — turning computed category/spending trends into a plain-language, prioritized narrative.
- **Item-to-category classification** (Categorizer) — reading an order's item description(s) against the real YNAB category list and proposing a category or split, a fuzzy classification problem that doesn't have a formula.

Everything with a right answer — debt snowball/avalanche ordering, payment-capacity math, threshold comparisons — is computed in deterministic code, never left to a model to calculate. AI-assisted proposals (especially in the Categorizer) are surfaced for review rather than applied silently; nothing rewrites budget history without a human confirming it first.

## Tech stack

- C# / .NET 10, Windows desktop (WinForms)
- [YNAB API](https://api.ynab.com/) — read-only personal access token
- No server component; runs entirely on the local machine
- Secrets (YNAB token, AI API keys) stored via Windows Credential Manager, never in plaintext config

## Repo layout

```
src/        one folder per project (app + shared libraries)
tests/      test projects, mirroring src/
docs/       planning docs (Executive Brief, Project Plan)
```

`src/` and `tests/` are scaffolded but empty — actual projects land here once Phase 1 architecture is finalized.

## License

MIT — see [`LICENSE`](LICENSE).
