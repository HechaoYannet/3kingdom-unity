# Source Directory

When editing code in this directory, keep the existing Claude workflow rules and
apply them through Codex.

## Engine Version Warning

The pinned engine/tooling may be newer than model cutoff knowledge.
Always check `Docs/engine-reference/` before using engine APIs.

## Coding Standards

- All public APIs require doc comments.
- Gameplay values must be data-driven, never hardcoded.
- Prefer dependency injection over singletons for testability.
- Every new system needs a corresponding ADR in `Docs/architecture/`.
- Commits must reference the relevant story ID or design document.

## File Routing

- Use the Unity-specialist guidance in `.claude/agents/` when work is engine-specific.
- If the task mentions a Claude role, read that agent file before editing.

## Tests

- Tests belong in `tests/`, not in `src/`.
- Every gameplay system should have unit tests for formulas and edge cases.
- For UI-facing work, capture verification evidence when practical.

