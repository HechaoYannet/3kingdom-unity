# Battle Runtime Architecture

## Status

- Date: 2026-04-30
- Status: Accepted
- Scope: `Assets/Scripts/CardManage/`, `Assets/Scripts/Character/`, `Assets/Scripts/Card/`

## Decision

The playable battle loop is owned by `RoundManager` as a six-state runtime driver:

1. `Preparing`
2. `Checking`
3. `GettingCard`
4. `Battling`
5. `RefusingCard`
6. `Ending`

`PlayerManager` guarantees there are always two combatants at runtime:

- the scene player (`TEMP_PLAYER`)
- one fallback `EnemyAI` created at runtime if the scene does not already contain an opponent

Card rules stay authoritative inside `Card` subclasses:

- `Sha` requests a response window and deals damage only when `ResponseTrigger()` resolves
- `Shan` cancels `Sha` by being a valid response card
- `Tao` heals immediately on use

`EnemyAI` uses a deterministic priority policy instead of presentation-driven behavior:

1. Heal with `Tao` when wounded
2. Attack with `Sha` when possible
3. Respond with `Shan` to `Sha`
4. Otherwise pass

## Rationale

The previous implementation had three blocking runtime issues:

- `EnemyAI` was created with `new EnemyAI(true)`, which is invalid for `MonoBehaviour`
- battle state progression existed but several phases were effectively inert
- `Shan` and `Tao` resources existed without corresponding rule implementations

This architecture restores a complete rules-first battle path without depending on unfinished UI or presentation orchestration.

## Consequences

Positive:

- the scene can now progress from battle start to win/lose resolution
- AI can serve as a temporary functional opponent
- card resolution, response, discard, and reshuffle behavior now form a complete loop

Tradeoffs:

- AI is intentionally simple and not difficulty-tuned
- role skills are still scaffolding and do not yet influence turn choices
- current player interaction is still minimal and uses drag-to-play plus existing turn ownership

## Follow-up

- Add executable EditMode tests for `BasicBattleAI` and card resolution
- Move deck composition into dedicated battle config assets
- Emit structured rule-result payloads for the battle presentation pipeline
