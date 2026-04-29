---
status: reverse-documented
source: Assets/Scripts/Character/, Assets/Scripts/Role/
date: 2026-04-09
verified-by: HechaoYannet
---

# Character System Design

> **Note**: This document was reverse-engineered from the existing implementation.
> It captures current behavior and inferred design intent. Some sections may be
> incomplete where implementation is partial or intent was unclear.

## Overview

The Character System manages player entities, AI opponents, and character roles in a turn-based card game inspired by "Three Kingdoms Kill" (三国杀). The system handles player state, health management, card ownership, turn-based gameplay, and AI behavior for opponents. Players are associated with Roles that provide unique abilities and faction affiliations.

## Player Fantasy

Players experience:
- Strategic role selection with unique abilities
- Tension of health-based resource management (hand size = health)
- Satisfaction of outsmarting AI opponents with timing and prediction
- Immersion in Three Kingdoms theme through faction-based roles
- Tactical decision-making in both active turns and defensive responses

## Detailed Rules

### Player Entities
1. **Player Class** (base for all playable entities):
   - **PlayerID**: Unique identifier (0: deck, 1: discard, 2+: players)
   - **CurrentHP**: Health points (determines maximum hand size)
   - **CurrentRole**: Active role providing abilities and faction
   - **currentCards**: List of cards currently held
   - **checkingCards**: Cards being evaluated for play

2. **Turn States**:
   - **isInitiativeRound**: Player's active turn (can play cards)
   - **isResponsingRound**: Responding to opponent's attack
   - **IsSubRound**: Within a larger round structure

3. **Card Management**:
   - `GetCard(Card card)`: Add card to hand, set ownership
   - `RemoveAllCard()`: Move all cards to discard (OwnerID = 1)
   - `UseCard(int cardId)`: Play card, trigger effects, remove from hand

### Enemy AI
1. **EnemyAI Class** (inherits from Player):
   - Simulates human decision-making with random delays
   - Overrides response logic with AI-specific behavior
   - Uses same card checking and play mechanics as human players

2. **AI Behavior**:
   - Response delay: 1-2 seconds (simulates thinking)
   - Turn delay: 2-3 seconds (simulates decision making)
   - Card selection: Checks for valid responses using same logic as players

### Role System Integration
1. **Role Properties**:
   - **roleName**: Display name (e.g., "Huang Gai")
   - **roleFaction**: Shu, Wu, Wei, or Qun (Three Kingdoms factions)
   - **skills**: List of unique abilities (Skill objects)
   - **HPmax/HP**: Maximum and current health
   - **Tags**: Dictionary of temporary effects (e.g., "Card|JIU" for intoxication)

2. **Role-Player Relationship**:
   - Each player has one `CurrentRole`
   - Role determines maximum hand size (`GetMaxCardsNum()` returns HP)
   - Role provides faction identity and special abilities

### Player Management
1. **PlayerManager** (Singleton):
   - Maintains list of all player instances
   - Provides lookup by PlayerID: `GetPlayerInstanceByID(int id)`
   - Temporary player reference for testing (`TEMP_PLAYER`)

2. **Player Identification**:
   - ID 0: Deck (card ownership)
   - ID 1: Discard pile (card ownership)  
   - ID 2+: Actual players

### Game Flow Integration
1. **Round States** (from Card System):
   - Players participate in 6-state round flow
   - Each player takes turns in sequence
   - AI and human players follow same state machine

2. **Card Interaction**:
   - Players check card playability based on turn state
   - Active turn: `IsCardAvailable()`
   - Response turn: `IsCardResponsible(ResponsingCard.CardType)`

## Formulas

### Hand Size Limit
```csharp
// From Role.cs:91-94
public virtual int GetMaxCardsNum() {
    return HP;  // Maximum cards = current health
}
```

**Design Intent**: Health-as-resource mechanic. Injured players hold fewer cards, creating tension between maintaining health and card advantage.

### Card Playability Checking
```csharp
// From Player.cs:122-138
if (isInitiativeRound) {
    return currentCards[cardID].IsCardAvailable();
}
else if (isResponsingRound) {
    return currentCards[cardID].IsCardResponsible(ResponsingCard.CardType);
}
```

**Rules**:
- Active turn: Card must be generally playable
- Response turn: Card must counter specific card type being played against you

### AI Decision Timing
```csharp
// From EnemyAI.cs:65,139
yield return new WaitForSeconds(Random.Range(1, 2));   // Response delay
yield return new WaitForSeconds(Random.Range(2, 3f));  // Turn delay
```

**Design Intent**: Simulate human hesitation and decision time. Prevents instant AI responses that feel unfair.

### Turn Time Limit
```csharp
// From Player.cs:225
while (PlayerUIOperation == UIOperation.NONE && currTime < 30f)
```

**Rule**: Players have 30 seconds to make a decision during their turn or when responding.

### Tag Management
```csharp
// From Role.cs:33-46, 65-85
public void AddTag(string tag, int num = 1) {
    if (Tags.ContainsKey(tag)) {
        Tags[tag] = Tags[tag] + num;
    } else {
        Tags.Add(tag, num);
    }
}
public void RemoveTag(string tag, int num = 1, bool isClearingTag = false) {
    // Decrement or remove tag
}
```

**Pattern**: Stackable temporary effects with automatic cleanup (e.g., `RefreshTags()` clears temporary tags each round).

## Edge Cases

### Implemented:
1. **Empty Hand**: Players can have 0 cards (no crash)
2. **AI Timeouts**: AI respects same time limits as players
3. **Role Switching**: `CurrentRole` can be changed (mechanism not shown)
4. **Multiple Players**: `PlayerManager` supports arbitrary number of players
5. **Card Ownership**: Clear distinction between deck (0), discard (1), and player-owned cards (2+)

### Not Implemented/Unclear:
1. **Damage Calculation**: `CurrentHP` can be set but no damage/healing mechanics
2. **Role Abilities**: `skills` list exists but skill activation not implemented
3. **Tag Effects**: Tag system in place but specific tag effects not defined
4. **Role Switching**: Can change `CurrentRole` but no UI/mechanic for doing so
5. **AI Difficulty**: Single AI behavior, no difficulty levels
6. **Network Sync**: PlayerID suggests multiplayer but no networking code

## Dependencies

### Internal Systems:
1. **Card System** (`Card/`, `CardManage/`):
   - Card objects and management
   - Round state machine (`GRoundState`)
   - Card playability checking (`IsCardAvailable()`, `IsCardResponsible()`)

2. **Role System** (`Role/`):
   - Role definitions and abilities
   - Faction system (Shu, Wu, Wei, Qun)
   - Tag management for temporary effects

3. **UI System** (`UI/`):
   - Player operation handling (`UIOperation` enum)
   - Health and card display
   - Button interactions (`Confirm_ButtonClick()`, `Cancel_ButtonClick()`)

4. **3D Visualization** (`CardOnDrawer/`):
   - Card hand display and interaction
   - References `HandManager3D` for layout

### External Systems:
1. **Unity Engine**: MonoBehaviour lifecycle, coroutines, time management
2. **Three Kingdoms Theme**: Cultural context for roles and factions

## Tuning Knobs

### Timing & Balance:
- `Player.cs:225`: Turn time limit (currently 30 seconds)
- `EnemyAI.cs:65`: AI response delay range (1-2 seconds)
- `EnemyAI.cs:139`: AI turn delay range (2-3 seconds)
- `Role.cs:91-94`: Hand size formula (HP = max cards)

### Gameplay:
- Starting health values (not defined in code)
- Damage/healing values (not implemented)
- Skill cooldowns or costs (not implemented)
- Tag effect durations (not implemented)

### AI Behavior:
- Decision randomness (currently simple random delays)
- Card priority logic (not implemented)
- Threat assessment (not implemented)
- Difficulty scaling (not implemented)

## Acceptance Criteria

### Functional Requirements:
- [x] Player entities can be created with ID, health, role
- [x] Players can receive, hold, and play cards
- [x] Turn states correctly managed (active vs response)
- [x] AI opponents simulate human decision timing
- [x] Player manager tracks all player instances
- [x] Role integration provides faction and ability framework
- [ ] Damage and healing mechanics implemented
- [ ] Role abilities are functional
- [ ] Tag system has defined effects
- [ ] AI has strategic decision making

### Performance Requirements:
- [ ] Player state updates are efficient
- [ ] AI decision making doesn't cause frame drops
- [ ] Player manager lookups are fast (list iteration)

### UX Requirements:
- [ ] Health and card count clearly displayed
- [ ] Turn state visually indicated
- [ ] AI "thinking" time feels natural
- [ ] Role abilities clearly explained

## Implementation Notes

### Code Patterns:
1. **Inheritance Hierarchy**: `EnemyAI` extends `Player` for shared behavior
2. **Singleton Manager**: `PlayerManager` for global player access
3. **Coroutine-Based Timing**: Turn timers and AI delays use Unity coroutines
4. **State Flags**: Boolean flags for turn states (`isInitiativeRound`, etc.)
5. **Event-Like Pattern**: `UIOperation` enum for UI communication

### Technical Debt:
1. **Health Mechanics**: `CurrentHP` setter exists but no damage system
2. **Skill System**: `skills` list defined but no activation logic
3. **Hardcoded IDs**: PlayerID meanings (0=deck, 1=discard) are magic numbers
4. **AI Simplicity**: Basic random delays, no strategic decision making
5. **Commented Code**: Significant commented-out functionality (UI integration)

### Cultural Context:
The system is designed for a "Three Kingdoms Kill" (三国杀) style game:
- **Factions**: Shu (蜀), Wu (吴), Wei (魏), Qun (群) - historical Chinese kingdoms
- **Role Types**: Likely includes Generals, Strategists, etc. with unique abilities
- **Health as Hand Limit**: Common in Chinese card games for resource tension
- **Tag System**: Similar to "status effect" systems in Chinese card games

## Future Extensions

### Planned Features (Inferred from Code):
1. **Damage System**: Implement card-based damage and healing
2. **Skill Activation**: Complete role ability system
3. **Tag Effects**: Define specific tag mechanics (e.g., intoxication, poison)
4. **Advanced AI**: Strategic card selection and threat assessment
5. **Multiplayer**: Network synchronization for multiple players
6. **Role Switching**: Mechanism to change roles during gameplay

### Suggested Improvements:
1. **Data-Driven Roles**: Move role definitions to config files/scriptable objects
2. **AI Behavior Trees**: Replace simple delays with decision trees
3. **Health UI Integration**: Connect health changes to visual feedback
4. **Skill Tooltips**: UI explanations for role abilities
5. **Difficulty Settings**: Adjustable AI intelligence levels
6. **Player Profiles**: Save player preferences and statistics