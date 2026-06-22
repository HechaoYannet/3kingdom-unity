# Sprint 01 - 2026-04-27 to 2026-05-08

## Sprint Goal
Turn the current brownfield repository into a workflow-ready production workspace with indexed design docs, minimum architecture traceability, and runnable testing scaffolding.

## Capacity
- Total days: 10
- Buffer (20%): 2 days
- Available: 8 days

## Tasks

### Must Have (Critical Path)
| ID | Task | Agent/Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------------|-----------|-------------|-------------------|
| DOC-01 | Bootstrap and maintain `systems-index.md` | design / production | 0.5 | None | Systems index exists, uses valid status values, and lists current major systems |
| DOC-02 | Reverse-document Asset Bundle Framework | design / programming | 1.5 | DOC-01 | `design/gdd/asset-bundle-framework.md` exists with 8 required sections |
| DOC-03 | Reverse-document Data Management and Event System | design / programming | 1.5 | DOC-01 | `design/gdd/data-management.md` and `design/gdd/event-system.md` exist with 8 required sections |
| ARCH-01 | Create ADRs for Role and UI system ownership | architecture | 2 | DOC-01 | Two ADRs exist and link back to the current GDD set |
| TEST-01 | Add first executable EditMode tests for round flow and hand-size rules | programming / QA | 1.5 | DOC-03, ARCH-01 | At least 3 EditMode tests exist and can be targeted by Unity Test Framework |

### Should Have
| ID | Task | Agent/Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------------|-----------|-------------|-------------------|
| QA-01 | Turn scaffold QA notes into a sprint-level QA plan update | QA | 0.5 | TEST-01 | QA plan lists concrete verification paths for sprint work |
| ARCH-02 | Bootstrap master architecture document outline | architecture | 1 | ARCH-01 | `Docs/architecture/architecture.md` exists with top-level section skeleton |

### Nice to Have
| ID | Task | Agent/Owner | Est. Days | Dependencies | Acceptance Criteria |
|----|------|-------------|-----------|-------------|-------------------|
| PROD-01 | Create first epic/story tree from approved docs | production | 1 | DOC-02, DOC-03, ARCH-01 | At least one epic is defined for the next sprint |

## Carryover from Previous Sprint
| Task | Reason | New Estimate |
|------|--------|-------------|
| None | Repository had no formal sprint plan before this bootstrap | - |

## Risks
| Risk | Probability | Impact | Mitigation |
|------|------------|--------|------------|
| Reverse documentation exposes contradictions between code and intended design | High | High | Keep GDDs `In Review` until validated |
| Runtime code remains hard to test due to singleton access | Medium | High | Start with isolated formula/state tests, then add seams incrementally |
| Sprint turns into documentation-only work with no verification | Medium | High | Make TEST-01 part of the critical path |

## Dependencies on External Factors
- Unity CI requires a valid `UNITY_LICENSE` secret before GitHub Actions can run successfully
- Design approval is needed before moving current GDDs to `Approved`

## Definition of Done for this Sprint
- [ ] All Must Have tasks completed
- [ ] All tasks pass acceptance criteria
- [ ] QA plan exists (`production/qa/qa-plan-sprint-01.md`)
- [ ] All Logic/Integration stories have passing unit/integration tests
- [ ] Smoke check passed (`tests/smoke/critical-paths.md` updated and reviewed)
- [ ] Design documents updated for any deviations
- [ ] Code reviewed and merged

> Scope check: this sprint is intentionally infrastructure-heavy. Do not allow unrelated feature work to displace the documentation and test-critical tasks above.
