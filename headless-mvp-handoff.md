# HEADLESS — MVP Handoff Spec
### Add a boss fight to a proven toy
*(“Headless” is a placeholder working title — rename freely.)*

---

## 0. For the implementing instance (read first)

You have Unity MCP access; the human does **not**. Your job is a **greybox vertical slice** built on an existing, working project.

**The base project is `Platformer45`.** Open and inspect it before writing anything. It already contains, working and *already proven fun*:
- a third-person character controller,
- **head-detach** scripts (the head pops off the skeleton), and
- **head-as-a-rolling-ball** movement that can **pick up scattered body parts**.

**Do not reinvent any of that.** The core toy already exists and already makes the human laugh. This MVP is about **fitting that toy into a boss-fight loop** — plus a few additions (a toy weapon, a bullet-time throw, and a boss). There is no “is the mechanic fun?” risk here — it’s proven. The risk is purely *tuning the boss loop around it*.

---

## 1. The game in one line

A comedy-horror jank **“soulslike”**: you’re a skeleton, and combat is a juggling act between attacking and not falling apart. The bad controls are **intentional** — jank as joke and challenge (QWOP / *Getting Over It* lineage), grotesque-silly horror tone.

---

## 2. What already exists (reuse, don’t rebuild)

- WASD moves the **whole body**; mouse aims the camera (third-person controller).
- **Head detaches** from the body.
- Detached head **rolls like a ball** and can **pick up body parts**.

---

## 3. What to ADD for the MVP

### 3a. A toy melee weapon
- Give the skeleton a **tiny wooden sword** — deliberately a *toy*, comically small against a giant boss.
- **Weak** damage. Crucially: a determined player **can** solo the boss with *just* the sword — it’s just slow and tedious.
- This is not a throwaway feature — it’s a **stealth difficulty slider**: players who can’t nail the head-throw timing can grind it out with the stick. Accessibility disguised as dark comedy (“I killed a giant with a toothpick”). It also means boss HP / head-bite damage don’t need perfect tuning — the system self-corrects.

### 3b. Head detach → BULLET-TIME aim → throw (the attack)
- WASD + mouse control the whole body normally.
- Press the **detach button**: the head ejects (little pop/jump) **and the whole game slows to bullet-time.**
- During slow-mo, **move the mouse** to aim within a range/cone — the player watches the arc in real time at reduced speed.
- **Click** to fire the head **as a rolling ball** in the aimed direction.
- Goal of the throw: land the head on the (much larger) boss and **bite a weakpoint** (e.g. an ear) for real damage.
- **Why bullet-time (not arrow keys):** it’s instantly legible (everyone knows Max Payne / Superhot), the slow-mo *is* the aim-feedback (the player sees where the head will go before committing), and slowing the universe to precisely lob a skull is inherently funny — Max Payne meets Looney Tunes. The chaos after it lands stays janky and unpredictable; only the *aiming moment* is deliberate.

### 3c. The vulnerability loop — TWO ways to get scattered (this is the soul)
- **Way 1 — you detach to attack.** The moment the head ejects, the **body disassembles** and you **cannot jump/attack** — you’re just a head plus loose parts, **exposed**. After biting, the head must **roll fast to gather the parts** (reuse the existing pickup) and **reassemble** before you can act again. Every head-attack is a **one-way commitment** until you rebuild.
- **Way 2 — the boss hits you while you’re whole.** A landed boss attack triggers a **big explosion** that blasts your bones across the level — ribs fly, skull rolls off, femur ends up across the room. Same recovery: roll the head around and gather the parts before you can jump again. (Getting hit while *already* disassembled is a non-threat — you’re scattered anyway.)
- **Why this matters:** it turns dodging into a real decision, not a background chore. Attack now and accept the reassembly risk — or dodge and stay whole to keep your next shot, but risk getting blown apart with nothing to show for it. That’s the tension the boss loop runs on.

---

## 4. The sample boss

- **One** boss, much larger than the player. Greybox blockout is fine.
- Clear **weakpoint** the thrown head bites (ear / head).
- **Three telegraphed special moves** on a timer (e.g. a charge, a jump-slam, and one more — the specifics don’t matter for the MVP; readability does). If any lands while the player is **whole**, it triggers the **explosion scatter** from §3c.
- Simple **health bar**; simple state machine: `idle → telegraph → move → recover`, cycling through the three moves.
- Beatable in ~2 min via head-bites once the rhythm clicks — or slowly, via the stubborn wooden sword.

---

## 5. Design guardrails — where this lives or dies

1. **Bullet-time duration is the #1 tuning target.** It must feel like a **power move** (“I get one slow-mo shot”), not a **pause menu** (“let me think for ten seconds”). Aim for ~1–2 seconds. Too long kills urgency; too short makes aiming feel rushed. The slow-mo already gives you the throw feedback for free — lean on it.
2. **“Intentionally janky” ≠ “broken.”** Consistent, fair, learnable, funny. Tune toward *“brilliantly frustrating, I must conquer this,”* never *“broken garbage.”*
3. **Reassembly pickup stays generous and simple** — proximity grab that clunks into place. Comedy is in the scramble, not precision. (Already how the existing pickup works — keep it.)
4. **The explosion scatter is a feel-call** — too chaotic and the player feels cheated; too tidy and it isn’t funny. It should read instantly as “oh no, I’m in pieces” and send parts far enough to be a real scramble but not a marathon.
5. **Keep the disassembled state readable** — the player must instantly understand “I can’t jump, I’m exposed, go get the parts.”

---

## 6. Build order

1. Confirm the existing Platformer45 toy runs (walk, detach, roll, pick up parts).
2. Add the **wooden sword** melee.
3. Add **detach → bullet-time → mouse-aim → click-throw**, and tune the slow-mo duration.
4. Add the **sample boss**: three telegraphed moves, weakpoint bite, health bar.
5. Add the **explosion scatter** when the boss lands a hit on a whole player.
6. Tune the **tension**: boss move timing vs. how long reassembly takes.

---

## 7. Success criteria

The toy is already fun — that greenlight is banked. The MVP succeeds if the **boss fight is tense and funny**: the bullet-time throw feels like a satisfying committed shot, the scramble to reassemble under threat (whether from detaching or from getting blown apart) is a comedy of panic, and beating the boss — by head-bite *or* by stubborn wooden sword — feels earned. If the bullet-time throw feels bad, fix that before anything else.

---

## 8. Out of scope for this MVP

No menus, no multiple bosses, no story, no horror set-dressing, no art pass, no sound design beyond placeholder feedback, no progression. Greybox only. Deliverable = the proven toy + sword + bullet-time throw, fighting one sample boss with three moves and the explosion-scatter.

---

## 9. Open decisions — flag back to the human after prototyping

- Bullet-time duration and aim-cone width.
- How much the throw auto-assists toward the weakpoint vs. fully manual aim.
- Explosion scatter radius / how far parts fly.
- Boss move set specifics and how hard it hits.
- Number of body parts to gather (whatever Platformer45 already uses is the default).
- Working title.
