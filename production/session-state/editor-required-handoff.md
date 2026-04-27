# Unity Editor Required Handoff

**Created**: 2026-04-27
**Purpose**: Track actions that cannot be safely completed from the CLI and must be executed in Unity/Tuanjie Editor by the user.

## Pending

- [ ] EDITOR-001 Confirm current render pipeline state in Project Settings and document whether the vertical slice uses Built-in as a transition path or migrates to URP immediately.
- [ ] EDITOR-002 Audit current materials/shaders for Built-in-only blockers:
  - Standard shader usage
  - surface shaders
  - `GrabPass`-style effects
  - custom post-process dependencies
- [ ] EDITOR-003 If URP is chosen, install/configure URP assets and prepare separate quality assets for lower/mobile and higher/PC targets.
- [ ] EDITOR-004 Validate current package state for Timeline, Cinemachine, and Shader Graph in the editor and record any compatibility issues.
- [ ] EDITOR-005 Open the current battle scene and capture baseline screenshots/video for:
  - default battlefield readability
  - 3D card drag flow
  - player/target framing
  - current UI feedback
- [ ] EDITOR-006 Prototype one baseline battle camera and one action camera using Cinemachine.
- [ ] EDITOR-007 Verify whether critical first-slice battle effects can be done with Particle System + shader effects before depending on VFX Graph.
- [ ] EDITOR-008 If VFX Graph is tested, validate target device compatibility and note fallback requirements for non-compute or lower-end targets.

## Completed

- [ ] None yet
