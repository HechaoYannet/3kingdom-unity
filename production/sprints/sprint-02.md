# Sprint 02 - 2026-05-09 to 2026-05-22

## Sprint Goal
Validate the first battle-presentation vertical slice by connecting one deterministic combat chain to a readable 3D action presentation path.

## Capacity
- Total days: 10
- Buffer (20%): 2 days
- Available: 8 days

## Tasks

### Must Have (Critical Path)
| ID | Task | Agent/Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------------|-----------|-------------|-------------------|
| VIS-01 | Validate render-pipeline route in-editor | technical art / programming | 1 | PLAN-003 | Render route decision recorded and blockers listed in editor handoff |
| VIS-02 | Build baseline battle camera and one action camera | programming / technical art | 1 | VIS-01 | Two Cinemachine camera states exist and can be blended in-scene |
| BTL-01 | Define first action result -> presentation template mapping | programming | 1 | PLAN-001 | One baseline card action has a documented payload and template mapping |
| BTL-02 | Implement first full battle action chain | programming / technical art | 2 | VIS-02, BTL-01 | One card action plays actor staging, camera emphasis, impact, and result feedback |
| BTL-03 | Implement one signature skill hero moment | programming / technical art | 2 | BTL-02 | One signature skill has a distinct presentation path with enhanced timing/camera emphasis |
| TEST-02 | Add first executable gameplay tests | programming / QA | 1 | BTL-01 | Round flow and role-rule tests exist and are runnable via Unity Test Framework |

### Should Have
| ID | Task | Agent/Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------------|-----------|-------------|-------------------|
| FX-01 | Define first effect fallback rules | technical art | 0.5 | VIS-01 | Critical effects have a documented non-premium fallback |
| QA-02 | Capture baseline slice review evidence | QA / design | 0.5 | BTL-03 | Screenshots/video plus checklist notes are stored for review |

### Nice to Have
| ID | Task | Agent/Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------------|-----------|-------------|-------------------|
| AI-01 | Trial one retrieval or content ideation workflow with project docs | production / design | 0.5 | PLAN-002 | At least one AI workflow is documented with input/output examples |

## Risks
| Risk | Probability | Impact | Mitigation |
|------|------------|--------|------------|
| Visual route stays undecided too long | High | High | VIS-01 is the first must-have, not a side task |
| First action chain is hardcoded and non-reusable | Medium | High | Require template mapping before presentation implementation |
| Test work slips behind editor work | Medium | Medium | TEST-02 remains on the critical path |

## Dependencies on External Factors
- Unity/Tuanjie editor access is required for camera/render/effect validation
- Any URP shift depends on actual material/shader audit results

## Definition of Done for this Sprint
- [ ] All Must Have tasks completed
- [ ] One full card action chain is playable in-editor
- [ ] One signature skill hero moment is playable in-editor
- [ ] At least two Cinemachine states are validated
- [ ] First gameplay tests are committed
- [ ] Review evidence exists for the slice
