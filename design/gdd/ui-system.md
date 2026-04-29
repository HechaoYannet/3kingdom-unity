---
status: reverse-documented
source: Assets/Scripts/UI/
date: 2026-04-30
verified-by: User
---

# UI System Design

> **Note**: This document was reverse-engineered from the existing implementation.
> It captures current behavior and clarified design intent. Some sections may be
> incomplete where implementation is partial or intent was unclear.

**Status**: In Review

## Overview

The UI system provides dual-space interface for the Three Kingdoms card game: screen-space UI for HUD and menus, plus world-space UI for in-game feedback. It targets both mobile touch and PC mouse interaction.

The current project direction treats UI and battle feedback as part of a broader battle-presentation layer that sits on top of deterministic card and turn logic.

## Player Fantasy

Players should get a readable but dramatic battlefield experience with strong state feedback, satisfying card interactions, and camera-assisted emphasis when actions resolve.

## Detailed Rules

### UI Flow Architecture
```text
Main Screen -> Functional Menu -> Gaming Screen -> Results Screen
```

### Dual-Space UI System

#### 1. Screen Space UI
**Purpose**: HUD, menus, player interface

Core components include:
- `TurnIndicator`
- `PlayerStatus`
- `HandArea`
- `ActionButtons`

#### 2. World Space UI
**Purpose**: In-game visual feedback attached to the battle space

Core feedback types include:
- Health bars
- Status icons
- Damage numbers
- Character indicators

### Camera System
**Target Views**:
- `tableTopView`
- `playerFocusView`
- `overviewView`

**Control Goal**:
- Focus on the current player during active turns
- Return to a readable baseline during non-active turns
- Use stronger emphasis only for important moments

**Current Validation Status**:
- `Cinemachine` and `Timeline` are installed and available
- The current battle scene layout and baseline camera are not yet accepted as final
- Camera placement and long-term camera organization still require review

### Card Interaction System
- Click/drag card play
- Hover or focus for details
- Highlight playable cards

### Visual Effects Pipeline
- Bezier-style card travel can support draw and play effects
- Rarity-based materials and particles are intended hooks
- Damage and healing feedback are expected to remain readable in world space

**Current Tooling Route**:
- `URP` is active for the current slice
- `Shader Graph` and `VFX Graph` are installed
- Premium effects should keep a fallback path for lower-end or unfinished states

## Dependencies

### Required Systems
- **Card System**
- **Character System**
- **Role System**
- **Event System**

### Integration Direction
1. Card draw should trigger a presentation-safe visual path
2. Damage events should drive world-space feedback
3. Turn changes should update readable HUD state
4. Skill activation should support both UI feedback and battle emphasis

## Acceptance Criteria

### Core Functionality
- [x] Dual-space UI architecture exists
- [x] World-space feedback hooks exist
- [x] Card interaction framework exists
- [x] Battle-feedback direction is defined
- [ ] Finalized battle camera baseline
- [ ] Complete screen flow implementation
- [ ] Mobile touch optimization
- [ ] Keyboard shortcut support
- [ ] Settings, pause, and tutorial flows

## Implementation Notes

### Current State
- Core UI framework exists, but battle-presentation integration is still incomplete
- Visual effects route is now planned against URP
- Cinemachine/Timeline tooling is available, but the baseline battle camera setup is still under review
- Basic input handling exists
