---
description: Add After Action Report (AAR) to EVE Online sc.yaml
---

# Add EVE Online AAR to YAML

> [!CAUTION]
> **Each AAR is completely unique and independent.** Absolutely NO information from any previous AAR should be carried over, assumed, or used when processing a new AAR. Only use the information explicitly provided in the current AAR text. Do not infer pilot names, ship types, or any other details from previous entries.

Use the following instructions to add After Action Reports (AARs) to year-specific YAML files in the `_scratchpad` directory. Follow the structure of the examples provided below. Ensure proper YAML formatting and indentation.

## File Naming

**IMPORTANT**: AARs should be saved to year-specific files based on the year portion of the AAR date:
- Extract the year (YYYY) from the AAR's date field
- Save to `_scratchpad\sc-aar-[YYYY].yaml` (e.g., `sc-aar-2025.yaml` for events in 2025)
- If the file doesn't exist, create it with the proper YAML structure (starting with `events:`)
- Each file contains all AARs for that specific year

## Pilot Alias Lookup

AAR authors often use abbreviated names or nicknames for pilots (e.g., "Dude" for "@Dude Johnson", "Lav" for "@Lavoleine Audier", "FC" for the fleet commander).

1. **Reference the alias file**: Before processing each AAR, check `_scratchpad\sc pilot alias lookup.json` to resolve any nicknames or abbreviations to their full pilot names.

2. **Update the alias file**: When you encounter a nickname or abbreviation in an AAR that is not yet in the lookup file, add it. Each pilot can have multiple aliases.

3. **File format**: The JSON file should map full pilot names (with @ prefix) to an array of their known aliases:
   ```json
   {
     "@Dude Johnson": ["Dude"],
     "@Lavoleine Audier": ["Lav"],
     "@Millan Abureque": ["Millan"]
   }
   ```

4. **When to add aliases**: Add an alias when it appears prominently in the AAR narrative (e.g., used multiple times or clearly referring to a specific pilot in context).

5. **Do NOT use role names as aliases**: Never add the following role names as pilot aliases, as they refer to fleet positions, not specific individuals:
   - FC (Fleet Commander)
   - Logi (Logistics)
   - Salvage
   - Wing Commander
   - Fleet Commander
   - XO (Executive Officer)
   - Scout
   - Any other fleet role designation

## Instructions

1. Each AAR entry should be added as a new item under the `events` list in the appropriate year-specific file (e.g., `_scratchpad\sc-aar-2025.yaml`).

2. Use the following keys for each AAR:
   - `date`: The date and time of the event in the format `MM/DD/YYYY HH:MM AM/PM`
   - `title`: The title of the AAR, starting with `AAR:` followed by the event name
   - `organizer`: The name of the organizer
   - `description`: A detailed description of the event. Use `>` for multi-line text
   - `roster`: A list of participants categorized by roles (e.g., `command`, `guide`, `scout`, `attendees`)
   - Optional: Include `ISK` and `ISK per Pilot` fields if the event involves loot distribution
     - If there is a total ISK value but no ISK per pilot, calculate the ISK per pilot as:
       ```
       ISK / (pilot count + 1)
       ```
     - If there is an ISK per pilot value but no total ISK, calculate the total ISK as:
       ```
       ISK per pilot * (pilot count + 1)
       ```

3. Ensure proper indentation and alignment for YAML compliance.

## Example 1: Basic AAR

```yaml
  - date: "1/12/2025 8:36 AM"
    title: "AAR: Trip Tik Guided Tour n°3, In Rust We Trust"
    organizer: "Nerialka"
    description: >
      The third Trip Tik Guided Tour went very well thanks to our Guide, @Jen Hoshi
      who introduced us to magnificent views of Minmatar Space and led us to discover
      interactive Trip Tik sites!
      Our scout helped to clear one of them from the little rat that lives there so we
      could jump the acceleration gate safely, thanks a lot @Strelnikova!
      Another special mention for the effort of all the attendees who brought a large
      motley crew of rust-and-duct-tape ships. The fleet was a sight in itself!
    roster:
      command:
        - name: "@Nerialka"
          ship: "Hound"
      guide:
        - name: "@Jen Hoshi"
          ship: "Svipul"
      scout:
        - name: "@Strelnikova"
          ship: "Probe"
      attendees:
        - name: "@Atvas Rotsuda"
          ship: "Prowler"
        - name: "@Azotox"
          ship: "Tornado"
        - name: "@Bram Boreillo"
          ship: "Cyclone"
        - name: "@Dizzy Dorasdottir"
          ship: "Wolf"
```

## Example 2: AAR with ISK

```yaml
  - date: "2/9/2025 10:12 PM"
    title: "AAR: Heavy Shield Fleet 1: C6 Magnetar"
    organizer: "Arachnis"
    description: >
      This was the first fleet with the new "Heavy Shield" battlecruiser doctrine, in a C6 magnetar to boot.
      It was certainly engaging; let's put it that way. 😛

      We ran 2 anomalies very quickly, including drifters. The main strength of this shield setup over armor is
      certainly its raw firepower. Sleepers and drifters alike crumbled like wet paper. Unfortunately, 2 Basis
      weren't quite enough to handle the incoming damage from a C6 relic site, and we lost both our logi in the
      last wave. @Dude Johnson and @Strelnikova both put up a heroic effort, even somehow managing to save my
      Cenotaph mid-hull. Sadly, no amount of effort can beat simple math. To run C6 relics safely with this
      doctrine and likely the armor version as well, 3 logi are needed (datas and anoms are easier).

      Unable to run sites with no logi, we evacuated the system to regroup and reship. Bob wasn't done entertaining
      Himself yet: our exit wormhole into the C2 leading back to HS collapsed right as the fleet was jumping through
      it, leaving some members stranded, though they simply scanned down the new C2 static and got a HS connection
      in a system right next to our old one. After people reshipped, we went back in and right back out after I made
      a bad target call in a Core Bastion and accidentally spawned the next wave early. Oops!

      All that said, we still made out with 1,559,999,354 ISK in loot, coming out to 74,285,683 per pilot. Thank you
      @Graphite for the consistently great looting work, and thank you everyone for coming!
    ISK: 1559999354
    ISK per Pilot: 74285683
    roster:
      attendees:
        - "@Arachnis"
        - "@Graphite"
        - "@Ashtoreth Nyah"
        - "@Bram Boreillo"
        - "@Dizzy Dorasdottir"
        - "@Dude Johnson"
        - "@SolutionD"
        - "@Horatio Corporatio"
        - "@KilroyJenkins"
        - "@Lexington Braddock"
        - "@Mzsbi Haev"
        - "@Nevare Yaochen"
        - "@Luber"
        - "@Nihaan Kumar"
        - "@pris Naari"
        - "@Sir Fiddle Sticks"
        - "@Strelnikova"
        - "@Sund Winds"
        - "@Troden Treadwell"
        - "@Yoshi Katelo"
```

## Workflow Steps

1. Extract the year (YYYY) from the AAR's date field
2. Determine the target file: `_scratchpad\sc-aar-[YYYY].yaml`
3. If the file doesn't exist, create it with the structure: `events:`
4. Open the appropriate year-specific file
5. Locate the `events` list
6. Add a new AAR entry following one of the examples above
7. Ensure proper YAML indentation (use 2 spaces per level)
8. If ISK distribution is involved, calculate ISK per pilot if not provided
9. Validate YAML syntax before saving