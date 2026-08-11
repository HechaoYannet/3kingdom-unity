# Smoke Test — Critical Paths

Setup work must not break the game. Run these checks after any repository restructure.

## Smoke Checks

1. Project opens with Unity 2022 LTS / Tuanjie-compatible editor settings intact
2. `BattleScene.scene` opens and its 4 business scripts resolve
   (`GamePlaying`, `Player`, `CardManager`, `BattleUIBootstrap`)
3. Battle scene assets and scripts remain present after documentation changes
4. Test scaffold exists: `Assets/Scripts/UI/Test/BattleLayoutTests.cs`
5. No workflow files were modified during preparation

## Files Checked

- `Docs/gdd/systems-index.md` is present and readable
- `Docs/production/sprint-status.yaml` is present and readable
