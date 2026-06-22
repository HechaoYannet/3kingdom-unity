# Design Directory

This directory keeps the original design workflow. Codex should follow it
without replacing the Claude-authored structure.

## GDD Rules

Every GDD in `design/gdd/` must include these 8 sections in order:

1. Overview
2. Player Fantasy
3. Detailed Rules
4. Formulas
5. Edge Cases
6. Dependencies
7. Tuning Knobs
8. Acceptance Criteria

## Naming And Flow

- File naming: `[system-slug].md`
- Update `design/gdd/systems-index.md` when adding a new GDD.
- Design order: Foundation -> Core -> Feature -> Presentation -> Polish

## Validation

- Use `.claude/skills/design-review/SKILL.md` as the review contract for a single GDD.
- Use `.claude/skills/review-all-gdds/SKILL.md` for cross-GDD consistency work.
- Use `.claude/skills/quick-design/SKILL.md` for minor specs instead of full GDDs.

