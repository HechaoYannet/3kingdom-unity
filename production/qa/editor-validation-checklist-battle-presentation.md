# 编辑器验证清单：战斗表现竖切片

**状态**: Ready
**创建时间**: 2026-04-27
**使用时机**: 在 Unity/Tuanjie 编辑器中打开本分支项目，并验证“战斗表现优先”路线时使用。

## 需要产出的结果

完成本清单后，请记录以下内容：
- 最终渲染路线决策
- 包与工具链验证备注
- 材质 / Shader 阻塞项
- 战斗场景基线截图
- 如果已经做出相机原型，补一段短视频

证据建议存放于：
- `production/qa/evidence/`：如果你把截图 / 视频导出到仓库外管理
- 或者在本清单旁边补一份简短的 markdown 记录：如果你只需要写结论

## 步骤 1：确认分支并打开项目

- [ ] 确认当前分支为 `feature-battle-presentation-planning`
- [ ] 使用目标 Unity / Tuanjie 编辑器版本打开项目
- [ ] 在做任何改动前，先记录是否存在包导入错误或编译错误

## 步骤 2：确认当前渲染路径基线

- [ ] 打开 `Project Settings`，检查当前是否已经指定 `Render Pipeline Asset`
- [ ] 记录当前战斗竖切片是从 `Built-in` 还是 `URP` 起步
- [ ] 如果是 `Built-in`，记录是否明显依赖旧版 Shader / 材质
- [ ] 如果已经启用 `URP`，记录哪些 `URP Asset` 绑定到了哪些质量档位

## 步骤 3：审查材质与 Shader 阻塞项

检查有代表性的战斗场景材质，并记录阻塞项：
- [ ] `Standard Shader` 的使用情况
- [ ] `Surface Shader` 的使用情况
- [ ] `GrabPass` 或旧式图像效果依赖
- [ ] 在 URP 测试转换下会明显出错的自定义材质

每个阻塞项至少记录：
- 资源路径
- 阻塞类型
- 严重程度：`low / medium / high`
- 如果直观看得出来，顺手记一个可能的绕过方案

## 步骤 4：验证包状态

- [ ] 确认 `Cinemachine` 是否已安装，且导入无报错
- [ ] 确认 `Timeline` 是否已安装，且导入无报错
- [ ] 确认 `Shader Graph` 是否已安装，或当前缺失
- [ ] 确认 `VFX Graph` 是否已安装，或当前缺失

记录任何会阻塞下列工作的版本或兼容性问题：
- 相机 rig 搭建
- Timeline 演出制作
- Shader 编写 / Shader Graph 使用
- 特效制作

## 步骤 5：采集战斗场景基线

- [ ] 打开当前战斗场景
- [ ] 截一张默认全局战斗机位截图
- [ ] 截一张当前 3D 卡牌交互状态截图
- [ ] 截一张当前 HUD / 结果反馈可读性截图

需要回答的问题：
- 玩家能否清楚读到角色站位？
- 当前镜头角度能否作为默认战斗基线机位？
- 当前 UI 是否会与后续更强的镜头演出产生冲突？

## 步骤 6：制作第一版相机原型

- [ ] 搭建或验证一个默认战斗相机
- [ ] 搭建或验证一个动作强调相机
- [ ] 测试这两个相机之间的一次简单 blend

记录：
- blend 是否清晰可读
- 当前场景布局是否支持更强的相机运动
- 是否存在遮挡或构图问题

## 步骤 7：确定特效路线

- [ ] 判断第一版竖切片的关键特效能否先依赖 `Particle System + Shader` 完成
- [ ] 如果测试 `VFX Graph`，记录目标硬件要求是否可接受
- [ ] 明确低配 / 移动兼容档的降级预期

## 步骤 8：回填到仓库文档

完成编辑器验证后，请回填这些文件：
- `production/session-state/editor-required-handoff.md`
- `production/session-state/development-checklist.md`
- `production/backlog/battle-presentation-vertical-slice-backlog.md`

如果结论会明显改变路线，还要同步更新：
- `Docs/architecture/battle-presentation-first-architecture.md`
- `Docs/planning/6-week-battle-presentation-roadmap.md`

## 完成标准

- [ ] 渲染路线决策已经明确
- [ ] 包 / 工具链状态已经明确
- [ ] 场景可读性基线已经记录
- [ ] 相机原型可行性已经记录
- [ ] 特效降级预期已经记录
