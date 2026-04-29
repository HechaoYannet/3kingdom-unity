# Battle Presentation Vertical Slice Backlog

**Status**: Active
**Updated**: 2026-04-30
**Primary Milestone**: `production/milestones/battle-presentation-vertical-slice.md`

## Critical Path

1. Freeze render route for the slice
2. Validate camera toolchain in-editor
3. Hook one full action chain from rules to presentation
4. Add tests around the deterministic side of the chain
5. Capture and review the first slice

## Backlog Items

| ID | Type | Title | Owner | Repo-Local | Editor-Only | Depends On | Status |
|----|------|-------|-------|------------|-------------|------------|--------|
| PLAN-001 | Planning | Battle-presentation-first direction | Codex | Yes | No | None | Done |
| PLAN-002 | Planning | 6-week roadmap and AI pipeline | Codex | Yes | No | PLAN-001 | Done |
| PLAN-003 | Planning | Editor-required handoff list | Codex | Yes | No | PLAN-001 | Done |
| VIS-01 | Validation | Render route decision | User + tech art | No | Yes | PLAN-003 | Done - URP |
| VIS-02 | Validation | Material/shader blocker audit | User + tech art | No | Yes | VIS-01 | Done - no obvious blockers |
| VIS-03 | Validation | Cinemachine/Timeline package validation | User + programming | No | Yes | VIS-01 | Done - installed and usable |
| BTL-01 | Runtime | Action result payload schema | Programming | Yes | No | PLAN-001 | Done |
| BTL-02 | Runtime | Presentation template schema | Programming | Yes | No | BTL-01 | Done |
| BTL-03 | Runtime | First full action chain | Programming | Mixed | Mixed | BTL-01, BTL-02, VIS-03 | Blocked by editor validation |
| BTL-04 | Runtime | Signature skill hero moment | Programming | Mixed | Mixed | BTL-03 | Blocked by editor validation |
| TEST-02 | QA | First gameplay tests | Programming | Yes | No | BTL-01 | Ready |
| FX-01 | Tech Art | Fallback effect policy | Tech art | Yes | Yes | VIS-01 | Ready |
| QA-02 | Review | Slice review evidence pack | QA / design | No | Yes | BTL-04 | Partial - editor check executed, evidence not attached |

## Repo-Local Tasks I Can Continue Immediately

- [x] Define `RuleActionRequest` and `RuleActionResult` schema docs
- [x] Define `PresentationDefinition` schema docs
- [ ] Reverse-document Asset Bundle, Data, and Event systems
- [ ] Add first deterministic gameplay tests if code seams allow it without editor work

## Editor-Required Tasks For User

- [x] Run `production/qa/editor-validation-checklist-battle-presentation.md`
- [x] Backfill the final written render-route conclusion
- [x] Backfill written shader/material blocker conclusions
- [x] Backfill written package-state conclusions
- [ ] Backfill written baseline scene observations
- [ ] Backfill written camera-validation conclusions

## Resume Notes

- If resuming in a new session, start by reading:
  - `production/session-state/active.md`
  - `production/session-state/development-checklist.md`
  - `production/session-state/editor-required-handoff.md`
  - `Docs/planning/6-week-battle-presentation-roadmap.md`
  - this backlog file
