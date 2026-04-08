---
status: reverse-documented
source: Assets/Scripts/Card/, Assets/Scripts/CardManage/, Assets/Scripts/CardOnDrawer/
date: 2026-04-09
verified-by: HechaoYannet
---

# Card System Design

> **Note**: This document was reverse-engineered from the existing implementation.
> It captures current behavior and inferred design intent. Some sections may be
> incomplete where implementation is partial or intent was unclear.

## Overview

The Card System is the core gameplay mechanic of a turn-based card game with 3D visualization. The system manages card creation, deck management, player hands, turn-based gameplay, and 3D card interaction. The game appears to be inspired by Chinese card games like "Three Kingdoms Kill" (三国杀) with card types "Sha", "Shan", and "Tao".

## Player Fantasy

Players experience strategic card gameplay with:
- Tactical decision-making in turn-based combat
- Physicality of 3D card manipulation and placement
- Collection and management of cards with different rarities
- Role-based gameplay with unique character abilities
- Tension of resource management and timing

## Detailed Rules

### Card Types
Based on code analysis, three card types are referenced:
1. **Sha** (杀) - Attack card (implemented in `Cards/Sha.cs`)
2. **Shan** (闪) - Defense/Evade card (referenced but not implemented)
3. **Tao** (桃) - Healing/Recovery card (referenced but not implemented)

### Card Properties
Each card has:
- **cardName**: Display name (e.g., "Sha")
- **cardImage**: Visual representation sprite
- **description**: Card effect description
- **CardType**: Type identifier ("Sha", "Shan", "Tao")
- **OwnerID**: Player who owns/controls the card
- **rarity** (via CardData): Common, Uncommon, Rare, Epic, Legendary
- **templateID**: Unique identifier for card data

### Game Flow
1. **Round States** (implemented in `RoundManager.cs`):
   - **Preparing**: Role abilities activate
   - **Checking**: Card effect resolution
   - **GettingCard**: Draw phase (4 cards per player)
   - **Battling**: Main action phase
   - **RefusingCard**: Discard excess cards
   - **Ending**: Cleanup and transition

2. **Turn Structure**:
   - Players take turns in sequence
   - Each turn follows the 6-state round flow
   - Game checks for victory/defeat conditions each round

### Card Management
- **Deck**: `cardsStack` - Draw pile managed by CardManager
- **Discard**: `usedCards` - Played/discarded cards
- **Hand**: `currentCards` - Cards held by player
- **Shuffle**: When draw pile empties, discard pile is shuffled into draw pile

### 3D Interaction
- Cards can be dragged in 3D space
- Play zones detect valid card placement
- Hand arranges cards in arc formation facing center
- Visual feedback for drag states and valid/invalid plays

## Formulas

### Card Distribution (Shuffle Algorithm)
```csharp
// From CardManager.cs:54-69
if (cardsStack.Count <= 3) return "Sha";
else if (cardsStack.Count <= 6) return "Shan";  
else if (cardsStack.Count <= 10) return "Tao";
else return "Sha";
```

**Interpretation**: Early in shuffle creates more "Sha" cards, middle creates "Shan", later creates "Tao". This suggests intentional pacing of card types.

### Hand Layout (3D Arc)
```csharp
// From HandManager3D.cs:103-109
float angle = startAngle + i * angleStep;
float rad = angle * Mathf.Deg2Rad;
Vector3 pos = handCenter.position + new Vector3(
    Mathf.Sin(rad) * radius,
    verticalOffset,
    Mathf.Cos(rad) * radius
);
```

**Parameters**:
- `radius`: Distance from hand center (default: 5f)
- `angleRange`: Total arc angle (default: 60°)
- `verticalOffset`: Height adjustment (default: 0f)

### Shuffle Algorithm
```csharp
// Fisher-Yates shuffle implementation
for (int i = 0; i < cardsStack.Count; i++) {
    int index = Random.Range(0, cardsStack.Count);
    Card temp = cardsStack[i];
    cardsStack[i] = cardsStack[index];
    cardsStack[index] = temp;
}
```

## Edge Cases

### Implemented:
1. **Empty Draw Pile**: Automatically reshuffles discard pile into draw pile
2. **Card Dragging**: Cards being dragged are excluded from hand layout updates
3. **Invalid Play**: Cards dragged outside play zones return to original position
4. **Multiple Players**: Round manager iterates through all players in PlayerManager

### Not Implemented/Unclear:
1. **Card Effect Resolution**: `DoCardsAction()` and `ResponseTrigger()` are virtual/abstract with no implementations
2. **Role Abilities**: Role system referenced but specific abilities not implemented
3. **Card Combos**: No evidence of card interaction or combo systems
4. **Network Sync**: OwnerID suggests multiplayer but networking not implemented
5. **Card Limits**: Max hand size defined (10) but not enforced in current implementation

## Dependencies

### Internal Systems:
1. **Player System** (`Character/Player.cs`):
   - Manages player state, HP, card ownership
   - Provides `GetCard()` method for card acquisition
   - Tracks current cards and checking cards

2. **Role System** (`Role/`):
   - Provides character-specific abilities
   - Referenced but implementation unclear

3. **UI System** (`UI/`):
   - Displays card information and game state
   - Manages button interactions and visual feedback

4. **Audio System** (`Audio/`):
   - Plays card sound effects (referenced in commented code)

5. **Asset Management** (`AssetBundleFramework/`):
   - Loads card assets and resources

### External Systems:
1. **Unity Engine**: 3D rendering, physics, input handling
2. **Tuanjie Engine**: China-specific Unity customization

## Tuning Knobs

### Card Distribution:
- `CardManager.GetNextCardName()` thresholds (3, 6, 10)
- Shuffle algorithm seed and randomness

### 3D Visualization:
- `dragHeightOffset`: Height when dragging (default: 1.0)
- `dragScaleMultiplier`: Scale when dragging (default: 1.2)
- `radius`: Hand arc radius (default: 5f)
- `angleRange`: Hand arc angle (default: 60°)
- `verticalOffset`: Hand height (default: 0f)

### Game Balance:
- Starting hand size (currently: 4 cards)
- Max hand size (defined: 10, not enforced)
- Deck size for shuffle (example: 10 cards in `RoundManager.cs:34`)

## Acceptance Criteria

### Functional Requirements:
- [x] Cards can be created with name, image, description, type
- [x] Deck can be shuffled with card type distribution
- [x] Players can draw cards from deck
- [x] Round states transition correctly
- [x] 3D cards can be dragged and interact with play zones
- [x] Hand arranges cards in arc formation
- [ ] Card effects are implemented and functional
- [ ] Role abilities are implemented and functional
- [ ] Game end conditions are properly checked

### Performance Requirements:
- [ ] 3D card rendering maintains target framerate
- [ ] Hand layout updates are efficient
- [ ] Card instantiation/destruction uses object pooling

### UX Requirements:
- [ ] Card dragging provides clear visual feedback
- [ ] Valid/invalid play zones are visually distinct
- [ ] Hand layout is readable and accessible

## Implementation Notes

### Code Patterns:
1. **Singleton Pattern**: `CardManager` uses singleton for global access
2. **Abstract Base Class**: `Card` provides template for all card types
3. **Component Architecture**: 3D visualization separated into `Card3D`, `CardView3D`, `HandManager3D`
4. **State Machine**: `RoundManager` implements 6-state round flow

### Technical Debt:
1. **Resource Loading**: Uses `Resources.Load()` instead of Addressables/AssetBundles
2. **Hardcoded Values**: Card distribution thresholds are magic numbers
3. **Incomplete Methods**: Virtual/abstract methods lack implementations
4. **Commented Code**: Significant amounts of commented-out functionality

### Cultural Context:
The card types "Sha", "Shan", "Tao" are from Chinese card game "Three Kingdoms Kill":
- **Sha** (杀): Attack, requires target to play "Shan" to avoid
- **Shan** (闪): Dodge/Defense, counters "Sha"
- **Tao** (桃): Heal, restores health points

## Future Extensions

### Planned Features (Inferred from Code):
1. **Card Effects**: Implement `DoCardsAction()` for each card type
2. **Role System**: Complete role ability implementations
3. **Network Play**: Extend OwnerID for multiplayer synchronization
4. **Card Collection**: Expand card data and rarity system
5. **Visual Effects**: Implement card play VFX and animations

### Suggested Improvements:
1. **Data-Driven Design**: Move card distribution rules to config files
2. **Object Pooling**: Pool card GameObjects for performance
3. **Input Abstraction**: Support touch, mouse, and gamepad uniformly
4. **Accessibility**: Add screen reader support and colorblind modes
5. **Localization**: Support multiple languages for card text