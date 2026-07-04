---
status: reverse-documented
source: Docs/ExportBlock-9dcf8159-bb2f-4551-a735-261516a80aa2-Part-1/ (Notion design export), Assets/Scripts/ (implementation), design conversation 2026-07-03
date: 2026-07-03
verified-by: Mauro Zurlo
---

# Game Concept: Platformer45 (working title — the "platformer" name is legacy)

*Created: 2026-07-03*
*Status: Draft — direction confirmed 2026-07-03*

> **Note**: Reverse-engineered from a Notion story bible and the Unity codebase,
> then sharpened in a design conversation that resolved the game's identity. The
> project began life as a scattered 3D platformer; it has been **deliberately
> redefined as a cozy narrative adventure**. Sections that still lack a decision
> are marked **[OPEN]**.

---

## Elevator Pitch

> A cozy, funny narrative adventure where you play a sarcastic skeleton stuck in
> Hell's bureaucracy — explore charming little islands, talk to weird damned
> souls, and solve gentle puzzles (including detaching your own head) to earn
> your way out... only to find the exit is worse than where you started.

---

## Core Identity

| Aspect | Detail |
| ---- | ---- |
| **Genre** | Cozy narrative adventure with light puzzles and forgiving traversal. Tone from LucasArts (*Grim Fandango*, *Monkey Island*); **structure from *Little Big Adventure*** (explore → talk → solve → advance story). |
| **Platform** | PC (Steam / itch.io) |
| **Target Audience** | Players who want a funny, chill story game — including non-hardcore players. (Confirmed: early testers who loved the writing/art but disliked twitch controls ARE the target.) |
| **Player Count** | Single-player |
| **Session Length** | Relaxed, pick-up-and-play; a session = one NPC's questline. **[OPEN]** exact target. |
| **Monetization** | Premium (single-player narrative game) |
| **Estimated Scope** | Medium — three islands planned; only Island 1 designed/built |
| **Comparable Titles** | *Little Big Adventure* (structure), *Grim Fandango* / *The Secret of Monkey Island* (tone), *A Short Hike* (cozy, forgiving traversal) |

---

## Core Fantasy

You are a sarcastic, resigned skeleton stuck in the worst circle of a
cartoonish, bureaucratic Hell — and you're in on the joke. You can't fight your
way out and nothing here can really hurt you; you get through this world by
exploring it, charming (or annoying) its residents, and cleverly falling apart
— literally detaching your head — to solve its problems. The fantasy is
**comedic powerlessness turned into cleverness, with zero stress.**

---

## Unique Hook

**Your head comes off — and it's a puzzle and social tool, not a skill toy.**
Maki can detach his head from his body and control each separately. This is used
for:
- **Puzzle traversal**: roll the head through a gap the body can't fit, hit a
  switch, reach a high shelf.
- **Environmental interaction**: set the head on fire to burn obstacles / light
  infernal mechanisms.
- **Social/state puzzles**: some NPCs react differently — or won't talk to you
  at all — depending on whether your head is on. Solving is about *noticing*,
  not execution.

*"And also" test*: "It's a cozy LucasArts-style afterlife comedy adventure,
**and also** you solve its puzzles by taking your own head off."

---

## Player Experience Analysis (MDA Framework)

### Target Aesthetics (What the player FEELS)

| Aesthetic | Priority | How We Deliver It |
| ---- | ---- | ---- |
| **Narrative** (drama, story arc) | 1 | Redemption-quest structure, the crab-Devil twist, the bittersweet "promoted to a Hell you still don't want" ending |
| **Submission** (relaxation, comfort) | 2 | Cozy, forgiving, no-fail play; wandering a charming world at your own pace |
| **Fantasy** (make-believe, role-play) | 3 | Being an expressive skeleton in a gothic-cartoon Hell |
| **Discovery** (exploration, secrets) | 4 | Bounded islands with hidden items, flavor interactions, optional joke paths |
| **Challenge** (mastery) | 5 (low) | Only ever *puzzle* challenge (figuring out routes/combinations) — never execution challenge |
| **Sensation** | 6 | Stylized cartoon-infernal visuals and comedic audio |
| **Expression / Fellowship** | N/A | Not a build game; single-player |

### Key Dynamics (Emergent player behaviors)

- Players read each obstacle as "which tool — head, fire, an item, or the right
  NPC — do I need here?"
- Players explore for the joy of finding the next character and the next joke.
- Players chain crafting/quest steps by gathering ingredients across the island.

### Core Mechanics (Systems we build)

1. **Light, forgiving traversal + the detachable head** — assisted 3D movement
   (coyote time, ledge-grab, soft landings, no death) where the *head* is the
   puzzle/interaction verb. Traversal is texture and puzzle, never a skill gate.
2. **Dialogue & quests** — data-driven (CSV) dialogue, ScriptableObject quests. *Built.*
3. **Crafting / item combination** — gather ingredients, merge into tools. *Partially built — merge UI is stubbed and must be completed.*
4. **Adventure puzzles** — inventory / observation / social / state puzzles (NOT spatial-dexterity puzzles). *Partly built.*
5. **Quest-gated minigames** — fishing, metal detector, etc., used as flavorful ways to acquire quest items — not the core loop.

---

## Player Motivation Profile

### Primary Psychological Needs Served

| Need | How This Game Satisfies It | Strength |
| ---- | ---- | ---- |
| **Relatedness** | Bond with a cast of funny, sympathetic NPCs through comedic questlines | Core |
| **Autonomy** | Explore at your own pace; choose puzzle approaches; optional joke paths | Supporting |
| **Competence** | Satisfying *aha* moments from puzzles — gentle, never punishing | Supporting |

### Player Type Appeal (Bartle Taxonomy)

- [x] **Explorers** — discovering the world, its characters, and its absurd flavor
- [x] **Achievers** — completing quest chains and crafting objectives
- [ ] **Socializers / Killers** — N/A (single-player, no combat)

### Flow State Design

- **Onboarding**: The Judge intro sets the goal; a gentle first quest (Púbol)
  teaches talk → gather → craft → return.
- **Difficulty**: Flat and low — difficulty comes from *puzzle insight*, not
  execution. The player is never blocked by skill.
- **Feedback clarity**: Floating `<?>` markers / blinking objects signal
  interactables.
- **Recovery from failure**: There is essentially no failure. No death, no
  combat; falls are harmless; save points exist as convenience, not as a safety
  net against punishment.

---

## Core Loop

### Moment-to-Moment (30 seconds)
Wander a cozy, characterful space; walk up to NPCs and objects; enjoy funny
dialogue; do gentle assisted traversal.

### Short-Term (5–15 minutes) — "the quest turn"
An NPC has a (usually absurd) problem → you learn what's needed → you go
find/make/solve it via a light puzzle, a head-detach trick, or a quest-gated
minigame → you return → funny payoff + the world and story change.

### Session-Level (30–120 minutes)
Complete a full NPC questline (e.g. all three of Púbol's art-tool quests) and
advance the redemption story.

### Long-Term Progression
Help the three skeletons → earn the Devil's demolition permit → clear the exit →
the twist ending. New head/traversal tricks unlock access to new spots.

### Retention Hooks
- **Curiosity**: undesigned Islands 2 & 3; how Maki got here; the crab conspiracy
- **Investment**: quest progress and NPC relationships
- **Comfort**: it's a nice, funny place to spend time

---

## Game Pillars

### Pillar 1: Absurd Bureaucratic Afterlife
Humor from mundane bureaucracy colliding with a fantastical Hell — paperwork,
permits, unions, and a Devil who is literally a crab.

*Design test*: If a scene isn't funny or doesn't reinforce "Hell is annoying,
not scary," rework it.

### Pillar 2: One Skeleton, Two Tools
The detachable head/body split is the signature verb — used for puzzles,
interaction, and social gags, **never as a twitch-skill mechanic.**

*Design test*: If a puzzle could be solved identically in any other game,
redesign it around the head. If it requires precise execution, redesign it to
require *thought* instead.

### Pillar 3: Character-Driven Comedy
The heart is the cast — Púbol, Juana, Franc, the Judge — and their absurd but
sympathetic problems. Comedy carries emotion.

*Design test*: If content advances plot but doesn't deepen a character or land a
joke, question whether it earns its place.

### Pillar 4: Cozy & Forgiving
No death, no combat, no fail states that cost the player progress or ask for
reflexes. The player is never blocked by execution — only invited to think.

*Design test*: If a mechanic can *punish* the player (lost progress, a redo, a
timing gauntlet), soften it or cut it.

### Anti-Pillars (What This Game Is NOT)

- **NOT a challenge platformer**: Traversal is light, assisted, and forgiving.
  The scrapped precision-platforming (cemetery level) and sokoban puzzles stay
  scrapped — they fought this pillar.
- **NOT combat / not punishing**: `PlayerCharacter.cs`'s `ATTACKING`/`DEAD`
  states and `health`/`stamina` are vestigial **tech debt** to remove — not a
  design direction.
- **NOT open-world**: Bounded, hand-crafted islands.
- **NOT point-and-click**: Despite the source doc's tag, the game is
  direct-control 3D (LBA-style), and that is intentional.

---

## Inspiration and References

| Reference | What We Take From It | What We Do Differently | Why It Matters |
| ---- | ---- | ---- | ---- |
| *Little Big Adventure* | **Structure**: direct-control 3D adventure blending exploration, dialogue, and puzzles into one loop; charming world | Comedic-infernal skin; head-detach verb; no combat | The structural north star — proves variety can live under one adventure spine |
| *Grim Fandango* | Afterlife setting, deadpan comedy tone, stylized characters | Real-time 3D; head/body mechanic | Validates comedic-afterlife adventure |
| *The Secret of Monkey Island* | Absurd multi-step inventory puzzles, insult comedy (the Devil confrontation is explicitly Monkey-Island-styled) | 3D traversal, physical puzzles | Validates layered comedic puzzle design |
| *A Short Hike* | Cozy, forgiving, no-fail traversal; movement as small joy | Story/quest-driven rather than open ramble | Proves gentle traversal reads as cozy, not shallow |

**Non-game inspirations**: Gothic cartoon aesthetic; bureaucratic comedy
(paperwork, unions, permits as the "enemies").

---

## Target Player Profile

| Attribute | Detail |
| ---- | ---- |
| **Gaming experience** | Casual → mid-core. Explicitly includes people who love story/art but dislike twitch challenge. |
| **What they're looking for** | A funny, cozy, low-stress story to spend a few evenings in |
| **What would turn them away** | Punishing controls, death, combat, precision platforming, being blocked by skill |
| **Current games they might play** | Cozy narrative/adventure games; LucasArts-style comedies |
| **Age / session availability** | **[OPEN]** — not yet defined |

---

## Technical Considerations

| Consideration | Assessment |
| ---- | ---- |
| **Engine** | Unity 2022.3.62f3 (LTS), C#, Built-in Render Pipeline — in use |
| **Key Technical Challenges** | Head/body dual-control + camera switching (built); **completing the crafting/merge system**; adding forgiving-traversal assists (ledge-grab, jump buffer); quest/save state across areas |
| **Art Style** | Cozy stylized 3D, cartoonish-infernal. Current character art is AI placeholder, intended to be replaced by human-made models |
| **Art Pipeline Complexity** | High (custom 3D) |
| **Audio Needs** | Moderate — comedic SFX, characterful VO potential |
| **Networking** | None |
| **Content Volume** | 3 islands planned; Island 1 designed (~10 NPCs, a crafting/quest chain, 2 minigames). Islands 2 & 3 undesigned. |
| **Controls note** | Prioritize forgiving feel over precision. The controls testers disliked were serving precision platforming; lowering the precision demand largely resolves the complaint. |

---

## Risks and Open Questions

### Design Risks
- **Puzzle design is the make-or-break skill** (and a known pain point — the one
  existing puzzle, the cemetery one, was a struggle). **Mitigation**: pursue
  *inventory / social / observation / state* puzzles (writing-adjacent, the
  designer's strength) and avoid *spatial / physics / dexterity* puzzles (the
  demonstrated weakness). The head-social puzzle idea is proof this works.
- Comedic tone depends on writing quality (source is Spanish-first; localization
  and voice consistency matter).
- Head verb must stay fresh across a whole game or the hook thins out.

### Technical Risks
- **Crafting merge UI is stubbed** — `DroppableMergeSlot.OnDrop()` is empty. The
  central "combine items into tools" interaction is not functional yet and is
  required for the core loop.
- **Zero automated test coverage** on ~5,800 lines.
- Vestigial combat scaffolding may cause confusion/bugs until removed.
- Save/load across islands + quest state unproven at full scope.

### Scope Risks
- Only 1 of 3 islands is designed. Islands 2 & 3 are empty headers.
- Democoin economy is entirely conceptual (no code); the "3M democoins" joke
  ending implies an economy that doesn't exist.

### Open Questions
- **Economy**: do democoins exist as a real system, or only narrative flavor?
- **Islands 2 & 3**: themes, NPCs, puzzles (Juana's and Franc's full questlines).
- Exact session-length / audience targets.

---

## MVP Definition

**Core hypothesis**: "A cozy, funny, forgiving adventure — explore, talk, solve
gentle puzzles with the head-detach verb — is fun and charming enough to carry
the game."

Because most of **Island 1** is already implemented, the MVP / vertical slice is
**Púbol's questline, start to finish** — one complete turn of the core loop that
proves the game is cozy *and* funny *and* works.

**Required for the vertical slice**:
1. Judge intro scene → goal setup (tone-setting)
2. Maki's forgiving traversal + at least one meaningful **head-detach** use
3. Púbol's three art-tool quests (Lienzo → Pincel → Pinturas): talk → gather →
   solve a light puzzle / do a quest-gated minigame (e.g. fishing for squid-ink
   paint) → craft → return → funny payoff
4. **Functional crafting/merge** (currently stubbed — the one true blocker)
5. Dialogue + quest tracking (built)
6. A save point + the fast-travel tent

**Explicitly NOT in the slice**:
- Islands 2 & 3, full democoin economy, Juana's/Franc's later questlines
- Any combat, death, or precision-platforming challenge

### Scope Tiers

| Tier | Content | Features | Timeline |
| ---- | ---- | ---- | ---- |
| **Vertical Slice** | Púbol's questline on Island 1 | Forgiving traversal, head verb, dialogue, quests, **working crafting**, one minigame | **[OPEN]** |
| **Full Island 1** | All Island 1 NPCs & content | + remaining NPCs, minigames, save/travel | **[OPEN]** |
| **Alpha** | All 3 islands, placeholder art | All questlines rough, twist ending | **[OPEN]** |
| **Full Vision** | Complete, human-made art | Polished comedy, all endings | **[OPEN]** |

---

## Next Steps

- [ ] Carve the vertical slice (Púbol's questline) down to a finishable scope
- [ ] Complete the stubbed crafting merge system (core-loop blocker)
- [ ] Add forgiving-traversal assists (ledge-grab, jump buffer, remove fall death)
- [ ] Design one head-detach puzzle and one head-social gag for the slice
- [ ] Run `/map-systems` to decompose into per-system GDDs
- [ ] Register the cast + key items in `design/registry/entities.yaml`
- [ ] Plan the slice as the first milestone (`/sprint-plan`)
