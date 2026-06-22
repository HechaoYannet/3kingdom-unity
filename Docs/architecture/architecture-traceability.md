# Architecture Traceability Index

## Document Status

- **Last Updated**: 2026-04-30
- **Engine**: Unity 2022.3 LTS (Tuanjie Engine 1.8.5)
- **GDDs Indexed**: 4
- **ADRs Indexed**: 4
- **Last Review**: Updated during first battle UI/runtime architecture pass

## Coverage Summary

| Status | Count | Percentage |
|--------|-------|-----------|
| Covered | 5 | 62.5% |
| Partial | 2 | 25% |
| Gap | 1 | 12.5% |
| **Total** | **8** | |

---

## Traceability Matrix

| Req ID | GDD | System | Requirement Summary | ADR(s) | Status | Notes |
|--------|-----|--------|---------------------|--------|--------|-------|
| TR-card-001 | `design/gdd/card-system.md` | Card System | Six-state round flow controls card play sequencing | `Docs/architecture/card-system-architecture.md` | Covered | State-machine decision is documented |
| TR-card-002 | `design/gdd/card-system.md` | Card System | 3D card drag, hand layout, and play-zone interaction | `Docs/architecture/card-system-architecture.md` | Partial | Interaction architecture is documented, but input/platform verification is still missing |
| TR-character-001 | `design/gdd/character-system.md` | Character System | Human and AI players share one turn/response model | `Docs/architecture/character-system-architecture.md` | Covered | Inheritance-based entity model documented |
| TR-character-002 | `design/gdd/character-system.md` | Character System | Turn timing and AI delays rely on coroutine sequencing | `Docs/architecture/character-system-architecture.md` | Covered | Coroutine timing trade-offs documented |
| TR-role-001 | `design/gdd/role-system.md` | Role System | Roles own HP-based hand limits, skills, and tags | - | Gap | Needs a dedicated role-system ADR |
| TR-role-002 | `design/gdd/role-system.md` | Role System | Active/passive skill execution must integrate with rounds and UI | - | Gap | Needs a role ability execution ADR or a broader gameplay authority ADR |
| TR-ui-001 | `design/gdd/ui-system.md` | UI System | UI uses both screen-space HUD and world-space feedback layers | `Docs/architecture/ui-presentation-architecture.md` | Covered | Ownership is now explicit between screen HUD, world feedback, and overlay FX |
| TR-ui-002 | `design/gdd/ui-system.md` | UI System | Camera, card effects, and presentation flow coordinate with gameplay events | `Docs/architecture/ui-presentation-architecture.md`, `Docs/architecture/battle-presentation-first-architecture.md` | Partial | Screen-space target selection baseline now exists, but world-space target feedback and in-editor camera validation still remain |

---

## Known Gaps

### Foundation Layer Gaps (BLOCKING - must resolve before coding)
- [ ] None currently documented as blocking, but Event System and Data Management still need first-pass GDDs and ADRs before new feature work expands them.

### Core Layer Gaps (must resolve before relevant system is built)
- [ ] TR-role-001: Role ownership and lifecycle - GDD: `design/gdd/role-system.md` - Suggested ADR: "role-system-architecture"
- [ ] TR-role-002: Skill execution and phase integration - GDD: `design/gdd/role-system.md` - Suggested ADR: "role-skill-execution"

### Feature Layer Gaps (should resolve before feature sprint)
- [ ] TR-ui-002: Presentation/event flow for card effects and camera transitions - GDD: `design/gdd/ui-system.md` - Follow-up: validate target selection UX and accepted camera baseline in-editor

### Presentation Layer Gaps (can defer to implementation)
- [ ] TR-card-002: Cross-platform validation for input, drag, and camera interactions - Suggested follow-up: PlayMode tests plus manual device checks

---

## Cross-ADR Conflicts

| Conflict ID | ADR A | ADR B | Type | Status |
|-------------|-------|-------|------|--------|
| None | - | - | - | No conflicts documented yet |

---

## ADR -> GDD Coverage (Reverse Index)

| ADR | Title | GDD Requirements Addressed | Engine Risk |
|-----|-------|---------------------------|-------------|
| `Docs/architecture/card-system-architecture.md` | Card System Architecture Decisions | TR-card-001, TR-card-002 | Medium |
| `Docs/architecture/character-system-architecture.md` | Character System Architecture Decisions | TR-character-001, TR-character-002 | Medium |
| `Docs/architecture/battle-runtime-architecture.md` | Battle Runtime Architecture | TR-card-001, TR-character-001 | Medium |
| `Docs/architecture/ui-presentation-architecture.md` | UI Presentation Architecture | TR-ui-001, TR-ui-002 | Medium |

---

## Superseded Requirements

| Req ID | GDD | Change | Affected ADR | Status |
|--------|-----|--------|-------------|--------|
| None | - | - | - | No superseded requirements recorded yet |

---

## How to Use This Document

When new ADRs are added, update the reverse index and mark the corresponding requirement rows as `Covered`. When a GDD changes materially, check whether its related ADR rows now need a `Partial` or `Gap` status. This file is intentionally conservative: it records only coverage that can be traced to a document already present in the repository.
