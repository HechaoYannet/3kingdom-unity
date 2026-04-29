---
status: proposed
date: 2026-04-27
context: Shift project direction toward 3D battle presentation while preserving deterministic card and turn systems
decision: Separate deterministic rule core from presentation orchestration and adopt an AI-first production pipeline for content creation
consequences: Prioritizes battle readability and spectacle without delegating authoritative combat outcomes to non-deterministic systems
---

# Battle Presentation First Architecture

## Status
Proposed

## Date
2026-04-27

## Engine Compatibility

| Field | Value |
|-------|-------|
| **Engine** | Unity 2022 LTS (Tuanjie Engine 1.5.3 compatible) |
| **Domain** | Rendering / Camera / Presentation / Content Pipeline |
| **Knowledge Risk** | Medium |
| **References Consulted** | `Docs/engine-reference/unity/VERSION.md`, Tuanjie URP package docs, Tuanjie Cinemachine docs, Tuanjie Timeline docs, Tuanjie Shader Graph docs |
| **Post-Cutoff APIs Used** | None required for this decision |
| **Verification Required** | Confirm render-pipeline migration feasibility, current material compatibility, Cinemachine/Timeline package state, and effect fallbacks inside Unity editor |

## ADR Dependencies

| Field | Value |
|-------|-------|
| **Depends On** | None |
| **Enables** | Role-system ADR, UI/presentation ADR, asset-loading ADR, action-definition data ADR |
| **Blocks** | Any broad visual migration that assumes URP without editor validation |
| **Ordering Note** | This ADR should be accepted before formalizing the first 3D battle vertical slice implementation stories |

## Context

### Problem Statement
The current project has a functioning framework but weak battlefield payoff. The original architecture over-indexed on traditional system layering and under-delivered on the intended player-facing spectacle. The new project direction requires the repository to support 3D battle staging, stronger camera language, stylized rendering, reusable action presentation templates, and faster content production through AI-assisted tooling.

### Constraints
- Existing gameplay code already exists and should not be thrown away wholesale.
- Core combat results must remain deterministic, testable, and eventually network-safe.
- The project targets both PC and mobile-friendly presentation tiers.
- Unity-editor-only migration work cannot be completed from the CLI and must be handed off cleanly.

### Requirements
- Must preserve authoritative rule resolution outside the presentation layer.
- Must let a card or skill trigger reusable presentation templates.
- Must support graceful degradation when high-end effects are unavailable.
- Must accelerate content generation and documentation with AI without handing off authoritative runtime combat logic.

## Decision

Adopt a **battle-presentation-first architecture** built on three runtime layers and one production layer:

1. **Rule Core**
   - deterministic turn order
   - card legality
   - target validation
   - result computation
   - battle log and test seams
2. **Battle Presentation**
   - camera orchestration
   - action staging
   - Timeline-authored hero moments
   - VFX and hit timing
   - world-space and HUD result feedback
3. **Content Data**
   - card definitions
   - role/skill definitions
   - action presentation templates
   - camera and VFX presets
4. **AI-First Production Pipeline**
   - concept generation
   - style exploration
   - shot ideation
   - documentation retrieval
   - QA/report helpers

### Rendering Direction
- Preferred production visual route: **URP**
- Short-term coexistence with Built-in is acceptable only as a transition constraint.
- Timeline + Cinemachine + Shader Graph form the preferred authored presentation stack.
- VFX Graph is optional and should not be a first-slice dependency for gameplay-critical effects.

### Runtime Authority Rule
All combat outcomes are owned by Rule Core.
Presentation may delay, emphasize, or visualize outcomes, but it may not decide:
- whether an action is legal
- whether an attack hits
- how much damage/heal applies
- who owns the next turn

### AI Authority Rule
AI is authorized for production acceleration, not authoritative combat resolution.
It may generate:
- look-dev references
- draft content
- retrieval-based design assistance
- automated summaries and QA helpers

It may not authoritatively decide:
- live deterministic combat outcomes
- final balance values without review
- networked match state

### Architecture Diagram

```text
Player Input / AI Intent
        |
        v
    Rule Core --------------------> Battle Log / Tests
        |
        v
Action Result Payload
        |
        v
Battle Presentation
  |       |        |
  |       |        +--> HUD / World Feedback
  |       +----------> VFX / Audio / Hit Timing
  +------------------> Camera / Timeline / Animation

Content Data ---------------------> Rule Core + Battle Presentation
AI Production Pipeline -----------> Docs + Content Drafts + QA Helpers
```

### Key Interfaces
- `RuleActionRequest`
  - actor id
  - target ids
  - card/skill id
  - declared action type
- `RuleActionResult`
  - validated action type
  - resolved targets
  - numeric results
  - status/tag changes
  - presentation hint id
- `PresentationDefinition`
  - action family
  - camera template id
  - animation cue id
  - VFX cue list
  - hit timing data
  - UI feedback data

## Alternatives Considered

### Alternative 1: Traditional system-first continuation
- **Description**: Keep prioritizing generic framework completion before battlefield spectacle.
- **Pros**: Low short-term architectural churn; easier to keep using current code patterns.
- **Cons**: Continues the exact project weakness identified by product direction; delayed player-facing payoff.
- **Rejection Reason**: It optimizes the wrong thing for the stated product goal.

### Alternative 2: Fully AI-directed runtime combat
- **Description**: Let generative AI interpret combat intent and drive runtime outcomes dynamically.
- **Pros**: High novelty, potentially fast iteration on expressive content.
- **Cons**: Non-deterministic, hard to test, hard to balance, unsuitable for authoritative combat logic, risky for future networking.
- **Rejection Reason**: Violates deterministic gameplay and production safety requirements.

### Alternative 3: Full rendering-stack migration before gameplay slicing
- **Description**: Pause gameplay-facing work and complete a broad URP/visual migration first.
- **Pros**: Cleaner long-term rendering foundation.
- **Cons**: High schedule risk; delays proof that the battle-presentation direction actually lands.
- **Rejection Reason**: A focused vertical slice should validate the direction before broad migration cost is paid.

## Consequences

### Positive
- Aligns technical effort with the product's real differentiator: battle spectacle.
- Preserves deterministic gameplay while modernizing presentation.
- Creates reusable action-presentation templates instead of one-off cinematic hacks.
- Gives AI a concrete, high-leverage role in production without surrendering runtime authority.

### Negative
- Introduces a temporary coexistence problem if Built-in and URP overlap during transition.
- Requires disciplined boundaries between rule and presentation layers.
- Adds authoring overhead for action templates, camera presets, and presentation tags.

### Risks
- **URP migration cost**: existing materials/shaders may not migrate cleanly.
  - **Mitigation**: validate on one battle slice before committing project-wide.
- **Presentation overreach**: camera/VFX may reduce readability.
  - **Mitigation**: enforce fallback and readability constraints in the GDD.
- **AI misuse**: team may attempt to push non-deterministic tooling into authoritative combat runtime.
  - **Mitigation**: keep AI-authority boundaries explicit in docs and backlog.

## GDD Requirements Addressed

| GDD System | Requirement | How This ADR Addresses It |
|------------|-------------|---------------------------|
| `design/gdd/3d-battle-presentation-system.md` | Cards and skills must map to reusable 3D battle presentation templates | Establishes Content Data and Battle Presentation separation |
| `design/gdd/3d-battle-presentation-system.md` | Rule resolution must remain authoritative outside presentation | Establishes Rule Core ownership and explicit authority boundary |
| `design/gdd/ui-system.md` | UI must support readable world-space and screen-space feedback | Routes HUD/world feedback through the presentation layer instead of core rules |
| `design/gdd/card-system.md` | Card play remains part of the turn-based core loop | Keeps card legality and result computation in Rule Core |

## Performance Implications
- **CPU**: Additional orchestration overhead for camera, event dispatch, and feedback timing; manageable if action templates remain lightweight.
- **Memory**: Increased content footprint from camera presets, effects, and animation references.
- **Load Time**: Can increase if battle scenes preload richer assets; asset strategy ADR should address this.
- **Network**: Safer than AI-runtime alternatives because the authoritative state remains deterministic and serializable.

## Migration Plan
1. Freeze the first battle vertical slice scope.
2. Validate current render path and URP migration feasibility inside Unity editor.
3. Introduce presentation definitions without replacing existing rule authority.
4. Hook one standard card and one signature skill into the new presentation chain.
5. Add fallback paths for missing premium effects.
6. Expand to broader content only after the slice proves readable and stable.

## Validation Criteria
- A baseline battle view exists and remains readable.
- One normal card action and one signature skill can complete a full presentation chain.
- Rule outcomes remain testable without requiring presentation playback.
- The first slice can degrade gracefully on lower-end settings.

## Related Decisions
- `design/gdd/card-system.md`
- `design/gdd/ui-system.md`
- `design/gdd/3d-battle-presentation-system.md`
- Future ADRs for role-system architecture, UI/presentation architecture, and asset-loading strategy
