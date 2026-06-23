# THREE KINGDOM — Unity Card Game

## Project Overview

A **Three Kingdoms-themed card battle game** (三国杀-style) built on Unity 2022.3 LTS + Tuanjie Engine 1.8.5. Players take on character roles from the Three Kingdoms era and battle using a deterministic turn-based card system with initiative/response mechanics.

**初心**：让每一次出牌都像一场电影级对决。策略化为电影级动作。
**使命**：确定性的规则核心 + 现代化的 3D 演出 + 剧本驱动的剧情战斗，区别于市面普通卡牌游戏。

> **五大支柱** (`design/gdd/game-pillars.md`)：规则权威不可动摇 → 每张牌都是演出 → 一目了然 → 双端可降级 → 剧本模式（战斗即叙事）
> 所有设计决策必须对齐支柱。冲突时按优先级裁决。

- **Engine**: Unity `2022.3.62t7` (Tuanjie `1.8.5`)
- **Render Pipeline**: URP 14.1.0
- **Project Stage**: Production — code exists, battle-presentation vertical slice complete
- **Active Scene**: `Assets/Scenes/BattleScene.scene`
- **Game Concept**: `design/gdd/game-concept.md`
- **Game Pillars**: `design/gdd/game-pillars.md`

## Key Scenes

| Scene | Path | Purpose |
|-------|------|---------|
| LoginScene | `Assets/Scenes/LoginScene.unity` | Login/entry point |
| MainHome | `Assets/Scenes/MainHome.scene` | Home screen hub |
| BattleScene | `Assets/Scenes/BattleScene.scene` | Primary battle gameplay |

> **Note**: Uses both `.unity` (Unity) and `.scene` (Tuanjie) extensions. Check both patterns.

## 9 Core Systems

| # | System | Location | Purpose |
|---|--------|----------|---------|
| 1 | Asset Bundle Framework | `Assets/Scripts/AssetBundleFramework/` | Asset loading from bundles |
| 2 | Card System | `Assets/Scripts/Card/`, `CardManage/` | Card rules, deck, round flow |
| 3 | Character System | `Assets/Scripts/Character/` | Player entity, Enemy AI |
| 4 | Role System | `Assets/Scripts/Role/` | Faction, HP, skills, tags |
| 5 | UI System | `Assets/Scripts/UI/` | Screen-space HUD, world-space feedback, battle bootstrap |
| 6 | Audio System | `Assets/Scripts/Audio/` | Audio management |
| 7 | Data Management | `Assets/Scripts/Data/` | Save/load, singleton base |
| 8 | Event System | `Assets/Scripts/Event/` | Game event dispatch |
| 9 | Tools | `Assets/Scripts/Tools/` | Dictionary utils, MonoSingleton, file ops |

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
- **Implemented**: `HuangGai.cs`

### AI
- `EnemyAI` extends `Player`, `UsesHumanInput => false`
- `BasicBattleAI` (static): Rule-based — heal when injured, then attack

## UI System Structure (Refactored)

| Subfolder | Purpose | Key Files |
|-----------|---------|-----------|
| `UIFramework/` | Battle UI core | BattleUIBootstrap, BattleHandPresenter, BattleHandCardView, BattleTargetButtonView, BattleCardViewPool, FeedbackTextPool, BattleUIConfig, BattleTheme, SafeAreaFitter |
| `UICompnent/` | HUD components | BattlePhaseHUD, BattlePlayerHUD, BattleResultPanel |

### UI Architecture Decisions
- **ScriptableObject config**: `BattleUIConfig` + `BattleTheme` replace hardcoded magic numbers
- **Object pooling**: `BattleCardViewPool` + `FeedbackTextPool` replace Instantiate/Destroy
- **Dirty flags**: `layoutDirty` pattern instead of per-frame full rebuild
- **DOTween**: All UI animations (hover, drag, play, HP, transitions)
- **SafeAreaFitter**: Attached to Canvas(Screen) for notched device support
- **Font**: SourceHanSansSC SDF (3500 CJK characters), loaded via `Resources.Load`
- **Camera**: UICamera(Screen) = URP Overlay, in Main Camera stack

### Latest Bugfixes (Jun 22, 2026)
| Bug | Root Cause | Fix |
|-----|-----------|-----|
| Hover animation jumps — easing curve never plays | EventSystem re-fires OnPointerEnter as card visual rect moves via DOTween → new tween DOKill/restart per frame | Hover detection migrated from visual rect raycast (IPointerEnter/Exit) to presenter polling against cached ground rects |
| Click card then move cursor — card stuck at hoverLift | OnPointerExit → card animates down (triggers OnPointerEnter on nearby cursor via visual rect) → stuck cycle | Also eliminated by polling architecture: ground rects don't move, so no false re-entries |
| Hover feels sluggish after easing change | OutExpo 0.30s tail too long | Switched to OutQuart 0.30s — decay profile more linear, no perceptible tail drag |
| Drag locks up after 1 drag | DOTween anims override drag pos; layout not refreshed | Kill tweens on drag start; set layoutDirty on drag end |
| Hand cards too small | Prefab RectTransform was 100×100 point anchor | Stretch anchors (0,0)→(1,1) sizeDelta (0,0) |
| HUD overflows screen | sizeDelta on stretch-anchored RectTransform | Removed sizeDelta assignment |
| TMP font warnings | LiberationSans missing CJK glyphs | SourceHanSansSC SDF, 10 replacements |

## Key Architecture Patterns

- **Singletons**: `GamePlaying.instance`, `RoundManager.instance`, `CardManager.instance`, `PlayerManager.Instance`, `RoleManager.Instance`
- **Abstract card base**: `Card` → `Sha`, `Shan`, `Tao` with virtual methods
- **Coroutines**: All game flow driven by `IEnumerator`
- **Data-driven**: JSON config (CardDefine.json), ScriptableObject config (BattleUIConfig)
- **Event-driven**: `Player.HandChanged` for presentation binding
- **DOTween**: All animations with Ease curves. Hover: OutQuart 0.30s position/rotation/fade + OutBack scale. Uniform duration = 0.30s from ScriptableObject config.

## Package & Dependency List

### Key Unity Packages
- **URP** `14.1.0` • **Cinemachine** `2.10.7` • **Input System** `1.14.3`
- **Timeline** `1.7.7` • **VFX Graph** `14.1.0` • **TextMeshPro** `3.0.10-t1`
- **Newtonsoft JSON** `3.2.2` • **AI Navigation** `1.1.7` • **Test Framework** `1.1.33`

### Third-Party
- **Mirror** — Networking (inactive in scene)
- **DOTween** — Animation (in `Assets/Plugins/Demigiant/`)
- **Tuanjie AI Graph** `1.0.6`

## Development Conventions

- **Language**: C#, Chinese XML doc comments (`<summary>`)
- **Naming**: PascalCase classes/properties, camelCase private fields
- **Singletons**: `public static T instance` in Awake (not MonoSingleton)
- **Gameplay values**: Must be data-driven (JSON / ScriptableObject)
- **Comments**: Chinese for game logic, English for technical APIs
- **Tests**: Per-system `Test/` subfolders

## Key Reference Documents

| Document | Path | Purpose |
|----------|------|---------|
| Quick Start | `PROJECT-QUICKSTART.md` | AI workflow entry |
| Code Summary | `Docs/architecture/code-summary.md` | System breakdown |
| Project Stage | `production/project-stage-report.md` | Gap analysis |
| Coding Standards | `Assets/Scripts/CLAUDE.md` | Source coding rules |
| Card Data | `Assets/StreamingAssets/CardDefine.json` | 6 card types |

## TODO / Open Questions

- [ ] No automated gameplay tests (BattleLayoutTests.cs scaffold exists)
- [ ] No CI pipeline
- [ ] No full game build script
- [ ] `AudioManage.cs` needs expansion
- [ ] Mirror integration status unclear (NetworkMgr inactive)
- [ ] GDDs missing for AssetBundleFramework, Event System, Data Management
- [ ] ~1 harmless "missing script" warning from old prefab GUIDs
- [ ] SDF font atlas dynamic mode: could expand for full Chinese text
