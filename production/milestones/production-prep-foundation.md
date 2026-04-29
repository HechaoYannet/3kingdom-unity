# Milestone: Production Prep Foundation

**Window**: 2026-04-27 to 2026-05-15
**Stage**: Production
**Owner**: Repository-wide documentation and infrastructure hardening

## Goal

Make the existing Unity/Tuanjie project safe for repeatable design, architecture, sprint, and testing workflows without rewriting the canonical `.claude/` process.

## Exit Criteria

- [ ] `design/gdd/systems-index.md` is present and kept in sync with current GDDs
- [ ] Event, Data, and Asset Bundle systems have first-pass GDDs
- [ ] Role and UI systems have ADR coverage
- [ ] Test scaffold is present and at least three core EditMode tests exist
- [ ] A story-driven sprint can be generated from approved documentation

## Scope

- Brownfield documentation hardening
- Production artifact bootstrap
- Test infrastructure bootstrap
- Initial traceability between GDDs and ADRs

## Out of Scope

- Major gameplay redesign
- Networking rollout
- Large runtime refactors
- Release-readiness work

## Risks

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| Reverse-documented behavior differs from intended design | High | High | Run design review on each GDD before deriving stories |
| Test scaffold exists but remains unused | Medium | High | Require first real EditMode tests in this milestone |
| Missing architecture ownership causes rework later | High | Medium | Prioritize Role/UI/Data/Event ADRs before new feature implementation |

## Success Signal

The next sprint after this milestone should be able to focus on feature or stabilization stories instead of repository setup.
