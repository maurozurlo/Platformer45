# Sprint 1 — 2026-07-03 to 2026-07-24

## Sprint Goal
Close one complete turn of the core loop — Púbol's canvas quest, start to finish — proving the cozy adventure loop is fun and technically sound. This is the vertical slice.

## Capacity
- Total days: 15 (3-week solo sprint)
- Buffer (20%): 3 days reserved for unplanned work
- Available: 12 days
- *Estimates are rough ideal-days — solo velocity varies; treat as relative sizing.*

## Tasks

### Must Have (Critical Path — the loop must close)
| ID | Task | Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------|-----------|-------------|---------------------|
| 1-1 | **Inventory in gameplay scene** — extract the inventory UI (InventoryCamera + canvas + slots + merge slot) from `Inventory.unity` into a **persistent overlay prefab**, instantiated once under a `DontDestroyOnLoad` bootstrap alongside `GameControl`/`I18nManager`. `InventoryUI.control` survives scene loads. | gameplay/ui-programmer | 2 | — | Open/close inventory (I key) works inside `new maki` (main gameplay scene); the merge slot and draggable items are present and functional there; inventory data still persists via `GameControl`. |
| 1-2 | **Wire up crafting/merge** — connect merge-slot drop → `MergeItems()`, add a craft/confirm button + success feedback; retire the `J` debug trigger. Author `BasicItem` assets for Branches, Sheet, Canvas (`Canvas.canBeMadeFromItems` = branches + sheet). | gameplay/ui-programmer | 2.5 | 1-1 | Dropping branches + sheet into the merge UI and confirming produces a Canvas, consumes both ingredients, and shows feedback. Merge logic covered by a passing unit test. |
| 1-3 | **Blue House head-gate** — a trigger that only the detached skull can pass; the full body is blocked. The sheet lives inside. | gameplay-programmer | 2 | — | Player cannot reach the sheet with head attached; can enter and retrieve it as the detached head; re-attaching works cleanly. |
| 1-4 | **Gather points** — pickup interactions: branches from the park tree, sheet inside the Blue House. Both add to inventory. | gameplay-programmer | 1 | 1-1, 1-3 | Interacting with the tree gives branches; the sheet is grabbable only as the head; both appear in the inventory. |
| 1-5 | **Púbol questline end-to-end** — Púbol gives the canvas quest via dialogue; the quest tracks branches + sheet; returning the crafted canvas completes it with a funny payoff line. | gameplay-programmer + writer | 2 | 1-2, 1-4 | Full loop playable start to finish: talk → gather → craft → return → payoff + quest marked complete. |

### Should Have (cozy feel — loop can close without these)
| ID | Task | Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------|-----------|-------------|---------------------|
| 1-6 | **Forgiving traversal** — add jump buffering + ledge-grab/vault assist; make falls harmless in the slice (soft reposition, no death). | gameplay-programmer | 2 | — | No fall kills the player; missed jumps are recoverable; ledge assist makes standard jumps reliable. |
| 1-7 | **Judge intro opener** — minimal tone-setting Judge dialogue that states the goal and hands off to Púbol. | gameplay-programmer + writer | 1 | — | New game opens on the Judge scene; dialogue sets the goal; transitions to the play space. |

### Nice to Have
| ID | Task | Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------|-----------|-------------|---------------------|
| 1-8 | **Strip vestigial combat** — remove `ATTACKING`/`DEAD`/`health`/`stamina` scaffolding from `PlayerCharacter`. | gameplay-programmer | 0.5 | — | Combat states/fields removed; project compiles; no gameplay regressions. |
| 1-9 | **Head-social gag** — one NPC reacts differently (or refuses to talk) based on head on/off, proving the social-verb idea. | gameplay-programmer + writer | 1 | 1-3 | An NPC gives distinct dialogue depending on head state. |

## Carryover from Previous Sprint
| Task | Reason | New Estimate |
|------|--------|-------------|
| — (Sprint 1, no carryover) | — | — |

## Risks
| Risk | Probability | Impact | Mitigation |
|------|------------|--------|------------|
| Inventory extraction from its scene is fiddly (separate camera, singleton lifetime, commented-out `SetActive`) | Med | High | Persistent-prefab approach chosen; time-box a half-day spike first; the merge *logic* is already proven so scope is UI/plumbing |
| Puzzle/quest design (known pain point) stalls on the head-gate | Med | High | Keep the gate dead-simple (size/tag check, not a mechanism); lean on inventory/social logic, not spatial precision |
| Solo velocity slips | Med | Med | Must-Have (~9.5d) fits inside 12 available; Should/Nice-to-Have cut first |

## Dependencies on External Factors
- None (single-developer, local Unity project).

## Definition of Done for this Sprint
- [ ] All Must Have tasks completed
- [ ] All tasks pass acceptance criteria
- [ ] Full canvas-quest loop playable start to finish
- [ ] All Logic/Integration stories have passing unit/integration tests (merge logic test)
- [ ] Smoke check passed (`/smoke-check sprint`)
- [ ] No S1 or S2 bugs in delivered features
- [ ] `design/gdd/game-concept.md` updated for any deviations
- [ ] Code reviewed and merged

> ⚠️ **No QA Plan**: This sprint was started without a QA plan. Run `/qa-plan sprint`
> before the last story is implemented. The Production → Polish gate requires a QA
> sign-off report, which requires a QA plan.

## Review Mode
- **solo** — director/producer/QA gates skipped. PR-SPRINT feasibility gate skipped (Solo mode).

## Notes
- Story files for these tasks are not yet authored. Generate them (`/create-stories` or drafted directly) before running `/dev-story`, or implement directly against the acceptance criteria above in solo mode.
