# Docs — 文档系统索引

本目录是项目的**唯一文档树**。所有项目文档统一存放于此，按类别组织。

> 文档维护规则：
> - 新增文档先判断归属类别，不建新顶层目录。
> - 文档内引用的代码/文件路径若变更，必须同步更新。
> - 每篇文档顶部可标注状态：`✅ 有效` / `⚠️ 待更新` / `📦 归档`。

## 目录导航

| 子目录 | 内容 | 关键文件 |
|--------|------|----------|
| `gdd/` | 游戏设计文档（Game Design Documents） | `game-pillars.md`（五大支柱）· `game-concept.md` · `card-system.md` · `character-system.md` · `role-system.md` · `ui-system.md` · `3d-battle-presentation-system.md` · `systems-index.md` |
| `architecture/` | 架构决策记录（ADR）与系统架构 | `code-summary.md`（系统分解总览）· `card-system-architecture.md` · `character-system-architecture.md` · `battle-presentation-first-architecture.md` · `battle-runtime-architecture.md` · `ui-presentation-architecture.md` · `architecture-traceability.md` |
| `production/` | 项目进度管理 | `project-stage-report.md` · `backlog/` · `milestones/` · `sprints/` · `qa/` · `session-state/` |
| `planning/` | 规划文档 | `6-week-battle-presentation-roadmap.md` · `playable-battle-plan.md` 等 |
| `guides/` | 操作指南 | `quickstart.md`（AI 工作流入口） |
| `reference/` | 参考与术语 | `glossary.md` |
| `testing/` | 测试说明 | `README.md` · `smoke-critical-paths.md` |
| `tools/` | 开发工具说明 | `console-git.md` |

## 阅读顺序（新成员 / AI 代理）

1. `Docs/guides/quickstart.md` — 项目状态与入口
2. `Docs/architecture/code-summary.md` — 代码系统分解
3. `Docs/gdd/game-pillars.md` — 设计支柱（一切决策的裁决依据）
4. `Docs/production/project-stage-report.md` — 当前差距与下一步

## 代码目录对照

| 代码位置 | 对应文档 |
|----------|----------|
| `Assets/Scripts/Card/`, `CardManage/` | `gdd/card-system.md`, `architecture/card-system-architecture.md` |
| `Assets/Scripts/Character/` | `gdd/character-system.md`, `architecture/character-system-architecture.md` |
| `Assets/Scripts/Role/` | `gdd/role-system.md` |
| `Assets/Scripts/UI/` | `gdd/ui-system.md`, `architecture/ui-presentation-architecture.md` |
| `Assets/Scripts/Audio/`, `Tools/` | `architecture/code-summary.md` |
