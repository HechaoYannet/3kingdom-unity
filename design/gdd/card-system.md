---
status: reverse-documented
source: Assets/Scripts/Card/, Assets/Scripts/CardManage/, Assets/Scripts/CardOnDrawer/
date: 2026-04-30
verified-by: HechaoYannet
---

# Card System Design

> **Note**: This document was reverse-engineered from the existing implementation.
> It captures current behavior and inferred design intent. Some sections may be
> incomplete where implementation is partial or intent was unclear.

**Status**: In Review

## Overview

The Card System is the deterministic gameplay core of a turn-based card game with 3D visualization. The system manages card creation, deck management, player hands, turn-based gameplay, and 3D card interaction. The game appears to be inspired by Chinese card games like "Three Kingdoms Kill" with card types "Sha", "Shan", and "Tao".

The current product direction requires this system to remain authoritative for legality and result computation while feeding a separate battle-presentation layer for camera, VFX, timing, and feedback.

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
1. **Sha** - Attack card (implemented in `Cards/Sha.cs`)
2. **Shan** - Defense/Evade card (referenced but not implemented)
3. **Tao** - Healing/Recovery card (referenced but not implemented)

### Card Properties
Each card has:
- **cardName**: Display name
- **cardImage**: Visual representation sprite
- **description**: Card effect description
- **CardType**: Type identifier
- **OwnerID**: Player who owns or controls the card
- **rarity** (via `CardData`): Common, Uncommon, Rare, Epic, Legendary
- **templateID**: Unique identifier for card data

### Game Flow
1. **Round States** (implemented in `RoundManager.cs`):
   - **Preparing**
   - **Checking**
   - **GettingCard**
   - **Battling**
   - **RefusingCard**
   - **Ending**

2. **Turn Structure**:
   - Players take turns in sequence
   - Each turn follows the 6-state round flow
   - Game checks for victory/defeat conditions each round

3. **Presentation Boundary Direction**:
   - Card play should eventually emit deterministic action/result payloads
   - Battle spectacle, camera emphasis, and VFX timing should consume those payloads rather than decide outcomes
   - The current implementation still mixes direct runtime actions with presentation-side behavior

### Card Management
- **Deck**: `cardsStack`
- **Discard**: `usedCards`
- **Hand**: `currentCards`
- **Shuffle**: When draw pile empties, discard pile is shuffled into draw pile

### 3D Interaction
- Cards can be dragged in 3D space
- Play zones detect valid card placement
- Hand arranges cards in arc formation facing center
- Visual feedback exists for drag states and valid/invalid plays

## Formulas

### Card Distribution
```csharp
if (cardsStack.Count <= 3) return "Sha";
else if (cardsStack.Count <= 6) return "Shan";
else if (cardsStack.Count <= 10) return "Tao";
else return "Sha";
```

### Hand Layout
```csharp
float angle = startAngle + i * angleStep;
float rad = angle * Mathf.Deg2Rad;
Vector3 pos = handCenter.position + new Vector3(
    Mathf.Sin(rad) * radius,
    verticalOffset,
    Mathf.Cos(rad) * radius
);
```

### Shuffle Algorithm
```csharp
for (int i = 0; i < cardsStack.Count; i++) {
    int index = Random.Range(0, cardsStack.Count);
    Card temp = cardsStack[i];
    cardsStack[i] = cardsStack[index];
    cardsStack[index] = temp;
}
```

## Edge Cases

### Implemented
1. Empty draw pile reshuffles discard into draw
2. Dragging cards are excluded from hand layout updates
3. Invalid plays return cards to the original position
4. Multiple players are supported through `PlayerManager`

### Not Implemented / Unclear
1. `DoCardsAction()` and `ResponseTrigger()` are not fully implemented across card types
2. Role abilities are referenced but not fully integrated
3. Card combos are not evident
4. Networking is not implemented
5. Max hand size is defined but not consistently enforced

## Dependencies

### Internal Systems
1. **Player System** (`Character/Player.cs`)
2. **Role System** (`Role/`)
3. **UI System** (`UI/`)
4. **Audio System** (`Audio/`)
5. **Asset Management** (`AssetBundleFramework/`)
6. **Battle Presentation Layer**
   - `Docs/architecture/battle-presentation-first-architecture.md`
   - `Docs/architecture/battle-action-schema.md`
   - This layer may consume card results, but must not own authoritative legality or damage results

### External Systems
1. **Unity Engine**: 3D rendering, physics, input handling
2. **Tuanjie Engine**: project runtime environment

## Tuning Knobs

### Card Distribution
- `CardManager.GetNextCardName()` thresholds
- Shuffle randomness

### 3D Visualization
- `dragHeightOffset`
- `dragScaleMultiplier`
- `radius`
- `angleRange`
- `verticalOffset`

### Game Balance
- Starting hand size
- Max hand size
- Deck size and distribution

## Acceptance Criteria

### Functional Requirements
- [x] Cards can be created with name, image, description, and type
- [x] Deck can be shuffled with card type distribution
- [x] Players can draw cards from deck
- [x] Round states transition correctly
- [x] 3D cards can be dragged and interact with play zones
- [x] Hand arranges cards in arc formation
- [ ] Card effects are implemented and functional
- [ ] Role abilities are implemented and functional
- [ ] Game end conditions are properly checked
- [ ] Card actions emit a clean rule-result boundary for battle presentation

### Performance Requirements
- [ ] 3D card rendering maintains target framerate
- [ ] Hand layout updates are efficient
- [ ] Card instantiation/destruction uses object pooling

### UX Requirements
- [ ] Card dragging provides clear visual feedback
- [ ] Valid/invalid play zones are visually distinct
- [ ] Hand layout is readable and accessible

## Implementation Notes

### Code Patterns
1. `CardManager` uses a singleton pattern
2. `Card` is an abstract base class
3. 3D visualization is separated into `Card3D`, `CardView3D`, `HandManager3D`
4. `RoundManager` uses a 6-state flow
5. Rule resolution and presentation hooks are not yet cleanly separated

### Technical Debt
1. Parts of the card flow still use `Resources.Load()` despite the project also containing an AssetBundle framework
2. Card distribution thresholds are magic numbers
3. Several virtual or abstract methods remain incomplete
4. Significant amounts of commented-out code remain
5. The planned `RuleActionRequest` / `RuleActionResult` boundary is not yet implemented

## Future Extensions

### Planned Features
1. Implement `DoCardsAction()` for each card type
2. Complete role ability implementations
3. Extend ownership and identity handling for multiplayer-safe state
4. Expand card data and rarity usage
5. Implement card play VFX and animations
6. Bind card results to reusable battle-presentation templates

### Suggested Improvements
1. Move card distribution rules into data
2. Pool card `GameObject`s
3. Support touch, mouse, and gamepad through a cleaner input layer
4. Improve accessibility
5. Add localization support for card text
