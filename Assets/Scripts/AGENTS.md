# Source Directory — AGENTS

When editing code in this directory, follow the project conventions in `CLAUDE.md` (root) and `CODELY.md`.

## Engine Version Warning

The pinned engine (Unity 2022.3.62t7 + Tuanjie 1.8.5) may be newer than model cutoff knowledge.
Verify API signatures against the installed package docs before using unfamiliar APIs.

## Coding Standards

- All public APIs require Chinese XML doc comments (`<summary>`).
- Gameplay values must be data-driven (JSON / ScriptableObject), never hardcoded.
- Follow existing singleton patterns (`public static T instance` in Awake) unless a new pattern is agreed.
- Every new system needs a corresponding document in `Docs/architecture/`.
- Commits must reference the relevant document or task.

## File Routing

- Game logic: `Assets/Scripts/Card/`, `Character/`, `Role/`, `CardManage/`
- UI: `Assets/Scripts/UI/` (`UIFramework/` core + `UICompnent/` HUD)
- Utilities: `Assets/Scripts/Tools/`

## Tests

- Runtime test code: `Assets/Scripts/UI/Test/` (has its own asmdef, Editor-only)
- Test documentation: `Docs/testing/`
