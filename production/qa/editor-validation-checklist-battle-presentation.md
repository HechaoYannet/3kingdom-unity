# Editor Validation Checklist: Battle Presentation Vertical Slice

**Status**: Ready
**Created**: 2026-04-27
**Use When**: Opening Unity/Tuanjie Editor to validate the battle-presentation-first path on this branch.

## Output Required

When you complete this checklist, record:
- final render-path decision
- package validation notes
- material/shader blockers
- baseline battle scene screenshots
- one short video clip if a camera prototype is built

Store evidence under:
- `production/qa/evidence/` if you export screenshots/video outside the repo, or
- a short markdown note beside this checklist if you are only reporting findings

## Step 1: Confirm Branch and Open Project

- [ ] Confirm the branch is `feature-battle-presentation-planning`
- [ ] Open the project in the intended Unity/Tuanjie editor version
- [ ] Note any package import or compilation errors before changing anything

## Step 2: Confirm Render Path Baseline

- [ ] Open Project Settings and check whether a Render Pipeline Asset is currently assigned
- [ ] Record whether the current battle slice starts from Built-in or URP
- [ ] If Built-in, note any obvious dependency on legacy shaders/materials
- [ ] If URP is already active, record which URP asset(s) are bound to quality tiers

## Step 3: Audit Material and Shader Blockers

Check representative battle-scene materials and note blockers:
- [ ] Standard shader usage
- [ ] Surface shader usage
- [ ] `GrabPass` or image-effect style dependencies
- [ ] Custom materials that break visually under URP test conversion

Record:
- asset path
- blocker type
- severity: low / medium / high
- workaround guess if obvious

## Step 4: Validate Package State

- [ ] Confirm whether Cinemachine is installed and import-clean
- [ ] Confirm whether Timeline is installed and import-clean
- [ ] Confirm whether Shader Graph is installed or absent
- [ ] Confirm whether VFX Graph is installed or absent

Record any version or compatibility issue that blocks:
- camera rigs
- Timeline authorement
- shader authoring
- effect authoring

## Step 5: Baseline Battle Scene Capture

- [ ] Open the current battle scene
- [ ] Capture one default full-battle framing screenshot
- [ ] Capture one screenshot showing current 3D card interaction state
- [ ] Capture one screenshot showing current HUD/result readability

Questions to answer:
- Can the player clearly read actor positions?
- Is the current camera angle usable as a baseline battle view?
- Does current UI clash with a future cinematic camera approach?

## Step 6: First Camera Prototype

- [ ] Build or validate one baseline battle camera
- [ ] Build or validate one action-emphasis camera
- [ ] Test a simple blend between them

Record:
- whether the blend feels readable
- whether current scene layout supports stronger camera moves
- any occlusion or framing problems

## Step 7: Effect Path Decision

- [ ] Decide whether the first slice can rely on Particle System + shaders for critical effects
- [ ] If testing VFX Graph, note whether target hardware requirements are acceptable
- [ ] Confirm fallback expectation for lower-end/mobile-compatible targets

## Step 8: Report Back Into Repo

Update these files after the editor pass:
- `production/session-state/editor-required-handoff.md`
- `production/session-state/development-checklist.md`
- `production/backlog/battle-presentation-vertical-slice-backlog.md`

If major findings change the route, also update:
- `Docs/architecture/battle-presentation-first-architecture.md`
- `Docs/planning/6-week-battle-presentation-roadmap.md`

## Exit Criteria

- [ ] Render route decision is explicit
- [ ] Package/toolchain state is explicit
- [ ] Scene readability baseline is documented
- [ ] Camera prototype feasibility is documented
- [ ] Effect fallback expectations are documented
