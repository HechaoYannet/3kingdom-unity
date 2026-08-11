# Glossary — Three Kingdom Unity Project

Shorthand, acronyms, and internal language used in the project.

## Acronyms & Terms
| Term | Meaning | Context |
|------|---------|---------|
| **SDF** | Signed Distance Field font | TMP font asset for Chinese text |
| **URP** | Universal Render Pipeline | Active render pipeline 14.1.0 |
| **DOTween** | Animation tweening library | `Assets/Plugins/Demigiant/` |
| **SO** | ScriptableObject | Data assets like BattleUIConfig |
| **TMP** | TextMeshPro | Text rendering system |
| **CJK** | Chinese-Japanese-Korean | Character set for font |
| **ADR** | Architecture Decision Record | In `Docs/architecture/` |
| **GDD** | Game Design Document | In `Docs/gdd/` |
| **UI Framework** | `Assets/Scripts/UI/UIFramework/` | Core battle UI code |
| **UI Component** | `Assets/Scripts/UI/UICompnent/` | HUD components (note: misspelled folder) |
| **BattleScene** | `Assets/Scenes/BattleScene.scene` | Active gameplay scene |
| **vertical slice** | Playable feature demo | Battle presentation slice complete |
| **stretch anchor** | RectTransform (0,0)→(1,1) | Full parent fill layout |
| **commit threshold** | Drag-to-play Y threshold | Canvas-local, not screen-pixel |
| **dirty flag** | `layoutDirty` bool | Skip per-frame full rebuilds |
| **diff update** | Compare old/new cards | Only create/remove changed views |
| **overlay camera** | URP CameraType.Overlay | UICamera(Screen) in Main Camera stack |

## Battle UI Cards
| Term | Card Type | Mechanic |
|------|-----------|----------|
| Sha / 杀 | Attack | 1 damage, triggers response |
| Shan / 闪 | Defense | Responds to Sha, dodges |
| Tao / 桃 | Heal | +1 HP |

## UI Files (Post-Refactor)
| File | Role |
|------|------|
| BattleUIConfig.cs | ScriptableObject config for all UI constants |
| BattleTheme.cs | Visual theme ScriptableObject |
| BattleCardViewPool.cs | Object pool for card views |
| FeedbackTextPool.cs | Object pool for damage/heal text |
| SafeAreaFitter.cs | Screen safe area adapter |
| BattleHandPresenter.cs | Hand layout + drag/target management |
| BattleHandCardView.cs | Single card interaction + parallax + DOTween |
| BattleTargetButtonView.cs | Target selection button |
| BattleUIBootstrap.cs | Scene wiring + event system setup |
| BattlePhaseHUD.cs | Turn/phase/response HUD |
| BattlePlayerHUD.cs | HP bar HUD |
| BattleResultPanel.cs | Win/lose result display |

## Config Keys (BattleUIConfig)
| Key | Default | Purpose |
|-----|---------|---------|
| baseCardSpacing | 240 | Card spacing in fan layout |
| maxSpacingWidth | 1600 | Max width for spacing calc |
| fanAngleRange | 16° | Fan spread angle |
| hoverLift | 60 | Card lift on hover (px) |
| commitThreshold | 220 | Drag threshold (Canvas-local) |
| cardWidth | 220 | Card visual width |
| cardHeight | 300 | Card visual height |

## Known Issues
| Issue | Status | Notes |
|-------|--------|-------|
| 1 missing script warning | ⚠️ Unresolved | Old prefab GUID, harmless |
| No gameplay tests | 📋 In progress | `Assets/Scripts/UI/Test/BattleLayoutTests.cs` — 5 NUnit tests exist |
| No CI | 📋 Planned | — |
| Audio system thin | 📋 Known | AudioManage.cs needs expansion |
