---
status: reverse-documented
source: Assets/Scripts/UI/
date: 2026-04-09
verified-by: User
---

# UI System Design

> **Note**: This document was reverse-engineered from the existing implementation.
> It captures current behavior and clarified design intent. Some sections may be
> incomplete where implementation is partial or intent was unclear.

## Overview

The UI system provides dual-space interface for Three Kingdoms card game: Screen Space for HUD/menus and World Space for in-game visuals. Designed for responsive cross-platform play (mobile touch + PC mouse).

## Player Fantasy

Immersive tabletop experience with dynamic camera views, satisfying card interactions, and clear game state feedback. Players feel like commanding generals with tactical overviews and detailed battlefield information.

## Detailed Rules

### UI Flow Architecture
```
Main Screen → Functional Menu → Gaming Screen → Results Screen
```

**Functional Menu**: Transition screen containing features not fitting in Main or Gaming screens.

### Dual-Space UI System

#### 1. Screen Space UI (Canvas - Screen Space)
**Purpose**: HUD, menus, player interface
**Core Components**:
- `TurnIndicator`: Round/turn tracking with player indicators
- `PlayerStatus`: Health, role, faction display
- `HandArea`: Card hand organization and management
- `ActionButtons`: Skill activation, game actions
- **More planned**: Additional screens and components

**Features**:
- Object pooling for performance
- `IScreenSpaceElement` interface for HUD updates
- Preloaded common elements

#### 2. World Space UI (Canvas - World Space)
**Purpose**: In-game visual feedback, character attachments
**Attachment Types**:
- **Health bars** above characters
- **Status icons** near affected targets
- **Damage numbers** at hit locations
- **Character indicators** for selection

**Components**:
- `DamageTextController`: Floating damage numbers with critical effects
- `HealTextController`: Healing feedback
- `CharacterHealthBar`: Health tracking above characters
- `StatusEffectIcon`: Buff/debuff visualization

### Camera System
**Views**:
- `tableTopView`: Default table overview
- `playerFocusView`: Close-up on player hand/area
- `overviewView`: Strategic battlefield view

**Control**: Automatic transitions based on game state
- Switch to `playerFocusView` during player's turn
- Return to `tableTopView` for opponent's turn
- `overviewView` for critical game moments

### Card Interaction System
**3D Card Manipulation**:
- **Click and drag**: Play cards from hand to battlefield
- **Hover for details**: Show card information on hover
- **Visual feedback**: Highlight playable cards

**Card Effects**:
- **Draw animation**: Bezier curve flight from deck to hand
- **Play effects**: Particle systems based on card type
- **Rarity visuals**: Emission colors (gray→blue→magenta→yellow→red)
- **3D models**: Card-specific 3D representations

### Visual Effects Pipeline
**Card Effects Manager**:
- Bezier curve flight paths with height control
- Rarity-based material effects
- Type-colored particle systems

**Damage/Heal Feedback**:
- Floating text with animation curves
- Critical hit visual differentiation
- World-space positioning at impact locations

### Platform Adaptation
**Responsive Design**:
- **Mobile**: Touch-optimized controls, larger hit areas
- **PC**: Mouse/keyboard precision, hover states
- **Shared Core**: Same UI components with input adaptation

**Input Handling**:
- Touch gestures (tap, drag, pinch)
- Mouse interactions (click, hover, drag)
- Keyboard shortcuts (PC enhancement)

## Formulas

### Card Flight Path
```
Bezier Curve: B(t) = (1-t)²P₀ + 2(1-t)tP₁ + t²P₂
Where:
  P₀ = Start position (deck)
  P₁ = Control point (midpoint + flight height)
  P₂ = End position (hand)
  t = Normalized time (0→1)
```

### Damage Text Animation
```
Position(t) = Lerp(start, start + height, floatCurve(t))
Alpha(t) = fadeCurve(t)
```

### Card Rarity Colors
| Rarity | Emission Color | Multiplier |
|--------|----------------|------------|
| Common | Gray | 0.2x |
| Uncommon | Blue | 0.5x |
| Rare | Magenta | 0.8x |
| Epic | Yellow | 1.2x |
| Legendary | Red | 1.5x + dynamic glow |

## Edge Cases

### Screen Space
- **Multiple active screens**: Layer management for overlapping UI
- **Resolution changes**: Responsive layout adaptation
- **Input conflict**: Touch vs mouse priority handling

### World Space
- **Object occlusion**: Ensure UI visible behind objects
- **Camera-relative rotation**: Keep UI facing camera
- **Performance**: Object pool limits for particle systems

### Camera Transitions
- **Interrupted transitions**: Smooth cancellation/resumption
- **View obstruction**: Ensure clear line of sight
- **Transition timing**: Match game pace

## Dependencies

### Required Systems
- **Card System**: Card data for 3D models and effects
- **Character System**: Health/status for world UI
- **Role System**: Skill buttons and player status
- **Event System**: UI update triggers

### Integration Points
1. Card draw → 3D flight animation
2. Damage event → World space damage text
3. Turn change → Screen space indicator update
4. Skill activation → Action button feedback

## Tuning Knobs

### Visual Parameters
- **Card flight height**: 2.0f (configurable)
- **Card flight duration**: 0.5f seconds
- **Damage float height**: 2f units
- **Damage duration**: 1f second
- **Animation curves**: Customizable for feel

### Performance Limits
- **Object pool sizes**: Preloaded element counts
- **Particle limits**: Maximum concurrent effects
- **UI update rate**: HUD refresh frequency

### Platform Settings
- **Touch sensitivity**: Mobile interaction thresholds
- **Hover delays**: PC hover activation timing
- **Input dead zones**: Platform-specific adjustments

## Acceptance Criteria

### Core Functionality
- [x] Dual-space UI architecture
- [x] Object pooling system
- [x] Screen space core components
- [x] World space attachment system
- [x] Camera view management
- [x] Card interaction framework
- [x] Visual effects pipeline

### Missing Features
- [ ] Complete screen flow implementation
- [ ] Additional screen space components
- [ ] Mobile touch optimization
- [ ] PC keyboard shortcuts
- [ ] Settings/options screens
- [ ] Pause menu system
- [ ] Tutorial/help UI

## Implementation Notes

### Current State
- Framework complete with core components
- Visual effects system functional
- Camera system with three views
- Basic input handling

### Code Structure
```
UIFramework/
  UIManager.cs              # Main coordinator
  ScreenSpaceUIManager.cs   # HUD/menu management
  WorldSpaceUIManager.cs    # In-game visuals
  CardEffectsManager.cs     # 3D card effects
  SceneCameraManager.cs     # Camera control

UIComponent/
  DamageTextController.cs   # Damage feedback
  HealTextController.cs     # Healing feedback
  TurnIndicator.cs          # Round tracking
```

### Design Decisions
1. **Dual-space separation**: Clear visual hierarchy
2. **Object pooling**: Performance optimization
3. **Automatic camera**: Cinematic game flow
4. **Responsive design**: Cross-platform support
5. **3D card effects**: Immersive tabletop feel

## Follow-Up Work

### High Priority
1. Complete Main → Functional Menu → Gaming → Results flow
2. Implement mobile touch optimization
3. Add PC keyboard shortcuts

### Medium Priority
4. Create additional screen space components
5. Implement settings/options screens
6. Add pause menu system

### Low Priority
7. Tutorial/help UI implementation
8. Advanced visual effects
9. Accessibility features (colorblind mode, etc.)