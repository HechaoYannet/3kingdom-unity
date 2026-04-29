# Codex Workflow Compatibility Guide

This repository now supports both workflows:

- Claude workflow: unchanged, canonical assets remain under `.claude/`
- Codex workflow: additive compatibility layer through `AGENTS.md`

The goal is migration without lock-in. If you need to fall back, the original
Claude setup is still available.

## Compatibility Model

Codex does not use Claude slash commands directly. In this repository, the
replacement rule is:

1. Keep using the same project artifacts
2. Keep using the same phase model
3. Translate each Claude command into an explicit Codex task
4. Read the matching `.claude` skill or agent file when the old workflow is referenced

## Session Start In Codex

At the start of a Codex session, read:

1. `AGENTS.md`
2. `PROJECT-QUICKSTART.md`
3. Any nearer `AGENTS.md` in the directory you will edit
4. `.claude/docs/coding-standards.md` or `.claude/docs/coordination-rules.md` when needed

## Default Behavior

In this repository, the document-reading step is implicit.
You should not need to ask Codex each time to read workflow files first.

Expected behavior:

1. Codex reads the minimum relevant workflow files automatically
2. Codex maps Claude-native commands or roles to the matching `.claude` files when referenced or implied
3. Codex proceeds with the task after context intake, without requiring a separate "read docs first" instruction

Only trivial requests should skip this, such as a one-line factual answer that does not touch repo workflow.

## Command Mapping

Use these prompt forms in Codex instead of Claude slash commands:

| Claude habit | Codex equivalent |
|---|---|
| `/project-stage-detect` | "Audit the current project stage using `.claude/skills/project-stage-detect/SKILL.md` and write or update the report." |
| `/reverse-document <path>` | "Reverse-document `<path>` using `.claude/skills/reverse-document/SKILL.md`." |
| `/sprint-plan` | "Create or update the sprint plan following `.claude/skills/sprint-plan/SKILL.md`." |
| `/design-review <file>` | "Review `<file>` against `.claude/skills/design-review/SKILL.md`." |
| `/review-all-gdds` | "Run a cross-GDD review using `.claude/skills/review-all-gdds/SKILL.md`." |
| `/architecture-decision` | "Create or retrofit an ADR using `.claude/skills/architecture-decision/SKILL.md`." |
| `/create-control-manifest` | "Generate the control manifest from accepted ADRs using `.claude/skills/create-control-manifest/SKILL.md`." |
| `/story-readiness` | "Validate this story using `.claude/skills/story-readiness/SKILL.md`." |
| `/story-done` | "Run end-of-story completion review using `.claude/skills/story-done/SKILL.md`." |
| `/gate-check <phase>` | "Perform the `<phase>` gate using `.claude/skills/gate-check/SKILL.md`." |
| `/help` | "Read the current project state and tell me the single best next step using `.claude/skills/help/SKILL.md`." |

## Agent Mapping

If you used Claude agent names before, keep the same mental model:

- Role definitions stay in `.claude/agents/*.md`
- In Codex, ask for that role explicitly in the prompt
- Example: "Act using the constraints from `.claude/agents/unity-specialist.md` and implement ..."

## What Stays Unchanged

- `.claude/settings.json`
- `.claude/hooks/`
- `.claude/skills/`
- `.claude/agents/`
- All existing design, architecture, sprint, and production artifacts

Nothing in the Codex layer should block a return to Claude Code.

## Recommended Migration Pattern

1. Use Codex for implementation, review, and repo-local workflow execution.
2. Keep `.claude/skills/` as the workflow playbook.
3. Keep writing outputs to the same project directories as before.
4. Only migrate a Claude-specific asset if there is a concrete Codex benefit.

## Practical Examples

- Old: `/reverse-document Assets/Scripts/CardSystem`
- New: "Read `.claude/skills/reverse-document/SKILL.md`, inspect `Assets/Scripts/CardSystem`, and generate the missing design documentation."

- Old: `/team-ui`
- New: "Coordinate this UI feature using the workflow intent from `.claude/skills/team-ui/SKILL.md`; use the relevant role docs in `.claude/agents/` when making design and implementation decisions."

- Old: `/code-review`
- New: "Review these changes using `.claude/skills/code-review/SKILL.md` and report findings first."

## Rollback

Rollback is trivial because the Claude workflow was not removed.
If Codex-specific files are not wanted later, remove:

- `AGENTS.md`
- `src/AGENTS.md`
- `Assets/Scripts/AGENTS.md`
- `design/AGENTS.md`
- `Docs/CODEX-WORKFLOW.md`

All original Claude assets remain untouched.
