---
description: Generate annual EVE Online AAR report for specified year
---

# Generate Annual AAR Report

This workflow analyzes all AARs for a specified year and generates a comprehensive annual report.

## Prerequisites

1. The year-specific AAR file must exist: `_scratchpad\sc-aar-[YYYY].yaml`
2. User must specify the target year (e.g., 2025, 2026)

## Workflow Steps

### 1. Identify Target Year

Ask the user which year to analyze if not already specified.

### 2. Verify Source File Exists

Check that `_scratchpad\sc-aar-[YYYY].yaml` exists. If not, inform the user and exit.

### 3. Count Total Fleets

Extract all AAR entries by counting instances of `title: "AAR:` pattern in the file.

### 4. Calculate Total ISK

- Search for all `ISK: ` fields in the YAML file
- Sum all ISK values found
- Convert to billions (divide by 1,000,000,000)
- Round to 2 decimal places

### 5. Categorize Fleet Types

For each AAR title, categorize into:

**Wormhole Operations** - Match patterns:
- `C\d` (C1, C2, C3, C4, C5, C6)
- `spider`
- `drake`
- `bumble`
- `frigate swarm`
- `armor fleet`
- `shield fleet`
- `wormhole`

**Trip Tik Tours** - Match patterns:
- `trip tik`
- `TTGT`
- `tik`

**Mission Running** - Match patterns:
- `mission`
- `SOE`
- `SoE`
- `LP`
- `sisters of eve`

**Hugs Fleets** - Match patterns:
- `hug`
- `hugen`
- `certain death`
- `midnight`

**Other** - Everything else

### 6. Identify Top ISK-Generating Fleets

- Extract all AARs with ISK values
- Sort by ISK amount (descending)
- Take top 5
- Format as: `Fleet Name: ISK amount (XB)`

### 7. Document Ship Losses

Search AAR descriptions for loss indicators:
- `lost`
- `loss`
- `losses`
- `destroyed`
- `killed`
- `RUD` (Rapid Unscheduled Disassembly)
- `wipe`
- `fell`
- `falling`

For each incident found:
- Extract fleet name
- Extract loss details from description
- Create brief summary

### 8. Identify Notable Achievements

Look for:
- Zero-loss fleets with high ISK
- First-time accomplishments
- Special events (Katia Sae, monument visits, etc.)
- Doctrine milestones
- Community events

### 9. Generate Report File

Create `_scratchpad\sc-aar-[YYYY]-report.md` with the following structure:

```markdown
# [YYYY] EVE Online Fleet Activity Report
## Signal Cartel After Action Review

---

## Executive Summary

- [Total] total fleets
- [Total ISK]B ISK recovered
- [Count] wormhole ops ([%]%)
- [Count] Trip Tik tours ([%]%)
- [Count] mission runs ([%]%)
- [Count] hugs fleets ([%]%)
- Top: [Fleet Name] ([ISK]B)
- ~[Count] ships lost
- [Notable achievement 1]
- [Notable achievement 2]

---

## Fleet Type Breakdown

### Wormhole Operations: [Count] fleets ([%]%)
[Description of wormhole operations]

**Notable Achievements**:
- [Achievement 1]
- [Achievement 2]
- [Achievement 3]

### Trip Tik Guided Tours: [Count] fleets ([%]%)
[List of tours]

### Mission Running: [Count] fleets ([%]%)
[List of mission fleets]

### Hugs Fleets: [Count] fleets ([%]%)
[List of hugs fleets]

### Other Operations: [Count] fleets ([%]%)
[List of other operations]

---

## Financial Performance

### Top ISK-Generating Fleets:
1. **[Fleet 1]**: [ISK] ISK ([X]B)
2. **[Fleet 2]**: [ISK] ISK ([X]B)
3. **[Fleet 3]**: [ISK] ISK ([X]B)
4. **[Fleet 4]**: [ISK] ISK ([X]B)
5. **[Fleet 5]**: [ISK] ISK ([X]B)

### ISK Distribution:
- **Wormhole Operations**: ~[X]B ISK ([%]% of total)
- **Mission Running**: Minimal ISK tracking (LP-focused)
- **Other**: ~[X]B ISK

---

## Ship Losses

### Documented Loss Incidents: [Count] events

[For each loss incident:]
[Number]. **[Fleet Name]**: [Ship count/type]
   - [Brief description of circumstances]

**Estimated Total Losses**: [Range] ships across the year

---

## Notable Achievements & Milestones

### Doctrine Development:
- [Doctrine evolution 1]
- [Doctrine evolution 2]

### Community Events:
- [Event 1]
- [Event 2]

### Training & Development:
- [Training accomplishment 1]
- [Training accomplishment 2]

---

## Key Trends

1. **[Trend 1]**: [Description]
2. **[Trend 2]**: [Description]
3. **[Trend 3]**: [Description]
4. **[Trend 4]**: [Description]
5. **[Trend 5]**: [Description]

---

## Recommendations for [YYYY+1]

Based on [YYYY] performance:
1. [Recommendation 1]
2. [Recommendation 2]
3. [Recommendation 3]
4. [Recommendation 4]
5. [Recommendation 5]

---

*Report compiled from [Count] After Action Reports spanning January - December [YYYY]*
```

### 10. Validate Report

- Verify all percentages add up to 100%
- Ensure all ISK calculations are accurate
- Check that all sections are populated
- Confirm file was created successfully

### 11. Notify User

Inform the user that the report has been generated at:
`_scratchpad\sc-aar-[YYYY]-report.md`

## Output Format

The report should be saved as: `_scratchpad\sc-aar-[YYYY]-report.md`

## Notes

- All ISK values should be formatted with commas for readability in detailed sections
- Percentages should be rounded to nearest whole number
- Executive summary bullets must be ≤35 characters each
- Ship loss estimates should be conservative (use ranges)
- Notable achievements should focus on unique or exceptional accomplishments
