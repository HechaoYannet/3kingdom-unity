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
| **Engine** | Unity 2022.3.62t7 + Tuanjie Engine 1.8.5 |
| **Domain** | Rendering / Camera / Presentation / Content Pipeline |
| **Knowledge Risk** | Medium |
| **References Consulted** | `Docs/engine-reference/unity/VERSION.md`, Tuanjie URP package docs, Tuanjie Cinemachine docs, Tuanjie Timeline docs, Tuanjie Shader Graph docs |
| **Post-Cutoff APIs Used** | None required for this decision |
| **Verification Required** | Confirm render-pipeline migration feasibility, current material compatibility, Cinemachine/Timeline package state, and effect fallbacks inside Unity 2022.3.62t7 + Tuanjie 1.8.5 |

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
2. **Battle Presentation**
3. **Content Data**
4. **AI-First Production Pipeline**

### Rendering Direction
- Preferred production visual route: **URP**
- Timeline + Cinemachine + Shader Graph form the preferred authored presentation stack.
- VFX Graph is optional and should not be a first-slice gameplay-critical dependency.

### Runtime Authority Rule
All combat outcomes are owned by Rule Core.

### AI Authority Rule
AI is authorized for production acceleration, not authoritative combat resolution.

## Consequences

### Positive
- Aligns technical effort with the product differentiator
- Preserves deterministic gameplay while modernizing presentation
- Creates reusable action-presentation templates

### Negative
- Introduces temporary coexistence problems during transition
- Requires disciplined boundaries between rule and presentation layers
- Adds authoring overhead for templates and presets

## Validation Criteria
- A baseline battle view exists and remains readable
- One normal card action and one signature skill can complete a full presentation chain
- Rule outcomes remain testable without requiring presentation playback
- The first slice can degrade gracefully on lower-end settings
