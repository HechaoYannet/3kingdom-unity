# THREE KINGDOM - Quick Start

## AI Workflow Entry
- Claude Code: keep using `CLAUDE.md` and `.claude/`
- Codex: start with `AGENTS.md` and `Docs/CODEX-WORKFLOW.md`

## Project State
- **Stage**: Production (code exists, docs and tests are still catching up)
- **Engine**: Unity `2022.3.62t7` + Tuanjie Engine `1.8.5`
- **Code**: 54 C# files, 3 scenes
- **Rendering Route**: URP is active for the current battle-presentation vertical slice
- **Docs**: Reverse-documentation, planning, backlog, schema, and QA artifacts now exist

## First Look Locations
1. **Code Summary**: `Docs/architecture/code-summary.md`
2. **Project Stage**: `production/project-stage-report.md`
3. **Design Docs**: `design/gdd/`
4. **Architecture**: `Docs/architecture/`
5. **Production Tracking**: `production/`
6. **Source Code**: `Assets/Scripts/`

## 9 Core Systems
1. **Asset Bundle Framework** - Asset loading
2. **Card System** - Card game mechanics
3. **Character System** - Player/Enemy AI
4. **Role System** - Special abilities
5. **UI System** - Interface management
6. **Audio System** - Sound management
7. **Data Management** - Save/load data
8. **Event System** - Game events
9. **Tools** - Utilities

## Current Work
- Reverse-documenting implemented systems and ownership boundaries
- Building the first battle-presentation vertical slice
- Keeping deterministic rule logic separate from presentation logic
- Validating URP, Cinemachine, Timeline, Shader Graph, and VFX Graph usage
- Preparing the first gameplay tests

## Next Actions
1. Check `production/project-stage-report.md` for the current gaps
2. Review `production/qa/editor-validation-report-2026-04-30.md` for validated editor facts
3. Continue reverse-documenting `AssetBundleFramework`, `Data`, and `Event`
4. Move one card action into the first rule-result to presentation chain
5. Add the first Unity gameplay tests around round flow and role rules

## Quick Commands
- `/project-stage-detect` - Full analysis
- `/reverse-document design Assets/Scripts/[system]` - Document code
- `/architecture-decision` - Create ADR
- `/sprint-plan` - Update production planning
