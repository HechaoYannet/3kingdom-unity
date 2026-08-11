# THREE KINGDOM — 三国志策略卡牌对战

**Unity 2022.3 LTS + Tuanjie 1.8.5 · URP 14.1.0 · 生产阶段**

三国题材的确定性回合制卡牌对战游戏：每一次出牌都像一场电影级对决。

- **初心**：让每一次出牌都像一场电影级对决。策略化为电影级动作。
- **使命**：确定性的规则核心 + 现代化的 3D 演出 + 剧本驱动的剧情战斗。
- **五大支柱**：规则权威不可动摇 · 每张牌都是演出 · 一目了然 · 双端可降级 · 剧本模式（战斗即叙事）

## 快速开始

```text
AI 协作入口 → Docs/guides/quickstart.md
文档索引   → Docs/README.md
代码入口   → Assets/Scenes/BattleScene.scene（Build Settings 启用的实际入口）
```

| 文档 | 路径 |
|------|------|
| 设计文档（GDD） | `Docs/gdd/` |
| 架构文档（ADR） | `Docs/architecture/` |
| 项目进度 | `Docs/production/` |
| 规划 | `Docs/planning/` |
| 术语表 | `Docs/reference/glossary.md` |

## 项目结构

```text
Assets/
├── Scripts/           # 业务代码（Card/Character/Role/UI/Data/Audio/Tools）
├── Scenes/            # BattleScene.scene（入口）· MainHome.scene · LoginScene.unity（预留）
├── Res/               # 美术资源（音频/卡牌/UI）
├── Resources/         # 运行时加载资源（Prefabs/Fonts/UIpanel）
├── ReEndUnity/        # 第三方 UI 框架（战斗 UI 依赖）
└── Plugins/           # DOTween 等
Docs/                  # 全部项目文档（详见 Docs/README.md）
Packages/              # UPM 包
```

## 核心玩法

`GamePlaying` → `RoundManager.EnterGamer()` 初始化 → 每回合 **准备 → 判定 → 摸牌 → 出牌 → 弃牌 → 结束**；出牌触发响应窗口（杀 → 闪）。

已实现卡牌：**杀 / 闪 / 桃**（`Assets/StreamingAssets/CardDefine.json` 定义 6 种）。

## License

MIT — 见 [LICENSE](LICENSE)。
