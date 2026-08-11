# THREE KINGDOM — Unity Card Game

## Project Overview

A **Three Kingdoms-themed card battle game** (三国杀-style) built on Unity 2022.3 LTS + Tuanjie Engine 1.8.5. Players take on character roles from the Three Kingdoms era and battle using a deterministic turn-based card system with initiative/response mechanics.

**初心**：让每一次出牌都像一场电影级对决。策略化为电影级动作。
**使命**：确定性的规则核心 + 现代化的 3D 演出 + 剧本驱动的剧情战斗，区别于市面普通卡牌游戏。

> **五大支柱** (`Docs/gdd/game-pillars.md`)：规则权威不可动摇 → 每张牌都是演出 → 一目了然 → 双端可降级 → 剧本模式（战斗即叙事）
> 所有设计决策必须对齐支柱。冲突时按优先级裁决。

- **Engine**: Unity `2022.3.62t7` (Tuanjie `1.8.5`)
- **Render Pipeline**: URP 14.1.0
- **Project Stage**: Production — code exists, battle-presentation vertical slice complete
- **Active Scene**: `Assets/Scenes/MainHome.scene`（Home 开发中，BattleScene 为战斗入口）
- **Game Concept**: `Docs/gdd/game-concept.md`
- **Game Pillars**: `Docs/gdd/game-pillars.md`

## Key Scenes

| Scene | Path | Purpose |
|-------|------|---------|
| MainHome | `Assets/Scenes/MainHome.scene` | 主界面（Build Settings 第 0 位，挂载 `HomeBootstrap`，开发中） |
| BattleScene | `Assets/Scenes/BattleScene.scene` | 战斗主玩法（挂载 `GamePlaying`/`Player`/`CardManager`/`BattleUIBootstrap`） |
| LoginScene | `Assets/Scenes/LoginScene.unity` | 预留（未在 Build Settings 启用，无业务脚本挂载） |

> **Note**: 场景同时使用 `.unity`（Unity）与 `.scene`（Tuanjie）扩展名，搜索场景时两种都要查。

## Core Systems (live)

| # | System | Location | Purpose |
|---|--------|----------|---------|
| 1 | Card System | `Assets/Scripts/Card/`, `CardManage/` | 卡牌定义（Sha/Shan/Tao）、CardManager、RoundManager 回合流 |
| 2 | Character System | `Assets/Scripts/Character/` | Player 实体、EnemyAI、BasicBattleAI、PlayerManager |
| 3 | Role System | `Assets/Scripts/Role/` | 武将（阵营/HP/技能/tags），`Roles/HuangGai.cs` |
| 4 | UI System | `Assets/Scripts/UI/` | UIFramework（战斗核心）+ UICompnent（HUD 组件） |
| 5 | Home System | `Assets/Scripts/Home/` | 主界面引导（HomeBootstrap/HomePresenter/HomeUIConfig） |
| 6 | Event System | `Assets/Scripts/Event/` | 事件总线 `EventManager`（`OnCardPlayed` 静态事件 → `card.DoCardsAction`） |
| 7 | Audio System | `Assets/Scripts/Audio/` | AudioManage（BGM + BattleSfx 钩子） |
| 8 | Tools | `Assets/Scripts/Tools/` | MonoSingleton、DictionaryHelper、OperationFile 等工具 |

> `Login/StartGame.cs` 保留（历史遗留，暂未挂载任何场景）。2026-08-12 清理时曾误删 Home/Login（场景 GUID 为 base64 编码导致反查漏判）与 Event 系统，均已从 git 历史恢复——**Home 是活跃开发中的场景，Event 是用户指定的活系统，切勿再删**。

## Core Gameplay Flow

1. `GamePlaying` sets `GameState.Start`
2. `RoundManager.EnterGamer()` initializes players, cards, and UI
3. Each round: **Preparing → Checking → GettingCard → Battling → RefusingCard → Ending**
4. Card play triggers response window (e.g., `Sha` → `Shan` response)
5. `HandlePlayerDefeated` checks win/loss at 0 HP

### Card Types (Implemented)

| Card | Type | Initiative | Response | Effect |
|------|------|------------|----------|--------|
| Sha (杀) | Attack | Yes | Triggers response | 1 damage |
| Shan (闪) | Defense | No | Responds to Sha | Dodge 1 attack |
| Tao (桃) | Recover | Yes (if injured) | No | Heal 1 HP |

Data: `Assets/StreamingAssets/CardDefine.json` (6 types defined).

### Role System
- **Factions**: Shu (蜀), Wu (吴), Wei (魏), Qun (群)
- **Skills**: Passive or Active, attached to roles
- **Tags**: Runtime buffs/debuffs via dictionary (e.g., `"Card|JIU"`)
- **Implemented**: `Assets/Scripts/Role/Roles/HuangGai.cs`

### AI
- `EnemyAI` extends `Player`, `UsesHumanInput => false`
- `BasicBattleAI` (static): Rule-based — heal when injured, then attack

## UI System Structure

| Subfolder | Purpose | Key Files |
|-----------|---------|-----------|
| `UI/UIFramework/` | Battle UI core | BattleUIBootstrap, BattleHandPresenter, BattleHandCardView, BattleTargetButtonView, BattleCardViewPool, FeedbackTextPool, BattleUIConfig, BattleTheme, SafeAreaFitter |
| `UI/UICompnent/` | HUD components | BattlePhaseHUD, BattlePlayerHUD, BattleResultPanel |
| `UI/Test/` | EditMode tests | BattleLayoutTests.cs（5 个 NUnit 测试，独立 asmdef `Unity.ThreeKingdom.UITests`） |

### UI Architecture Decisions
- **ScriptableObject config**: `BattleUIConfig` + `BattleTheme`（战斗）+ `HomeUIConfig`（主界面，`namespace ReEndUnity`）
- **ReEndUnity 主题桥接**: Battle/Home UI 均通过 `ReEndThemeManager.Current` 桥接主题色（`Assets/ReEndUnity/` 是**活跃依赖**）
- **Object pooling**: `BattleCardViewPool` + `FeedbackTextPool` replace Instantiate/Destroy
- **Dirty flags**: `layoutDirty` pattern instead of per-frame full rebuild
- **DOTween**: All UI animations (hover, drag, play, HP, transitions)
- **SafeAreaFitter**: Attached to Canvas(Screen) for notched device support
- **Font**: SourceHanSansSC SDF (3500 CJK characters), loaded via `Resources.Load`
- **Camera**: UICamera(Screen) = URP Overlay, in Main Camera stack
- **HomeBootstrap**: `[DefaultExecutionOrder(-50)]` — ReEnd 主题 → EventSystem(InputSystemUIInputModule) → Canvas → HomePresenter → SafeArea

### Latest Bugfixes (Jun 22, 2026)
| Bug | Root Cause | Fix |
|-----|-----------|-----|
| Hover animation jumps | EventSystem re-fires OnPointerEnter as card rect moves via DOTween | Hover detection migrated to presenter polling against cached ground rects |
| Click card then move cursor — stuck at hoverLift | OnPointerExit → re-enter cycle | Also eliminated by polling architecture |
| Hover feels sluggish | OutExpo 0.30s tail too long | Switched to OutQuart 0.30s |
| Drag locks up after 1 drag | DOTween anims override drag pos | Kill tweens on drag start; layoutDirty on drag end |
| Hand cards too small | Prefab RectTransform 100×100 point anchor | Stretch anchors (0,0)→(1,1) sizeDelta (0,0) |
| HUD overflows screen | sizeDelta on stretch-anchored RectTransform | Removed sizeDelta assignment |
| TMP font warnings | LiberationSans missing CJK glyphs | SourceHanSansSC SDF, 10 replacements |

## Key Architecture Patterns

- **Singletons**: `GamePlaying.instance`, `RoundManager.instance`, `CardManager.instance`, `PlayerManager.Instance`, `RoleManager.Instance`, `AudioManage.instance`
- **Abstract card base**: `Card` → `Sha`, `Shan`, `Tao` with virtual methods
- **Coroutines**: All game flow driven by `IEnumerator`
- **Event-driven**: `EventManager.OnCardPlayed` 静态事件触发卡牌行动；`Player.HandChanged` 驱动表现层
- **Data-driven**: JSON config (CardDefine.json), ScriptableObject config (BattleUIConfig/HomeUIConfig)
- **DOTween**: All animations with Ease curves. Hover: OutQuart 0.30s position/rotation/fade + OutBack scale.

## Package & Dependency List

### Key Unity Packages (`Packages/manifest.json`)
- **URP** `14.1.0` • **Cinemachine** `2.10.7` • **Input System** `1.14.3`
- **Timeline** `1.7.7` • **VFX Graph** `14.1.0` • **TextMeshPro** `3.0.10-t1`
- **Newtonsoft JSON** `3.2.2` • **AI Navigation** `1.1.7` • **Test Framework** `1.1.33`
- **uGUI** `2.0.0` • **gltfast** `6.8.0` • **Tuanjie AI Graph** `1.0.6`

### Third-Party / Local
- **DOTween** — Animation (in `Assets/Plugins/Demigiant/`)
- **ReEndUnity** — UI framework (battle + home UI depend on it, `Assets/ReEndUnity/`)
- **codely bridge** — local file dependency `../.codely.packages/`（工具链，勿提交该目录）

## Documentation System (Docs/)

All project documentation unified under `Docs/` (restructured 2026-08-12):

```
Docs/
├── README.md            # 文档总索引（新入口）
├── gdd/                 # 游戏设计文档（game-pillars / game-concept / card / character / role / ui / 3d-presentation / systems-index）
├── architecture/        # ADR 与架构（code-summary.md 总览 + card/character/ui/battle-presentation 等）
├── production/          # 进度（project-stage-report / backlog / milestones / sprints / qa / session-state）
├── planning/            # 规划（roadmap / playable-battle-plan 等）
├── guides/              # quickstart.md（AI 工作流入口）
├── reference/           # glossary.md（术语表）
├── testing/             # 测试说明（README / smoke-critical-paths）
└── tools/               # console-git.md
```

## Development Conventions

- **Language**: C#, Chinese XML doc comments (`<summary>`)
- **Naming**: PascalCase classes/properties, camelCase private fields
- **Singletons**: `public static T instance` in Awake (not MonoSingleton)
- **Gameplay values**: Must be data-driven (JSON / ScriptableObject)
- **Comments**: Chinese for game logic, English for technical APIs
- **Tests**: `Assets/Scripts/UI/Test/`（Editor-only asmdef）
- **Code standards**: `Assets/Scripts/CLAUDE.md` / `Assets/Scripts/AGENTS.md`

## Building & Running

- **编辑器运行**: 打开 Tuanjie Editor → 打开 `Assets/Scenes/BattleScene.scene` → 按 **Play**（Build Settings 首位是 MainHome.scene，主界面开发中）
- **编辑器路径**: `D:/Program Files/Tuanjie/Hub/Editor/2022.3.62t7/Editor/Tuanjie.exe`
- **CLI 构建示例**（batchmode）:
  ```bash
  "D:/Program Files/Tuanjie/Hub/Editor/2022.3.62t7/Editor/Tuanjie.exe" -batchmode -quit -projectPath . -buildTarget Android -logFile build.log
  ```
- **测试**: Test Runner（EditMode）运行 `Unity.ThreeKingdom.UITests`（BattleLayoutTests.cs，5 个用例）；测试说明见 `Docs/testing/`

## Version-Control Tips

- **Remote**: `https://github.com/HechaoYannet/3kingdom-unity.git`（当前分支 `feature/home-scene`）
- **Git LFS**: 已配置（`.gitattributes`，206+ 文件走 LFS，push 前需 `git lfs push --all`）
- `.gitignore` 覆盖: `Library/`, `Temp/`, `obj/`, `Logs/`, `UserSettings/`, `*.csproj`, `*.slnx`, `.codely-cli/`, `.codely.packages/`
- 只提交源资产、`ProjectSettings/`、`Packages/manifest.json`、文档
- **场景 GUID 注意**: 本项目 Tuanjie 的 `.meta` 文件 guid 为 base64 编码（非标准 hex），grep 反查脚本引用时勿用 `[0-9a-f]{32}` 正则，否则会漏判（2026-08-12 曾因此误删 Home/Login）

## TODO / Open Questions

- [ ] `DateMode/`（tabtoy 数据表工具链 + Character.xlsx）保留待确认，不需要可删
- [ ] `Library/` 缓存 ~3.8GB，可随时手动删除（重建需数分钟）
- [ ] SDF 字体 atlas dynamic mode: could expand for full Chinese text
- [ ] No CI pipeline（`.workflow/` 有 3 个 yml 流水线配置，未接入）
- [ ] No full game build script
- [ ] Role/UI 部分 TR 无 ADR 覆盖（见 `Docs/architecture/architecture-traceability.md` gaps）
- [ ] Event System 无 GDD（`Docs/gdd/` 缺失，参考 `Docs/architecture/code-summary.md`）
- [ ] MainHome 场景开发中：HomeBootstrap/HomePresenter 已恢复并挂载，需在编辑器验证运行
- [ ] `Assets/Res/UI/Eye Of The Tiger.mp3`（商业歌曲）确认版权

## Codely Structured Memories

### User

### Feedback

### Project
- [2026-08-12 01:04:38] 2026-08-12 项目清理完成并重写 CODELY.md：删除死代码 Mirror/AssetBundleFramework/xiaoxi/Data/CCGS/.claude/engine-reference；Event 系统（Assets/Scripts/Event/EventManager.cs）按用户要求已从 git 历史恢复（属活系统，勿再删）；文档统一到 Docs/ 树（总索引 Docs/README.md）；Home/Login 脚本因 MainHome.scene 场景引用被误删后已从 git 恢复（commit ccd85d5，Home 是活跃开发场景，勿再删）；场景 meta GUID 为 base64 编码，grep 反查脚本引用勿用 [0-9a-f]{32} 正则。**Why:** 防止未来会话误删活跃代码或按过时路径查找。**How to apply:** 删除脚本前先查场景 YAML 的 m_Script GUID 引用；文档路径以 Docs/ 为准。

### Reference
