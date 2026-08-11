---
status: draft
source: Product direction update on 2026-04-27
date: 2026-04-27
verified-by: Pending
---

# 3D Battle Presentation System

**Status**: In Design

## Overview

The 3D Battle Presentation System transforms the project from a framework-led digital card prototype into a battle-presentation-first game. It keeps the card and turn-based rule skeleton inspired by Three Kingdoms Kill, but expresses card play through spatial 3D combat staging, stylized character actions, dynamic camera language, VFX timing, and modern audiovisual feedback inspired by contemporary anime-style turn-based games. This system is responsible for how actions look, feel, and read to the player, while remaining downstream from deterministic battle rules.

## Player Fantasy

Players should feel that every card is not just a UI command but a dramatic battlefield instruction. Playing an attack card should read like ordering a general to strike in a living 3D arena. Skills should feel like signature hero moments with camera focus, impact timing, and spectacle. The player fantasy is not "I moved a card on a table," but "I commanded a stylized duel where tactics become cinematic action."

## Detailed Rules

### System Role
- The system owns battle presentation, not battle legality.
- It consumes battle events from rule systems and translates them into presentation actions.
- It must never be the authoritative source of hit, heal, card legality, target validity, or round order.

### Presentation Layers
1. **Baseline Battle View**
   - Maintains the readable default battlefield framing.
   - Shows character placement, turn ownership, current target emphasis, and persistent HUD feedback.
2. **Action Presentation**
   - Triggered when a card or skill is executed.
   - Plays character motion, camera transitions, VFX, time emphasis, hit timing, and UI result feedback.
3. **Hero Moment Presentation**
   - Used for ult-like or signature skills, decisive responses, and rare states.
   - May use Timeline-authored shots, custom camera tracks, and enhanced VFX/audio emphasis.

### Runtime Flow
1. Rule core confirms an action is legal.
2. Rule core emits an action payload:
   - actor
   - targets
   - action type
   - card/skill identifier
   - result payload
3. Presentation system resolves a matching presentation definition.
4. Presentation system stages:
   - actor pose or animation
   - camera shot selection
   - VFX and hit timing
   - HUD/world-space feedback
5. Rule result is displayed at the authored hit frame or result frame.
6. Camera and HUD return to baseline battle view.

### Presentation Definitions
Every playable battle action should map to a reusable presentation definition made from:
- action category
- animation cue
- camera template
- hit timing
- VFX cues
- UI feedback cues
- optional Timeline sequence

### Visual Direction Rules
- The battlefield must retain clear spatial readability even during dramatic shots.
- Camera motion should emphasize action, not disorient the player.
- Core actions must be readable without relying on top-end hardware-only effects.
- Critical feedback must remain visible even when VFX are disabled or downgraded.

### Rendering Strategy
- Preferred target: URP for the production visual route.
- Built-in render path may remain temporarily only to keep the existing project runnable during transition.
- Shader Graph is the preferred route for stylized materials, card glows, outlines, hit flashes, and dissolves once URP is active.
- VFX Graph is optional and should not be required for gameplay-critical effects in the first playable slice.

### AI Production Role
AI may be used to accelerate:
- concept art and moodboards
- action and VFX ideation
- shot list generation
- animation reference generation
- documentation retrieval and summarization
- QA reporting and log summarization

AI must not be the authoritative runtime source of:
- battle legality
- card resolution
- damage/heal values
- deterministic turn decisions

## Formulas

### Presentation Duration Envelope
For a standard action:

`TotalActionPresentation = Windup + Travel + HitPause + Recovery`

Where:
- `Windup` is the time from action trigger to visible commitment
- `Travel` is projectile or movement travel time, if any
- `HitPause` is the emphasized impact frame window
- `Recovery` is the return to readable baseline state

### Camera Priority Rule

`HeroMoment > ActionPresentation > BaselineBattleView`

Only one top-priority camera state may control the screen at a time. Lower-priority states must blend out or wait.

### Feedback Synchronization Rule

`DisplayedResultFrame = AuthoredHitFrame + ResultOffset`

Where:
- `AuthoredHitFrame` is the impact beat defined by the action template
- `ResultOffset` is a small per-template adjustment used to align gameplay feedback with the animation/VFX beat

## Edge Cases

### Readability and Failover
- If a premium effect pipeline is unavailable, the action must still play with fallback particles, UI result feedback, and baseline camera emphasis.
- If a Timeline-authored sequence is missing, the system must fall back to a generic action camera template.

### Interruptions
- If the target dies or leaves the state space before the final recovery frame, the presentation should still resolve its result feedback and then return to baseline.
- If multiple rapid actions queue, the system must preserve result ordering even if presentation is compressed.

### Platform Constraints
- Mobile and lower-end hardware must be able to run a downgraded presentation path with reduced camera layers, lower VFX density, and simpler materials.
- Critical gameplay information cannot depend on motion blur, bloom, or high-end particle counts.

### Authoring Gaps
- If a new card or skill has no bespoke presentation template, it should inherit a generic action family template rather than block implementation.

## Dependencies

### Required Systems
- `Docs/gdd/card-system.md`
- `Docs/gdd/character-system.md`
- `Docs/gdd/role-system.md`
- `Docs/gdd/ui-system.md`

### Supporting Runtime Domains
- camera management
- animation playback
- VFX playback
- world-space UI feedback
- battle event dispatch
- action definition data

### Tooling and Packages
- Unity Timeline
- Cinemachine
- Shader Graph after URP adoption
- optional VFX Graph for high-end visual layers

## Tuning Knobs

- default battle camera distance
- camera blend duration per action family
- hit pause duration
- world-space damage text duration
- action presentation duration cap for normal cards
- hero moment presentation duration cap for signature skills
- VFX quality tier
- post-processing intensity per platform tier
- fallback template mapping for undocumented actions

## Acceptance Criteria

- [ ] A baseline 3D battle view exists and preserves readability of actors, targets, and active turn state.
- [ ] At least one standard card action can trigger a full presentation chain: animation, camera, VFX, and result feedback.
- [ ] At least one signature skill can trigger an enhanced hero-moment presentation path.
- [ ] Rule resolution remains authoritative outside the presentation layer.
- [ ] The system supports template fallback when bespoke presentation content is missing.
- [ ] The first playable slice can run on both a baseline PC target and a reduced-quality mobile-compatible target path.
- [ ] Critical battle information remains readable when premium VFX are disabled.
