# Systems Index: Three Kingdom - Current Implementation

> **Status**: Draft
> **Created**: 2026-04-27
> **Last Updated**: 2026-04-27
> **Source Concept**: Reverse-documented from `Assets/Scripts/` and existing GDDs

---

## Overview

This project already contains an implemented turn-based Three Kingdoms-themed card battler built in Unity/Tuanjie. The current design task is not greenfield ideation but organizing the implemented systems into a stable index so downstream skills can reason about design coverage, dependency order, and missing documentation. The mechanical scope centers on card play, player and AI turns, role abilities, supporting UI, and the data/event foundations needed to make those systems maintainable.

---

## Systems Enumeration

| # | System Name | Category | Priority | Status | Design Doc | Depends On |
|---|-------------|----------|----------|--------|------------|------------|
| 1 | Asset Bundle Framework | Core | MVP | Not Started | - | None |
| 2 | Event System | Core | MVP | Not Started | - | None |
| 3 | Data Management | Persistence | MVP | Not Started | - | Event System |
| 4 | Character System | Gameplay | MVP | In Review | `design/gdd/character-system.md` | Data Management, Event System |
| 5 | Role System | Gameplay | MVP | In Review | `design/gdd/role-system.md` | Character System |
| 6 | Card System | Gameplay | MVP | In Review | `design/gdd/card-system.md` | Character System, Role System |
| 7 | UI System | UI | MVP | In Review | `design/gdd/ui-system.md` | Card System, Character System, Role System, Event System |
| 8 | 3D Battle Presentation System | UI | MVP | In Design | `design/gdd/3d-battle-presentation-system.md` | Card System, Character System, Role System, UI System |
| 9 | Audio System | Audio | Vertical Slice | Not Started | - | Event System, UI System, 3D Battle Presentation System |
| 10 | Home/Login Flow | UI | Vertical Slice | Not Started | - | UI System, Data Management |
| 11 | Utility Tools | Meta | Alpha | Not Started | - | None |
| 12 | Messaging/Networking Hooks (inferred) | Core | Alpha | Not Started | - | Event System, Data Management |

---

## Categories

| Category | Description | Typical Systems |
|----------|-------------|-----------------|
| **Core** | Foundation systems everything depends on | Asset loading, event dispatch, networking/message routing |
| **Gameplay** | The systems that make the game playable | Card resolution, players, AI, roles |
| **Persistence** | Save state and continuity | Save/load, profile data, runtime data storage |
| **UI** | Player-facing information displays | HUD, menus, hand presentation, scene flow |
| **Audio** | Sound and music systems | Music manager, SFX, card and UI feedback |
| **Meta** | Systems outside the core game loop | Shared utilities, editor helpers, support tooling |

---

## Priority Tiers

| Tier | Definition | Target Milestone | Design Urgency |
|------|------------|------------------|----------------|
| **MVP** | Required for the battle loop and reverse-documentation baseline to function | Production stabilization | Design FIRST |
| **Vertical Slice** | Required to make one end-to-end playable slice coherent for handoff/testing | First documented playable slice | Design SECOND |
| **Alpha** | Support and extension systems that matter once the core loop is stable | Broader system hardening | Design THIRD |

---

## Dependency Map

### Foundation Layer (no dependencies)

1. Asset Bundle Framework - Owns asset loading and bundle relationships.
2. Event System - Provides loose coupling point for gameplay/UI notifications.

### Core Layer (depends on foundation)

1. Data Management - depends on: Event System.
2. Character System - depends on: Data Management, Event System.
3. Messaging/Networking Hooks (inferred) - depends on: Event System, Data Management.

### Feature Layer (depends on core)

1. Role System - depends on: Character System.
2. Card System - depends on: Character System, Role System.

### Presentation Layer (depends on features)

1. UI System - depends on: Card System, Character System, Role System, Event System.
2. 3D Battle Presentation System - depends on: Card System, Character System, Role System, UI System.
3. Audio System - depends on: Event System, UI System, 3D Battle Presentation System.
4. Home/Login Flow - depends on: UI System, Data Management.

### Polish Layer (depends on everything)

1. Utility Tools - depends on: whichever runtime system they support.

---

## Recommended Design Order

| Order | System | Priority | Layer | Agent(s) | Est. Effort |
|-------|--------|----------|-------|----------|-------------|
| 1 | Event System | MVP | Foundation | game-designer, gameplay-programmer | S |
| 2 | Data Management | MVP | Core | game-designer, gameplay-programmer | S |
| 3 | Character System | MVP | Core | game-designer | M |
| 4 | Role System | MVP | Feature | game-designer | S |
| 5 | Card System | MVP | Feature | game-designer | M |
| 6 | UI System | MVP | Presentation | ux-designer, ui-programmer | M |
| 7 | 3D Battle Presentation System | MVP | Presentation | technical-director, ui-programmer, technical-artist | L |
| 8 | Asset Bundle Framework | MVP | Foundation | technical-director, gameplay-programmer | M |
| 9 | Audio System | Vertical Slice | Presentation | sound-designer | S |
| 10 | Home/Login Flow | Vertical Slice | Presentation | ux-designer | S |
| 11 | Messaging/Networking Hooks (inferred) | Alpha | Core | technical-director | M |
| 12 | Utility Tools | Alpha | Polish | gameplay-programmer | S |

---

## Circular Dependencies

- Card System <-> Character System: players own cards, while cards target and validate against players. Resolution: treat player/role state as the authoritative contract and keep card effects operating through those interfaces.
- Character System <-> Role System: players hold roles and role rules shape player limits. Resolution: role data should remain compositional and avoid reaching back into player logic except through explicit APIs.

---

## High-Risk Systems

| System | Risk Type | Risk Description | Mitigation |
|--------|-----------|-----------------|------------|
| Card System | Design | Several card effects are referenced but not fully implemented, so documented rules may drift from intent | Reverse-document current behavior first, then validate with a balance/design pass |
| UI System | Technical | Dual-space UI and 3D card interaction are sensitive to camera/input regressions across PC and mobile | Add manual walkthrough evidence and PlayMode coverage before expanding UI scope |
| 3D Battle Presentation System | Technical | The new flagship feature depends on rendering, camera, animation, and VFX coordination across PC/mobile tiers | Validate one vertical slice before broad migration and keep premium-effect fallbacks |
| Asset Bundle Framework | Technical | Project contains both custom bundle logic and direct `Resources.Load()` usage | Record the intended loading split in an ADR before refactoring |
| Messaging/Networking Hooks (inferred) | Scope | Mirror and message handlers exist, but gameplay ownership is undocumented | Keep out of current sprint scope until a network authority ADR exists |

---

## Progress Tracker

| Metric | Count |
|--------|-------|
| Total systems identified | 12 |
| Design docs started | 5 |
| Design docs reviewed | 4 |
| Design docs approved | 0 |
| MVP systems designed | 5/8 |
| Vertical Slice systems designed | 0/2 |

---

## Next Steps

- [ ] Run `/reverse-document design Assets/Scripts/AssetBundleFramework`
- [ ] Run `/reverse-document design Assets/Scripts/Data`
- [ ] Run `/reverse-document design Assets/Scripts/Event`
- [ ] Review and approve `design/gdd/3d-battle-presentation-system.md`
- [ ] Run `/design-review` on the four existing GDDs so statuses can move from `In Review` to `Approved`
- [ ] Create missing ADRs for role, UI, data, and asset-loading architecture
