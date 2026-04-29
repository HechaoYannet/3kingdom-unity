---
status: reverse-documented
source: Assets/Scripts/Role/
date: 2026-04-09
verified-by: User
---

# Role System Design

> **Note**: This document was reverse-engineered from the existing implementation.
> It captures current behavior and clarified design intent. Some sections may be
> incomplete where implementation is partial or intent was unclear.

## Overview

The Role system defines playable characters in the Three Kingdoms card game. Each role has unique abilities, faction alignment, and health-based card limits.

## Player Fantasy

Players embody legendary Three Kingdoms figures with distinctive abilities that match their historical personas. Switching between roles during gameplay allows strategic adaptation to changing game states.

## Detailed Rules

### Role Attributes
- **Name**: Role identifier (e.g., "黄盖" - HuangGai)
- **Faction**: Shu, Wu, Wei, or Qun - affects special abilities
- **HP**: Health points (4 for HuangGai)
- **Max Cards**: Equal to current HP
- **Skills**: List of unique abilities
- **Tags**: Status condition tracking system

### Role Selection
- **Planned**: Choose from role pool at game start
- **Current**: Single role assignment (HuangGai implemented)
- **Future**: Multiple roles per player with switching capability

### Skill System
**Skill Types:**
1. **Active Skills**: Manual activation via UI button during play phase
   - Example: HuangGai's "苦肉计" (Bitter Flesh Stratagem)
   - Once per phase limit
2. **Passive Skills**: Automatic activation when conditions met
   - Trigger based on game events
   - No player input required

**Skill Components:**
- Name (string)
- Description (string)
- Type (Active/Passive)

### Faction System
- **Shu, Wu, Wei, Qun**: Four Three Kingdoms factions
- **Effect**: Faction-specific special abilities
- **Implementation**: Currently faction affects role assignment only

### Tags System
- **Purpose**: Track status conditions (buffs/debuffs)
- **Format**: Key-value pairs (e.g., "Card|JIU": count)
- **Operations**: Add, remove, refresh (clear temporary tags)
- **Refresh**: "Card|JIU" cleared at refresh phase

### Role Management
- **RoleManager**: Singleton managing all active roles
- **Enable/Disable**: Roles can be activated/deactivated
- **Skill Reset**: Reset skills between rounds (placeholder)

## Formulas

### Card Limit
```
Max Cards = Current HP
```

### Tag Management
```
AddTag(tag, count = 1):
  If tag exists and count ≥ 1: increment
  Else: add new tag with count

RemoveTag(tag, count = 1, clear = false):
  If clear: remove tag entirely
  Else: decrement count, remove if count ≤ 0
```

## Edge Cases

### HP at Zero
- Role disabled when HP ≤ 0
- Card limit becomes 0
- May trigger role switching

### Tag Overflow
- No upper limit on tag counts
- Refresh clears specific temporary tags only

### Multiple Active Skills
- Only one Active skill per phase (HuangGai implementation)
- Future: May allow multiple with different cooldowns

## Dependencies

### Required Systems
- **Card System**: Card interactions with roles
- **Character System**: Player role assignment
- **UI System**: Skill activation buttons
- **Round Manager**: Phase tracking for skill limits

### Integration Points
1. Player selects role from pool (future)
2. Role HP affects card hand size
3. Skills interact with card plays
4. Tags track card/status effects

## Tuning Knobs

### Balance Values
- **Base HP**: 4 (HuangGai)
- **Skill Limits**: Once per phase (Active skills)
- **Card Ratio**: 1 card per HP point

### Configurable Values
1. Role HP ranges
2. Skill cooldowns/limits
3. Faction ability modifiers
4. Tag effect durations

## Acceptance Criteria

### Core Functionality
- [x] Role base class with attributes
- [x] Skill system (Active/Passive types)
- [x] Tags system for status tracking
- [x] RoleManager singleton
- [x] HuangGai role implementation

### Missing Features
- [ ] Role selection from pool
- [ ] Multiple role switching
- [ ] Faction ability implementation
- [ ] More role implementations
- [ ] Passive skill triggers
- [ ] Skill cooldown tracking

## Implementation Notes

### Current State
- Basic framework complete
- HuangGai demonstrates Active skill pattern
- Tags system functional but minimal usage
- Faction system placeholder only

### Code Structure
```
Role.cs              # Base class with tags system
RoleManager.cs       # Singleton manager  
Roles/HuangGai.cs    # Example role implementation
```

### Design Decisions
1. **HP-based card limit**: Encourages HP management
2. **Tag system**: Flexible status tracking
3. **Skill types**: Manual vs automatic activation
4. **Faction alignment**: Future ability differentiation

## Follow-Up Work

### High Priority
1. Implement role selection from pool
2. Add more role examples
3. Implement faction abilities

### Medium Priority  
4. Complete multiple role switching
5. Add Passive skill triggers
6. Expand tags usage

### Low Priority
7. Skill cooldown system
8. Role progression/unlocks
9. Faction team bonuses