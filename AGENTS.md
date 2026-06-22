# THREE KINGDOM - Codex Entry

This repository keeps the original Claude workflow intact under `.claude/`.
Do not replace or rewrite that workflow unless the user explicitly asks.

## Operating Mode

- Treat `.claude/` as the canonical workflow library for this project.
- Treat this `AGENTS.md` file set as the Codex compatibility layer.
- Preserve rollback safety: new Codex-facing files should be additive.
- When a workflow detail is unclear, read the matching document in `.claude/docs/`
  or the matching skill in `.claude/skills/`.

## Automatic Intake Protocol

Codex should do this automatically for every task in this repository.
The user does not need to repeat these instructions each time.

### Baseline Reads

Read these at the start of any non-trivial task:

1. `AGENTS.md`
2. `PROJECT-QUICKSTART.md`

### Contextual Reads

Read additional files only when relevant to the task:

- Code changes in `src/`: `src/AGENTS.md`
- Code changes in `Assets/Scripts/`: `Assets/Scripts/AGENTS.md`
- Design or GDD work: `design/AGENTS.md`
- Workflow or phase questions: `Docs/CODEX-WORKFLOW.md`
- Coding constraints: `.claude/docs/coding-standards.md`
- Coordination or multi-role workflow: `.claude/docs/coordination-rules.md`
- Claude command equivalence: matching `.claude/skills/<skill>/SKILL.md`
- Claude role equivalence: matching `.claude/agents/<role>.md`

### Execution Rule

- Do not wait for the user to explicitly say "read the docs" in normal tasks.
- Before substantial work, gather only the minimum relevant context from the files above.
- Avoid bulk-reading the entire `.claude/` tree unless the task actually requires it.

## First Read

1. `PROJECT-QUICKSTART.md`
2. `Docs/CODEX-WORKFLOW.md`
3. `Docs/architecture/code-summary.md`
4. `production/project-stage-report.md`

## Project State

- Stage: Production
- Engine: Unity 2022 LTS + Tuanjie Engine 1.5.3
- Focus: Reverse-documenting existing systems while keeping production moving

## Core Rules

- Keep existing `.claude/` files as-is unless the task is explicitly about them.
- Use `Docs/engine-reference/` before relying on engine APIs.
- Keep gameplay values data-driven, not hardcoded.
- Prefer testable code paths and add verification with code changes.
- Do not commit or rewrite workflow files unless asked.

## Workflow Source Of Truth

- Studio workflow guide: `Docs/WORKFLOW-GUIDE.md`
- Claude quick start: `.claude/docs/quick-start.md`
- Coordination rules: `.claude/docs/coordination-rules.md`
- Coding standards: `.claude/docs/coding-standards.md`
- Skill catalog: `.claude/docs/skills-reference.md`
- Agent roster: `.claude/docs/agent-roster.md`

## How To Use Codex Here

- Use `Docs/CODEX-WORKFLOW.md` for the Codex-side mapping.
- If the user refers to a Claude slash command, inspect the matching
  `.claude/skills/<skill>/SKILL.md` and execute the intent manually.
- If the user refers to a Claude agent role, use the matching file in
  `.claude/agents/` as the role contract.
