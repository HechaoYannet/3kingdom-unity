---
status: reverse-documented
source: Assets/Scripts/Character/, Assets/Scripts/Role/
date: 2026-04-30
verified-by: HechaoYannet
---

# Character System Design

> **Note**: This document was reverse-engineered from the existing implementation.
> It captures current behavior and inferred design intent. Some sections may be
> incomplete where implementation is partial or intent was unclear.

**Status**: In Review

## Overview

The Character System manages player entities, AI opponents, and character roles in a turn-based card game inspired by "Three Kingdoms Kill". The system handles player state, health management, card ownership, turn-based gameplay, and AI behavior for opponents. Players are associated with Roles that provide unique abilities and faction affiliations.

In the current battle-presentation-first direction, this system remains part of the deterministic rule core and should eventually emit structured action/result data for downstream presentation.

## Player Fantasy

Players experience:
- Strategic role selection with unique abilities
- Tension of health-based resource management
- Satisfaction of outsmarting AI opponents
- Immersion in Three Kingdoms faction identity
- Tactical decision-making in both active turns and defensive responses

## Detailed Rules

### Player Entities
1. **Player Class**
   - `PlayerID`
   - `CurrentHP`
   - `CurrentRole`
   - `currentCards`
   - `checkingCards`

2. **Turn States**
   - `isInitiativeRound`
   - `isResponsingRound`
   - `IsSubRound`

3. **Card Management**
   - `GetCard(Card card)`
   - `RemoveAllCard()`
   - `UseCard(int cardId)`

### Enemy AI
1. **EnemyAI Class**
   - Inherits from `Player`
   - Uses the same checking and play mechanics
   - Adds AI-specific delays

2. **AI Behavior**
   - Response delay: 1-2 seconds
   - Turn delay: 2-3 seconds
   - Checks for valid responses using the same logic as players

### Role System Integration
1. **Role Properties**
   - `roleName`
   - `roleFaction`
   - `skills`
   - `HPmax` / `HP`
   - `Tags`

2. **Role-Player Relationship**
   - Each player has one `CurrentRole`
   - Role determines maximum hand size
   - Role provides faction identity and special abilities

### Player Management
1. **PlayerManager**
   - Maintains the player list
   - Provides lookup by `PlayerID`
   - Includes temporary/testing references

2. **Player Identification**
   - `0`: Deck
   - `1`: Discard pile
   - `2+`: Actual players

## Formulas

### Hand Size Limit
```csharp
public virtual int GetMaxCardsNum() {
    return HP;
}
```

### Card Playability
```csharp
if (isInitiativeRound) {
    return currentCards[cardID].IsCardAvailable();
}
else if (isResponsingRound) {
    return currentCards[cardID].IsCardResponsible(ResponsingCard.CardType);
}
```

### AI Timing
```csharp
yield return new WaitForSeconds(Random.Range(1, 2));
yield return new WaitForSeconds(Random.Range(2, 3f));
```

### Turn Time Limit
```csharp
while (PlayerUIOperation == UIOperation.NONE && currTime < 30f)
```

## Edge Cases

### Implemented
1. Players can have an empty hand
2. AI respects the same time limits
3. `CurrentRole` can be reassigned
4. `PlayerManager` supports multiple players
5. Card ownership uses distinct deck/discard/player IDs

### Not Implemented / Unclear
1. Damage and healing are not fully implemented
2. Skill activation logic is incomplete
3. Tag effects are not fully defined
4. AI has no difficulty model
5. Networking is not implemented

## Dependencies

### Internal Systems
1. **Card System**
2. **Role System**
3. **UI System**
4. **3D Visualization**
5. **Battle Presentation Layer**
   - Future action presentation should consume resolved actor/target/result payloads from character-driven gameplay flow
   - Camera timing and spectacle must not own turn or damage decisions

### External Systems
1. **Unity Engine**: coroutines, lifecycle, timing
2. **Three Kingdoms theme**: roles and factions

## Tuning Knobs

- Turn time limit
- AI response delay range
- AI turn delay range
- HP-driven hand-size rule
- Future skill cooldowns and tag durations

## Acceptance Criteria

### Functional Requirements
- [x] Player entities can be created with ID, health, and role
- [x] Players can receive, hold, and play cards
- [x] Turn states are managed
- [x] AI opponents simulate human decision timing
- [x] `PlayerManager` tracks all player instances
- [x] Role integration provides faction and ability framework
- [ ] Damage and healing are fully implemented
- [ ] Role abilities are functional
- [ ] Tag system has defined effects
- [ ] Character-driven actions emit structured data for downstream presentation

## Implementation Notes

### Code Patterns
1. `EnemyAI` extends `Player`
2. `PlayerManager` is a singleton
3. Turn timers and AI delays use coroutines
4. Turn state uses boolean flags
5. UI communication uses `UIOperation`

### Technical Debt
1. No complete damage system
2. Skills are defined but not fully activated
3. Player IDs use magic numbers
4. AI behavior is simplistic
5. Presentation-facing action/result payloads are still missing
