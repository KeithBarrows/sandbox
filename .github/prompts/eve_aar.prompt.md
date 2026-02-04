# Prompt for Adding AAR to YAML

Use the following instructions to add After Action Reports (AARs) to the `sc.yaml` file. Follow the structure of the examples provided below. Ensure proper YAML formatting and indentation.

## Instructions:
1. Each AAR entry should be added as a new item under the `events` list in the `_scratchpad\sc.yaml` file.
2. Use the following keys for each AAR:
   - `date`: The date and time of the event in the format `MM/DD/YYYY HH:MM AM/PM`.
   - `title`: The title of the AAR, starting with `AAR:` followed by the event name.
   - `organizer`: The name of the organizer.
   - `description`: A detailed description of the event. Use `>` for multi-line text.
   - `roster`: A list of participants categorized by roles (e.g., `command`, `guide`, `scout`, `attendees`).
   - Optional: Include `ISK` and `ISK per Pilot` fields if the event involves loot distribution.
     - If there is a total ISK value but no ISK per pilot, calculate the ISK per pilot as:
       ```
       ISK / (pilot count + 1)
       ```
3. Ensure proper indentation and alignment for YAML compliance.

## Examples:

### Example 1: Basic AAR
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

### Example 2: AAR with ISK
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