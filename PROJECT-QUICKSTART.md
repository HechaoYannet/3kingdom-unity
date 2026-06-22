# THREE KINGDOM - Quick Start

## AI Workflow Entry
- Claude Code: use `CLAUDE.md` (working memory) + `CODELY.md` (project overview)
- Codex: start with `AGENTS.md` and `Docs/CODEX-WORKFLOW.md`

## Project State
- **Stage**: Production — code exists, docs continue catching up
- **Engine**: Unity `2022.3.62t7` + Tuanjie Engine `1.8.5`
- **Code**: 58+ C# files, 3 scenes
- **Rendering**: URP 14.1.0 active for battle-presentation vertical slice
- **Battle UI**: Fully refactored (10 phases + 4 post-launch bugfixes completed)

## First Look Locations
1. **Code Summary**: `Docs/architecture/code-summary.md`
2. **Project Stage**: `production/project-stage-report.md`
3. **Design Docs**: `design/gdd/`
4. **Working Memory**: `CLAUDE.md`
5. **Memory Files**: `memory/glossary.md`, `memory/projects/`
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

## Battle UI Status (Latest)
| Component | Status | Notes |
|-----------|--------|-------|
| Config (BattleUIConfig) | ✅ Data-driven | ScriptableObject |
| Theme (BattleTheme) | ✅ Visual theming | ScriptableObject |
| Object pools | ✅ Card + feedback | Replaces Instantiate/Destroy |
| Hand layout | ✅ Adaptive fan | Stretch-anchored, config-driven spacing |
| Card interaction | ✅ Drag/hover/click | DOTween + parallax |
| HUD | ✅ Phase + Player/Enemy + Result | True-size, not overflowing |
| SafeArea | ✅ On Canvas(Screen) | Notch/cutout safe |
| Font | ✅ SourceHanSansSC SDF | 3500 CJK chars, 0 TMP warnings |
| Camera | ✅ UICamera Overlay | In Main Camera stack |
| Animations | ✅ Full DOTween | hover/drag/play/HP/transitions |
| Audio | ✅ Battle SFX hooks | AudioManage.BattleSfx |
| Tests | 📋 Scaffolded | BattleLayoutTests.cs |

## Current Work ✅ (Complete)
Battle UI full refactor is complete. Three post-refactor defects resolved:

1. **Drag interaction** — DOTween kill on drag start; layoutDirty on drag end
2. **Hand spread** — Prefab stretch anchors + config tuning
3. **HUD overflow** — sizeDelta removed; Canvas scale fixed; SafeAreaFitter added
4. **TMP font** — All 10 UI scripts now use SourceHanSansSC SDF

## Next Actions
1. Run BattleLayoutTests.cs in Test Runner
2. Complete manual testing checklist
3. Investigate 1 harmless "missing script" warning
4. Continue reverse-documenting AssetBundleFramework, Data, Event systems
5. Expand SDF font atlas for full Chinese text support
