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

### `CardData`
File: `Assets/Scripts/Card/CardData.cs`

Current responsibilities:
- carries `templateID`
- carries card rarity and lightweight card metadata

Current opportunity:
- `templateID` should become the stable presentation-facing definition key
- `rarity` can directly drive presentation style variants

Current gap:
- gameplay definition data is not yet rich enough to stand in for full rule-domain card definitions

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

### `CardManager`
File: `Assets/Scripts/CardManage/CardManager.cs`

Current responsibilities:
- owns deck, discard, shuffle, dealing, and card zone movement

Current opportunity:
- card movement should be expressed through `RuleActionResult.stateChanges`

Current gap:
- discard/deck/hand transitions are not yet modeled as explicit state-change records
- current discard handling should be audited because removal and discard semantics are not clearly separated

### `Role`
File: `Assets/Scripts/Role/Role.cs`

Current responsibilities:
- role identity
- HP-based hand-size rule
- tag stack add/remove/refresh

Current gap:
- tag changes and role-side effects are not emitted as structured result deltas

### Presentation Consumers
Files:
- `Assets/Scripts/UI/UIFramework/UIManager.cs`
- `Assets/Scripts/UI/UIFramework/ScreenSpaceUIManager.cs`
- `Assets/Scripts/UI/UIFramework/WorldSpaceUIManager.cs`
- `Assets/Scripts/UI/UIFramework/CardEffectsManager.cs`
- `Assets/Scripts/UI/UICompnent/DamageTextController.cs`
- `Assets/Scripts/UI/UICompnent/HealTextController.cs`
- `Assets/Scripts/UI/UICompnent/TurnIndicator.cs`

Current opportunity:
- these are already strong downstream consumers for `PresentationDefinition`
- they should render authored payloads, not infer gameplay directly from scene state

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

### 4. Stable Identity Types
These should replace magic IDs and list-index assumptions over time.

```text
ActorId
CardInstanceId
CardDefinitionId
ZoneId
ActionRequestId
ActionResultId
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

### `CardData.templateID`
- today: mostly content metadata
- target state: stable `CardDefinitionId` and presentation lookup key

### `Player.ResponsingCard`
- today: direct object reference used to drive response flow
- target state: represented by `responseWindowOpened` and `responseCardType` in `RuleActionResult`

### `Role.Tags`
- today: mutated in place
- target state: changes summarized in `targetResults[].tagsAdded` and `tagsRemoved`

### `CardManager` zone movement
- today: handled through manager methods and owner changes
- target state: represented explicitly in `battleStateChanges[]` using stable `cardInstanceId` and `zoneId`

## First Non-Breaking Adoption Steps

1. Keep current coroutine flow.
2. Add schema docs and logging first.
3. Wrap `UseCard()` and `DoCardsAction()` with request/result generation.
4. Route UI/presentation from `RuleActionResult` instead of direct runtime inspection where possible.
5. Introduce `PresentationDefinition` lookup only for the first action slice.
6. Replace magic owner IDs (`0` deck, `1` discard, `2+` players) with explicit zone/actor identity gradually, not in one rewrite.

## First Test Targets Enabled By This Schema

- round state transitions remain deterministic
- HP-driven hand-size limit remains deterministic
- role tag add/remove/refresh remains deterministic
- card response windows open only for response-capable action types

## Worked Example: `Sha`

### Request

```text
RuleActionRequest
- requestId: req_001
- actorPlayerId: actor_player_01
- sourceKind: card
- sourceId: card_inst_014
- sourceType: Sha
- targetPlayerIds: [actor_enemy_01]
- declaredRoundState: Battling
- isResponseAction: false
```

### Result

```text
RuleActionResult
- requestId: req_001
- resolvedActionType: normal_attack
- actorPlayerId: actor_player_01
- targetResults:
  - targetPlayerId: actor_enemy_01
  - hpDelta: -1
  - cardsDrawn: 0
  - cardsDiscarded: 0
  - tagsAdded: []
  - tagsRemoved: []
  - wasBlocked: false
  - wasDodged: false
- battleStateChanges:
  - move card_inst_014 from hand_zone_actor_player_01 to discard_zone
- responseWindowOpened: true
- responseCardType: Shan
- presentationHintId: normal_attack_sha
```

### Presentation

```text
PresentationDefinition
- presentationId: normal_attack_sha
- actionFamily: normal_attack
- cameraTemplateId: cam_attack_standard
- animationCueId: anim_attack_basic
- vfxCueIds: [vfx_card_sha_cast, vfx_hit_slash]
- uiFeedbackStyleId: ui_damage_standard
- authoredHitFrame: 18
- resultOffset: 0
- usesTimelineHeroMoment: false
- fallbackPresentationId: normal_attack_generic
```

## Risks

- If schema work is skipped, presentation code will hardcode directly against `Player`/`Card` coroutines.
- If result payloads try to carry full presentation logic, the rule/presentation boundary collapses again.
- If stable IDs are skipped, replay/network/UI decoupling work will stay fragile.

## Acceptance Criteria

- [ ] Current code responsibilities are mapped to future request/result/template structures.
- [ ] `RuleActionRequest`, `RuleActionResult`, and `PresentationDefinition` fields are defined clearly enough for future implementation.
- [ ] The document identifies non-breaking adoption steps instead of requiring a full rewrite.
- [ ] First test targets are explicitly linked to the schema.
