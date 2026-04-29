# 6-Week Battle Presentation Roadmap

**Status**: Draft
**Start Date**: 2026-04-27
**End Date**: 2026-06-07
**Branch Baseline**: `feature-battle-presentation-planning`

## Outcome Target

By the end of this roadmap, the project should have a playable vertical slice that proves the game is no longer just a digital card framework. The slice must show a deterministic card-and-turn battle core expressed through a 3D battlefield, character action staging, camera language, and strong hit/result feedback.

## Vertical Slice Scope

- 1 formal battle scene
- 2 battle-ready roles/characters
- 3 baseline cards
- 1 signature skill
- 1 reusable standard-action presentation template
- 1 reusable hero-moment presentation template
- baseline HUD plus world-space result feedback
- first executable gameplay tests

## Workstreams

### Workstream A: Rule Core Stability
- preserve deterministic turn and card resolution
- add test seams and battle logs
- keep presentation downstream from rules

### Workstream B: Battle Presentation
- baseline battle view
- action camera templates
- Timeline-authored hero moments
- hit timing and result synchronization
- VFX/audio/UI feedback

### Workstream C: Content Data
- card definitions
- role/skill definitions
- presentation definitions
- camera and VFX preset IDs

### Workstream D: AI-First Production
- concept and look-dev generation
- doc retrieval and summarization
- QA/report helpers
- asset naming and task helper flows

## Week-by-Week Plan

### Week 1: 2026-04-27 to 2026-05-03
**Goal**: Freeze direction and vertical slice scope.

Repo-local tasks:
- [x] Establish branch and resume-safe tracking files
- [x] Add battle-presentation system GDD
- [x] Add battle-presentation-first ADR
- [ ] Expand roadmap and AI production docs

Unity-editor-required tasks:
- [ ] Confirm current render pipeline state and migration feasibility
- [ ] Capture baseline battle scene screenshots/video

Exit criteria:
- product direction frozen
- vertical slice scope frozen
- editor validation tasks assigned

### Week 2: 2026-05-04 to 2026-05-10
**Goal**: Connect one battle action from deterministic rule output to visible 3D presentation.

Repo-local tasks:
- [ ] Define action result payload schema
- [ ] Define presentation template schema
- [ ] Add first gameplay tests for round and role rules

Unity-editor-required tasks:
- [ ] Validate Cinemachine and Timeline package state
- [ ] Prototype baseline camera rig and one action camera

Exit criteria:
- one card action can traverse rule output -> presentation template -> visible staging

### Week 3: 2026-05-11 to 2026-05-17
**Goal**: Build a battle scene that can be shown, not just debugged.

Repo-local tasks:
- [ ] Document shot taxonomy and presentation tags
- [ ] Finalize first pass of card/role/presentation data mapping

Unity-editor-required tasks:
- [ ] Stage 2 roles/characters in battle scene
- [ ] Build one signature-skill hero-moment sequence
- [ ] Add world-space damage/heal feedback

Exit criteria:
- 15-30 second battle clip is recordable and readable

### Week 4: 2026-05-18 to 2026-05-24
**Goal**: Productize the first presentation path into reusable templates.

Repo-local tasks:
- [ ] Add action-definition and camera/VFX preset docs
- [ ] Expand automated tests around role/tag/round logic
- [ ] Document effect fallback behavior

Unity-editor-required tasks:
- [ ] Convert one-off action setup into reusable scene/prefab/template assets
- [ ] Validate downgraded quality path for lower-end targets

Exit criteria:
- new actions can inherit a template rather than bespoke hardcoded setup

### Week 5: 2026-05-25 to 2026-05-31
**Goal**: Reach a minimum playable combat loop.

Repo-local tasks:
- [ ] Backlog next role/card additions from validated templates
- [ ] Update QA checklist and smoke paths based on real slice behavior

Unity-editor-required tasks:
- [ ] Bring 3 baseline cards into the vertical slice
- [ ] Bring 1 signature skill into the vertical slice
- [ ] Validate turn flow, hit timing, and result readability in-game

Exit criteria:
- one minimal battle can start, progress, and end without presentation chain collapse

### Week 6: 2026-06-01 to 2026-06-07
**Goal**: Evaluate whether the new direction is strong enough to scale.

Repo-local tasks:
- [ ] Record technical debt and migration blockers found during the slice
- [ ] Update milestone and sprint docs for the next phase

Unity-editor-required tasks:
- [ ] Capture review footage and screenshots
- [ ] Run final vertical-slice evaluation in-editor

Exit criteria:
- clear verdict on whether to expand content, deepen presentation, or revise rendering strategy

## Dependency Order

1. Freeze direction
2. Validate render/camera toolchain
3. Hook one full action chain
4. Productize templates
5. Expand to minimum playable loop
6. Evaluate and branch next phase

## Success Metrics

- A player can identify this as a 3D battle presentation game, not a flat card UI prototype.
- Core rule results remain deterministic and testable outside presentation playback.
- At least one standard card action and one signature skill have complete presentation chains.
- Lower-end fallback behavior exists for readability-critical feedback.

## Failure Signals

- The project continues adding systems without a showable battle clip.
- Presentation requires bespoke per-action code with no reusable template path.
- Visual quality depends on high-end-only effects with no fallback.
- AI tooling creates artifacts faster, but without a review or integration path.
