# Three Kingdom — Battle UI Refactor

**Codename:** Battle UI
**Status:** ✅ Complete (10 phases + 4 post-launch bugfixes)

## What It Is
Full screen-space battle UI transformation. Replaced procedural white-box UI with ScriptableObject-driven, DOTween-animated, responsive layout system.

## Phases Completed
| Phase | Description | Files |
|-------|-------------|-------|
| 0 | Dead code cleanup | Deleted UIManager chain, CardOnDrawer/, .cs.1 files, etc. |
| 1 | Config centralization | BattleUIConfig.cs + .asset |
| 2 | Object pooling | BattleCardViewPool, FeedbackTextPool, dirty flags |
| 3 | Visual design system | BattleTheme, BattleCardSpriteLibrary, card layers |
| 4 | Responsive layout | SafeAreaFitter, adaptive spacing, resolution-independent threshold |
| 5 | DOTween animations | All card/HUD/feedback animations |
| 6 | Audio integration | BattleSfx enum in AudioManage |
| 7 | Prefab + scene organization | Prefabs, wired references, Canvas(Screen) setup |
| 8 | Mobile adaptation | DPI drag threshold, touch areas |
| 9 | Tests | BattleLayoutTests.cs (6 cases) |

## Post-Launch Fixes (Jun 22)
| Fix | Root Cause | Files Changed |
|-----|-----------|---------------|
| Drag interaction | DOTween conflicting with drag pos | BattleHandCardView, BattleHandPresenter |
| Hand spread | Prefab RectTransform point-anchored | BattleHandPresenter.prefab, BattleUIConfig |
| HUD overflow | sizeDelta on stretch-anchored HUD | BattlePlayerHUD.cs, scene via C# script |
| TMP font | LiberationSans missing CJK | 10 font references across UI scripts |
| Canvas scale | Saved as 0.72 instead of 1.0 | Scene via C# script |
| ResultPanel | Had Transform not RectTransform + duplicate component | Scene via C# script |

## Key Architecture Choices
- **DOTween** over manual `Mathf.Lerp` for all animations
- **ScriptableObject config** over hardcoded constants
- **Dirty flag** over per-frame full rebuild
- **Diff update** over full rebuild on hand change
- **Resources.Load font** over TMP_Settings.defaultFontAsset for CJK support
- **Overlay camera** in camera stack for UI

## Remaining Work
- [ ] Run BattleLayoutTests.cs in Test Runner
- [ ] Manual testing checklist (resolution scaling, hand interactions)
- [ ] Expand SDF atlas for full Chinese text
- [ ] Investigate 1 harmless missing script warning
