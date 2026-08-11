# THREE KINGDOM — Quick Start

## AI Workflow Entry

- 项目协作上下文：根目录 `CODELY.md`（项目总览 + AI 上下文）与 `CLAUDE.md`（工作记忆）
- 文档总索引：`Docs/README.md`
- 源码目录规范：`Assets/Scripts/CLAUDE.md`

## Project State

- **Stage**: Production — code exists, docs catching up
- **Engine**: Unity `2022.3.62t7` + Tuanjie Engine `1.8.5`
- **Rendering**: URP 14.1.0 (battle-presentation vertical slice)
- **Battle UI**: Refactored (10 phases + bugfixes completed)

## First Look Locations

1. **Code Summary**: `Docs/architecture/code-summary.md`
2. **Project Stage**: `Docs/production/project-stage-report.md`
3. **Design Docs**: `Docs/gdd/`
4. **Glossary**: `Docs/reference/glossary.md`
5. **Source Code**: `Assets/Scripts/`

## Core Systems (live)

1. **Card System** — 卡牌规则（Sha/Shan/Tao），`Assets/Scripts/Card/`, `CardManage/`
2. **Character System** — 玩家实体 + AI，`Assets/Scripts/Character/`
3. **Role System** — 武将（阵营/技能），`Assets/Scripts/Role/`
4. **UI System** — 战斗 UI，`Assets/Scripts/UI/`
5. **Audio System** — 音效管理，`Assets/Scripts/Audio/`
6. **Tools** — 工具类，`Assets/Scripts/Tools/`

> 入口链路（Build Settings）：`MainHome.scene`（空壳，未挂业务脚本）→ `BattleScene.scene`（挂载 `GamePlaying` / `Player` / `CardManager` / `BattleUIBootstrap`）。

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
| Tests | 🧪 5 NUnit tests | `Assets/Scripts/UI/Test/BattleLayoutTests.cs` |

## Next Actions

1. Run `BattleLayoutTests.cs` in Test Runner
2. Investigate the remaining missing-script warning (old prefab GUID) in Console
3. Continue reverse-documenting AssetBundle/Data/Event systems (removed dead code, GDDs still pending)
4. Expand SDF font atlas for full Chinese text support
