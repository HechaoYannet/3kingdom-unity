# UI Presentation Architecture

## Status

- Date: 2026-04-30
- Status: Accepted
- Scope: `Assets/Scripts/UI/`, `Assets/Scripts/CardOnDrawer/`, `Assets/Scripts/CardManage/`

## Context

The current battle runtime inherited a world-space 3D hand interaction model:

- hand cards are runtime scene objects
- card interaction is driven by `OnMouse*`
- camera readability and hand readability are coupled
- screen-space and world-space UI responsibilities are only partially defined

This blocks three product goals:

1. mobile-first drag interaction
2. high-quality hover/press/drag animation
3. clean separation between deterministic battle rules and battle presentation

The project direction already defines battle presentation as downstream from rule authority, so the hand should become a command surface rather than a world-space gameplay object.

## Decision

Adopt a four-layer presentation split:

1. `Battle World`
2. `World Feedback`
3. `Screen HUD`
4. `Overlay FX`

### Screen HUD

The player hand is owned by screen-space UGUI and stays visible as a fixed HUD surface.

- runtime UI system: `UGUI + TMP`
- root canvas mode: `Screen Space - Overlay`
- hand interaction: drag-first on mobile, click/target as secondary support path
- card layout: fan layout in screen space
- card rendering: layered 2D card widgets with back layer, art layer, and front layer

### Layered 2D Hand Cards

Persistent hand cards use a 2D layered composition:

- background layer for color glow and depth base
- main art/body layer for card image and readable content
- foreground layer for sheen, edge tint, and subtle motion

These layers can shift slightly from pointer movement and gyroscope input to recover depth cues without reintroducing world-space hand management.

### Hover Detection

Hand card hover detection uses **presenter polling against cached ground rectangles**.

- EventSystem raycasts (IPointerEnter/Exit) are **not** used for hover state — they fire against a moving visual rect during DOTween lift animation, creating a perpetual re-enter → DOKill → restart cycle
- Instead, `BattleHandPresenter` stores a `cardGroundRects` dictionary updated once per `LayoutCards()` pass
- Each `LateUpdate`, `UpdateHoverFromPointer()` converts `Input.mousePosition` to `handAnchor` local space and tests containment against the cached ground rects
- Ground rects are the **rest position** (before hover lift / push / selection offset is applied), so they never move during DOTween playback
- On hover change: `layoutDirty` is set; `SetHovered(bool)` is pushed to `BattleHandCardView` for parallax consumption
- Benefit: DOTween animations play through uninterrupted — no Kill/restart, no easing curve truncation
- Tradeoff: removes the natural "accurate thin-target" detection of visual rect raycast; ground rects should be generous enough (matches card size) to avoid false negative near card edges

This solves three previously intractable bugs:

1. **Hover jump**: visual rect moves up → EventSystem re-fires OnPointerEnter → DOKill kills active tween → animation resets from current position → perceived jump
2. **Stuck hover after click**: card animates down from hoverLift, visual rect sweeps over cursor → OnPointerEnter re-fires → card goes back up
3. **Easing never plays through**: tween killed every frame, only first frame of easing curve ever runs

### World Feedback

World-space UGUI remains responsible for:

- damage numbers
- healing numbers
- status icons
- character-bound feedback

### 3D Card Objects

3D card objects are no longer the primary hand interaction surface.

They remain valid for:

- draw/pull-in presentation
- card launch / flight
- impact / hit staging
- hero-moment inserts
- large preview or showcase moments

### Input Direction

The long-term input route is Unity Input System plus EventSystem.

- target package: `com.unity.inputsystem`
- runtime interaction should use pointer and drag interfaces, not `OnMouse*`
- the bootstrap path may temporarily fall back to legacy `StandaloneInputModule` until the package is enabled in-editor

### Camera Responsibility

Camera language serves battle staging, not hand readability.

- camera owns baseline view, action emphasis, and hero moments
- hand readability must not depend on zooming the battle camera toward the player

## Rationale

This architecture separates command input from spectacle:

- screen-space hand UI is easier to read on PC and mobile
- drag interaction becomes consistent across mouse and touch
- layered 2D cards allow depth and polish while preserving stable sort order
- battle camera can focus on stage readability instead of compensating for hand visibility

## Consequences

Positive:

- removes the largest coupling between battle rules and hand presentation
- enables high-quality hover and drag animation
- supports gyroscope-assisted micro-parallax without moving gameplay authority into presentation
- creates a stable base for target highlighting and card-preview UX

Tradeoffs:

- existing `HandManager3D` and `Card3D` become legacy for primary hand input
- a runtime bootstrap layer is required until the battle scene is cleaned up in-editor
- world-space target indicators still need a dedicated follow-up after the first screen-space target-selection layer

## Follow-up

- add `Input System` package and switch active input handling in-editor
- replace the inactive battle scene UI placeholders with accepted runtime-prefab or scene-root assets
- extend the first explicit target selection UI into accepted world-space target feedback
- route card-play presentation clones from rule results instead of hand widgets themselves
