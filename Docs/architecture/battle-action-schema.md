# Battle Action Schema

**Status**: Draft
**Created**: 2026-04-27
**Purpose**: Bridge the current battle code (`Card`, `Player`, `RoundManager`, `Role`) to the future deterministic rule/presentation split.

## Why This Exists

The current codebase already contains the beginnings of a battle action chain:
- `Player.UpdateTime()` drives turn-time action selection
- `Player.DoUseCard()` triggers action execution
- `Card.DoCardsAction()` is the card-specific effect entry point
- `Player.EnterResponse()` models response windows
- `RoundManager.RoundState` owns the outer turn phases

What is missing is a stable payload boundary between:
1. rule validation and rule resolution
2. battle presentation and effect timing

This document defines that boundary.

## Current Code Mapping

### `Card`
File: `Assets/Scripts/Card/Card.cs`

Current responsibilities:
- card identity via `cardName` and `CardType`
- ownership via `OwnerID`
- effect entry point via `DoCardsAction(Player user, Player target)`
- response capability via `IsCardResponsible(...)`

Current gap:
- card identity is too presentation-light for template routing
- `DoCardsAction` executes directly against runtime objects with no result payload boundary

### `Player`
File: `Assets/Scripts/Character/Player.cs`

Current responsibilities:
- stores `currentCards`
- owns current role and HP
- selects card and target
- executes `UseCard()`
- manages response windows

Current gap:
- selection, validation, execution, and presentation timing are tightly coupled inside coroutines
- no explicit result object exists for downstream UI/presentation

### `RoundManager`
File: `Assets/Scripts/CardManage/RoundManager.cs`

Current responsibilities:
- owns outer round state via `GRoundState`
- sequences preparing/checking/draw/battle/discard/end

Current gap:
- round transitions are not surfaced as battle events or logs

### `Role`
File: `Assets/Scripts/Role/Role.cs`

Current responsibilities:
- role identity
- HP-based hand-size rule
- tag stack add/remove/refresh

Current gap:
- tag changes and role-side effects are not emitted as structured result deltas

## Proposed Runtime Boundary

### 1. `RuleActionRequest`
Created when a player or AI declares an action.

```text
RuleActionRequest
- requestId
- actorPlayerId
- sourceKind            # card | skill | response
- sourceId              # card id / skill id / response id
- sourceType            # Sha / Shan / Tao / custom skill family
- targetPlayerIds[]
- declaredRoundState    # Battling / Response / etc.
- isResponseAction
- inputContextTags[]    # optional authoring/use-context tags
```

### 2. `RuleActionResult`
Created after legality checks and deterministic resolution are complete.

```text
RuleActionResult
- requestId
- resolvedActionType
- actorPlayerId
- targetResults[]
  - targetPlayerId
  - hpDelta
  - cardsDrawn
  - cardsDiscarded
  - tagsAdded[]
  - tagsRemoved[]
  - wasBlocked
  - wasDodged
  - wasCriticalPresentationMoment
- battleStateChanges[]
- responseWindowOpened   # true/false
- responseCardType       # e.g. Shan
- presentationHintId
- battleLogTextKey
```

### 3. `PresentationDefinition`
Resolved from `RuleActionResult.presentationHintId` or action family fallback.

```text
PresentationDefinition
- presentationId
- actionFamily               # normal_attack / dodge / heal / signature_skill
- cameraTemplateId
- animationCueId
- vfxCueIds[]
- uiFeedbackStyleId
- authoredHitFrame
- resultOffset
- usesTimelineHeroMoment
- fallbackPresentationId
```

## Mapping From Current Code To Proposed Schema

### `Player.currSelectedCardID`
- today: index into `currentCards`
- target state: used only long enough to build `RuleActionRequest`

### `Player.TargetPlayerEnemyID`
- today: implicit target field reused across actions/responses
- target state: copied into `RuleActionRequest.targetPlayerIds`

### `Card.CardType`
- today: both identity and response routing
- target state: retained as `sourceType`, and also used for default `actionFamily`

### `Player.ResponsingCard`
- today: direct object reference used to drive response flow
- target state: represented by `responseWindowOpened` and `responseCardType` in `RuleActionResult`

### `Role.Tags`
- today: mutated in place
- target state: changes summarized in `targetResults[].tagsAdded` and `tagsRemoved`

## First Non-Breaking Adoption Steps

1. Keep current coroutine flow.
2. Add schema docs and logging first.
3. Wrap `UseCard()` and `DoCardsAction()` with request/result generation.
4. Route UI/presentation from `RuleActionResult` instead of direct runtime inspection where possible.
5. Introduce `PresentationDefinition` lookup only for the first action slice.

## First Test Targets Enabled By This Schema

- round state transitions remain deterministic
- HP-driven hand-size limit remains deterministic
- role tag add/remove/refresh remains deterministic
- card response windows open only for response-capable action types

## Risks

- If schema work is skipped, presentation code will hardcode directly against `Player`/`Card` coroutines.
- If result payloads try to carry full presentation logic, the rule/presentation boundary collapses again.

## Acceptance Criteria

- [ ] Current code responsibilities are mapped to future request/result/template structures.
- [ ] `RuleActionRequest`, `RuleActionResult`, and `PresentationDefinition` fields are defined clearly enough for future implementation.
- [ ] The document identifies non-breaking adoption steps instead of requiring a full rewrite.
- [ ] First test targets are explicitly linked to the schema.
