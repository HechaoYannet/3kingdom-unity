---
status: reverse-documented
date: 2026-04-09
context: Card system architecture decisions discovered from existing implementation
decision: Record architectural patterns used in Card system
consequences: Provides documentation for maintenance and future development
---

# Card System Architecture Decisions

## Status
Reverse-documented from existing implementation

## Date
2026-04-09

## Engine Compatibility

| Field | Value |
|-------|-------|
| **Engine** | Unity 2022.3.62t7 + Tuanjie Engine 1.8.5 |
| **Domain** | Core / UI / Asset Loading |
| **Knowledge Risk** | Medium |
| **References Consulted** | Tuanjie UI docs, Addressables package docs |
| **Post-Cutoff APIs Used** | None explicitly documented |
| **Verification Required** | Validate `Resources.Load()` usage, 3D card drag interactions, and scene/UI camera behavior inside Unity 2022.3.62t7 + Tuanjie 1.8.5 with the current URP route |

## ADR Dependencies

| Field | Value |
|-------|-------|
| **Depends On** | None |
| **Enables** | Future ADRs for UI presentation flow, save/load, and card effect resolution |
| **Blocks** | None |
| **Ordering Note** | A dedicated asset loading ADR should be accepted before replacing `Resources.Load()` or the current AssetBundle framework usage |

## Context

The Card System is a core component of a turn-based card game built in Unity 2022.3.62t7 + Tuanjie Engine 1.8.5. The system was implemented without formal architecture documentation. This ADR captures the architectural decisions discovered through code analysis.

The current product direction also expects card actions to feed a separate battle-presentation layer without surrendering deterministic rule ownership.

## Decision

### 1. Singleton Pattern for Global Managers
- `CardManager` exposes global access
- This simplifies access but creates global state and tighter coupling

### 2. Abstract Base Class for Card Types
- `Card` defines the shared contract for card types
- This enables polymorphic handling but requires concrete implementations everywhere

### 3. Component-Based 3D Visualization
- `Card3D`, `CardView3D`, `HandManager3D`, and play-zone components separate view, layout, and interaction concerns

### 4. State Machine for Round Management
- `RoundManager` owns a fixed 6-state round flow
- This is simple and debuggable but still rigid

### 5. Generic Resource Loading Utility
- `ResourcesLoad<T>` provides a simple static wrapper over `Resources.Load`
- This is easy to use but not a scalable long-term asset strategy

### 6. Event-Based Card Updates
- Card view update paths use event-style callbacks for data refresh

## Alternatives Considered

- Dependency injection instead of singleton access
- Addressables or a formal asset-loading ADR instead of `Resources.Load`
- A more explicit action-result payload boundary instead of direct runtime calls

## Compliance with Project Standards

### Adheres To
- Unity component-based architecture
- C# naming conventions
- MonoBehaviour lifecycle usage

### Deviations
1. Singleton usage reduces testability
2. `Resources.Load()` remains in the flow
3. Magic numbers exist in card distribution
4. Presentation-facing payload boundaries are still missing in runtime

## Related Decisions

### Connected Systems
1. **Player System**
2. **Role System**
3. **UI System**
4. **Audio System**

### Dependencies
- Unity 2022.3.62t7 + Tuanjie Engine 1.8.5
- DG.Tweening (referenced)
- TMPro
- `Docs/architecture/battle-presentation-first-architecture.md`
- `Docs/architecture/battle-action-schema.md`

## GDD Requirements Addressed

| GDD System | Requirement | How This ADR Addresses It |
|------------|-------------|---------------------------|
| `Docs/gdd/card-system.md` | Support a six-state round flow for card play sequencing | Documents the round-manager state machine |
| `Docs/gdd/card-system.md` | Support polymorphic card behavior across multiple card types | Documents the abstract base class contract |
| `Docs/gdd/card-system.md` | Support 3D hand layout, dragging, and play-zone interactions | Documents the component split |

## Performance Implications
- **CPU**: Lightweight now, but large hands and repeated layout updates may become hotspots
- **Memory**: Scene-object coupling and resource loading strategy still need cleanup
- **Load Time**: `Resources.Load()` keeps things simple but risks hitches and weak memory control
- **Network**: No network authority model is integrated yet

## Verification

- [x] Singleton usage identified
- [x] Component separation documented
- [x] State-machine flow documented
- [x] Event-style update path documented
- [ ] Runtime payload boundary still needs implementation

## Revision History
- 2026-04-09: Initial reverse-documentation from existing code
- 2026-04-30: Refreshed engine and battle-presentation context facts after URP/editor validation
