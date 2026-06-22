# THREE KINGDOM — Working Memory

## Me
Codely CLI agent for Unity 2022.3 LTS card battle game. Battle-presentation vertical slice completed — UI systems fully refactored.

## Project State
- **Stage**: Production — code exists, docs catching up
- **Engine**: Unity `2022.3.62t7` + Tuanjie `1.8.5`
- **Render**: URP 14.1.0
- **Active Scene**: `Assets/Scenes/BattleScene.scene`
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
