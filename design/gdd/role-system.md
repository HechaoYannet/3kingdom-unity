---
status: reverse-documented
source: Assets/Scripts/Role/
date: 2026-04-30
verified-by: User
---

# Role System Design

> **Note**: This document was reverse-engineered from the existing implementation.
> It captures current behavior and clarified design intent. Some sections may be
> incomplete where implementation is partial or intent was unclear.

**Status**: In Review

## Overview

The Role system defines playable characters in the Three Kingdoms card game. Each role has unique abilities, faction alignment, and health-based card limits.

For the current vertical-slice direction, roles are also expected to become a major source of signature battle-presentation moments, but their skill legality and result ownership must remain deterministic.

## Player Fantasy

Players embody legendary Three Kingdoms figures with distinctive abilities that match their historical personas. Roles should eventually provide both gameplay differentiation and memorable battle moments.

## Detailed Rules

### Role Attributes
- **Name**: Role identifier
- **Faction**: Shu, Wu, Wei, or Qun
- **HP**: Health points
- **Max Cards**: Equal to current HP
- **Skills**: List of unique abilities
- **Tags**: Status condition tracking system

### Role Selection
- **Planned**: Choose from a role pool at game start
- **Current**: Single role assignment with `HuangGai` as the implemented example
- **Future**: Possible role expansion and switching

### Skill System
1. **Active Skills**
   - Manual activation via UI during play phase
   - Once-per-phase limit in the current `HuangGai` pattern
2. **Passive Skills**
   - Automatic activation when conditions are met
   - Not fully implemented yet

### Tags System
- Tracks temporary effects via key-value pairs
- Supports add, remove, and refresh operations
- Some tags are cleared during refresh

## Formulas

### Card Limit
```text
Max Cards = Current HP
```

## Edge Cases

### Implemented
1. Roles can hold HP and tag state
2. Roles can be enabled or disabled
3. `HuangGai` demonstrates one active-skill pattern

### Not Implemented / Unclear
1. Faction abilities are placeholder-only
2. Passive triggers are incomplete
3. Signature-skill presentation hooks are still planning targets

## Dependencies

- **Card System**
- **Character System**
- **UI System**
- **Round Manager**
- **Battle Presentation Layer** for future hero-moment routing

## Acceptance Criteria

- [x] Role base class with attributes
- [x] Skill system structure
- [x] Tags system
- [x] `RoleManager`
- [x] `HuangGai` role example
- [ ] Role selection from a pool
- [ ] More role implementations
- [ ] Passive skill triggers
- [ ] Signature skill presentation path

## Implementation Notes

### Current State
- Basic framework exists
- `HuangGai` demonstrates the active-skill pattern
- Tags system works at a basic level
- Faction system is still placeholder-only
- Signature-skill presentation hooks are still a planning target, not an implemented runtime path
