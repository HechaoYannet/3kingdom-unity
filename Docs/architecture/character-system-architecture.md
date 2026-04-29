---
status: reverse-documented
date: 2026-04-09
context: Character system architecture decisions discovered from existing implementation
decision: Record architectural patterns used in Character system
consequences: Provides documentation for maintenance and future development
---

# Character System Architecture Decisions

## Status
Reverse-documented from existing implementation

## Date
2026-04-09

## Engine Compatibility

| Field | Value |
|-------|-------|
| **Engine** | Unity 2022.3.62t7 + Tuanjie Engine 1.8.5 |
| **Domain** | Core / Gameplay / Coroutine Timing |
| **Knowledge Risk** | Medium |
| **References Consulted** | `Docs/engine-reference/unity/VERSION.md`, `Docs/engine-reference/unity/modules/animation.md`, `Docs/engine-reference/unity/modules/ui.md` |
| **Post-Cutoff APIs Used** | None explicitly documented |
| **Verification Required** | Validate coroutine timing, turn timeout handling, and role/player coupling inside Unity 2022.3.62t7 + Tuanjie 1.8.5 with the current URP route |

## ADR Dependencies

| Field | Value |
|-------|-------|
| **Depends On** | None |
| **Enables** | Future ADRs for role abilities, combat resolution, and persistence of player state |
| **Blocks** | None |
| **Ordering Note** | A later combat or health ADR should define damage ownership before broader gameplay implementation continues |

## Context

The Character System manages player entities, AI opponents, and role integration in a turn-based Three Kingdoms card game. The system was implemented without formal architecture documentation. This ADR captures the architectural decisions discovered through code analysis.

The current production direction places this system inside the deterministic rule core that must later emit stable action/result payloads for battle presentation.

## Decision

### 1. Inheritance-Based Entity System
- `EnemyAI` inherits from `Player`
- This maximizes code reuse but creates rigid hierarchy coupling

### 2. Singleton Player Manager
- `PlayerManager` exposes global player access
- This is simple but reduces testability and increases global state

### 3. Coroutine-Based Timing System
- Turn timers and AI delays use Unity coroutines
- This fits Unity well but complicates timing verification

### 4. State Flag Pattern for Turn Management
- Turn state is tracked through boolean flags
- This is easy to read but can allow invalid state combinations

### 5. Composition with Role System
- Players hold roles rather than inheriting role behavior
- This is flexible but adds another layer of coordination

### 6. Enum-Based UI Communication
- `UIOperation` provides a lightweight bridge between UI and gameplay flow

## Alternatives Considered

- Strategy-based behavior instead of inheritance
- Dependency injection instead of singleton player lookup
- A stricter state-machine model instead of distributed booleans

## Compliance with Project Standards

### Adheres To
- Unity MonoBehaviour patterns
- C# naming conventions
- Coroutine-based async flow

### Deviations
1. Singleton usage reduces testability
2. Boolean state flags can drift into invalid combinations
3. Player IDs use magic numbers
4. Presentation-facing action/result payloads are still absent

## Related Decisions

### Connected Systems
1. **Card System**
2. **Role System**
3. **UI System**
4. **Round Manager**

### Dependencies
- Unity 2022.3.62t7 + Tuanjie Engine 1.8.5
- Card System
- Role System
- `Docs/architecture/battle-action-schema.md`

## GDD Requirements Addressed

| GDD System | Requirement | How This ADR Addresses It |
|------------|-------------|---------------------------|
| `design/gdd/character-system.md` | Represent both human players and AI opponents within one turn framework | Documents the inheritance-based entity model |
| `design/gdd/character-system.md` | Enforce HP-driven hand limits and role composition | Documents the player-role composition pattern |
| `design/gdd/character-system.md` | Support timed turns and AI response delays | Documents the coroutine-based timing model |

## Performance Implications
- **CPU**: Cheap at current scale, but polling-style logic and manager scans will degrade with more actors
- **Memory**: Lightweight, though singleton/global references increase lifetime coupling
- **Load Time**: Minimal in current form
- **Network**: Current player ID conventions are local-only and would need redesign for network authority

## Verification

- [x] Inheritance hierarchy identified
- [x] Singleton usage documented
- [x] Coroutine timing documented
- [x] Role composition documented
- [x] State flag pattern documented
- [ ] Runtime action/result payload boundary still needs implementation

## Revision History
- 2026-04-09: Initial reverse-documentation from existing code
- 2026-04-30: Refreshed engine and deterministic-rule-core context facts after URP/editor validation
