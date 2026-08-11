# Milestone: Battle Presentation Vertical Slice

**Window**: 2026-05-09 to 2026-06-07
**Stage**: Production
**Owner**: Battle presentation strike team

## Goal

Prove that the project's new differentiator is real: a deterministic Three Kingdoms card-and-turn rules core expressed through a modern 3D battle presentation with camera language, action staging, and readable spectacle.

## Exit Criteria

- [ ] One formal battle scene is playable in-editor
- [ ] Two battle-ready characters/roles can complete a minimal duel loop
- [ ] Three baseline cards can trigger presentation-aware combat actions
- [ ] One signature skill can trigger a hero-moment presentation path
- [ ] One standard-action template and one hero-moment template are reusable
- [ ] First executable gameplay tests exist for round flow and role rules
- [ ] Fallback behavior is defined for lower-end visual tiers

## Scope

- 3D battle vertical slice only
- presentation pipeline validation
- deterministic rule/presentation boundary enforcement
- AI-assisted planning and production support

## Out of Scope

- full roster expansion
- networking
- complete progression/meta loop
- polished release UI shell
- project-wide rendering migration beyond the validated slice

## Major Deliverables

1. battle-presentation system GDD
2. battle-presentation-first ADR
3. battle vertical-slice backlog and sprint plan
4. editor-validated render/camera setup
5. first playable slice capture and review pack

## Risks

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| URP migration cost is higher than expected | Medium | High | Validate on a single battle slice before broad adoption |
| Spectacle harms readability | High | High | Keep baseline camera, fallback VFX, and result feedback constraints explicit |
| AI speeds ideation but not integration | Medium | Medium | Require every AI-generated output to land in repo files with an owner |
| First slice over-expands in content before the template path is stable | High | High | Keep slice to 2 characters, 3 cards, 1 signature skill |

## Success Signal

An external viewer can watch a short battle clip and immediately understand that the game is targeting modern 3D anime-style tactical presentation rather than a flat table-card prototype.
