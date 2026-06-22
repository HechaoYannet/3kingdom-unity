# Smoke Test: Critical Paths

**Purpose**: Verify the repository's critical gameplay entry points and workflow scaffolding quickly before QA hand-off.
**Run via**: Manual review or future `/smoke-check`
**Update**: Expand this file as executable tests and feature stories are added.

## Core Stability

1. Project opens with the expected Unity/Tuanjie version constraints
2. Login scene and battle scene assets are present
3. No required workflow document under `.claude/` was modified during setup work

## Core Mechanic

4. Round manager, player/role scripts, and card scripts all remain present in `Assets/Scripts/`
5. Existing reverse-documented GDDs map cleanly to the implemented script areas

## Data Integrity

6. `production/stage.txt`, `production/sprint-status.yaml`, and `design/gdd/systems-index.md` are present and readable
7. Architecture traceability file exists and references current ADRs

## Performance / Pipeline

8. `.github/workflows/tests.yml` exists
9. `tests/` scaffold is present with EditMode and PlayMode folders
