# THREE KINGDOM — Working Memory

## Me
Codely CLI agent for Unity 2022.3 LTS card battle game. Battle-presentation vertical slice completed — UI systems fully refactored.

## Mission & Pillars

> **初心**：做一款三国题材的策略卡牌对战游戏，让每一次出牌都像一场电影级对决。
> **使命**：用确定性的规则核心 + 现代化的 3D 演出 + 剧本驱动的剧情战斗，区别于市面普通卡牌游戏。
>
> **五大支柱**（详见 `design/gdd/game-pillars.md`）：
> 1. **规则权威不可动摇** — Rule Core 是战斗的唯一裁决者
> 2. **每张牌都是演出** — 标准模板 + 英雄时刻，零裸牌
> 3. **一目了然** — 30 秒读懂战况
> 4. **PC/移动双端可降级** — 降 VFX 不丢信息
> 5. **剧本模式：战斗即叙事** — 借鉴崩铁，剧情战斗可被剧本编排

**在做任何设计/架构决策时，必须检查是否与五大支柱对齐。遇冲突时按支柱优先级裁决。**

## Project State
- **Stage**: Production — code exists, docs catching up
- **Engine**: Unity `2022.3.62t7` + Tuanjie `1.8.5`
- **Render**: URP 14.1.0
- **Active Scene**: `Assets/Scenes/BattleScene.scene`
- **Game Concept**: `design/gdd/game-concept.md`
- **Game Pillars**: `design/gdd/game-pillars.md`
- **Last Play Mode Verdict**: ✅ 0 errors, 0 TMP warnings, all 3 UI defects fixed

## Battle UI Status
| System | Status | Files |
|--------|--------|-------|
| BattleUIConfig | ✅ Data-driven SO | `BattleUIConfig.cs` + `.asset` |
| BattleTheme | ✅ Visual theme SO | `BattleTheme.cs` + `.asset` |
| Object Pools | ✅ Card + feedback pools | `BattleCardViewPool.cs`, `FeedbackTextPool.cs` |
| Hand Presenter | ✅ Config + pool + dirty flags | `BattleHandPresenter.cs` |
| Card View | ✅ DOTween + parallax + interaction | `BattleHandCardView.cs` |
| HUD | ✅ PhaseHUD + PlayerHUD + ResultPanel | `UICompnent/` |
| SafeArea | ✅ SafeAreaFitter on Canvas(Screen) | `SafeAreaFitter.cs` |
| Font | ✅ SourceHanSansSC SDF (3500 chars) | `Resources/Fonts/` |
| Camera | ✅ UICamera Overlay + Main Camera stack | scene |

## Latest Fixes (Jun 22)
1. **Drag Interaction** — DOTween kill on drag start; layoutDirty after drag end; state cleanup order in OnEndDrag
2. **Hand Spread** — Prefab RectTransform stretch anchors (was 100×100 point); config spacing 165→240
3. **HUD Overflow** — Removed sizeDelta in stretch-achored HUDs; Canvas scale 0.72→1.0; SafeAreaFitter on Canvas; ResultPanel RectTransform added
4. **Font** — 10x `TMP_Settings.defaultFontAsset` → `SourceHanSansSC SDF`; PermanentHintText runtime fix

## Key References
- `CODELY.md` — Full project overview
- `PROJECT-QUICKSTART.md` — Entry point
- `Docs/architecture/code-summary.md` — System breakdown
- `production/project-stage-report.md` — Gap analysis
