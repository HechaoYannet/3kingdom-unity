# THREE KINGDOM - Quick Start

## Project State
- **Stage**: Production (code exists, needs docs/tests)
- **Engine**: Unity 2022 LTS + Tuanjie Engine 1.5.3
- **Code**: 54 C# files, 3 scenes
- **Docs**: Minimal (reverse-documenting in progress)

## First Look Locations
1. **Code Summary**: `Docs/architecture/code-summary.md` (system overview)
2. **Design Docs**: `design/gdd/` (card-system.md, character-system.md)
3. **Architecture**: `Docs/architecture/` (ADRs for each system)
4. **Source Code**: `Assets/Scripts/` (9 systems)

## 9 Core Systems
1. **Asset Bundle Framework** - Asset loading
2. **Card System** - Card game mechanics  
3. **Character System** - Player/Enemy AI
4. **Role System** - Special abilities (HuangGai, etc.)
5. **UI System** - Interface management
6. **Audio System** - Sound management
7. **Data Management** - Save/load data
8. **Event System** - Game events
9. **Tools** - Utilities

## Current Work
- Reverse-documenting existing code
- Creating missing design docs
- Setting up tests
- Establishing production planning

## Next Actions
1. Check `production/project-stage-report.md` for gaps
2. Run `/reverse-document` for undocumented systems
3. Run `/test-setup` for Unity tests
4. Run `/sprint-plan` for production tracking

## Quick Commands
- `/project-stage-detect` - Full analysis
- `/reverse-document design Assets/Scripts/[system]` - Document code
- `/architecture-decision` - Create ADR
- `/sprint-plan` - Start production planning