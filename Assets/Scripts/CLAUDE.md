# Source Directory — Coding Standards

When writing or editing game code in this directory, follow these standards.

## Engine Version Warning

The pinned engine may be newer than model cutoff knowledge.
**Verify API signatures against the installed package docs before using them.**
Do not guess at post-cutoff API signatures — look them up first.

## Coding Standards

- All public APIs require Chinese XML doc comments
- Gameplay values must be **data-driven** (JSON / ScriptableObject), never hardcoded
- Follow the existing singleton convention (`public static T instance` in Awake)
- Every new system needs a corresponding document in `Docs/architecture/`
- Commits must reference the relevant design document or task

## File Routing

- Card rules: `Assets/Scripts/Card/`, `CardManage/`
- Player / AI: `Assets/Scripts/Character/`
- Roles: `Assets/Scripts/Role/`
- Battle UI: `Assets/Scripts/UI/`
- Utilities: `Assets/Scripts/Tools/`

## Tests

Tests live in `Assets/Scripts/UI/Test/` (Editor-only asmdef) and are documented in `Docs/testing/`.
Every gameplay system should have unit tests covering its formulas and edge cases.
For UI changes, verify with screenshots in the Unity editor.
