# 编辑器验证清单：战斗表现竖切片

**状态**: 已执行，待补文字结论
**创建时间**: 2026-04-27
**最近执行**: 2026-04-30
**使用时机**: 在 Unity/Tuanjie 编辑器中打开本分支项目，并验证“战斗表现优先”路线时使用。

## 本次执行说明

- [x] 用户已于 2026-04-30 完成一次 Unity 编辑器检查
- [x] 本次回填不附截图
- [ ] 渲染路线、包状态、相机原型、材质阻塞项的文字结论仍需补录

## 需要产出的结果

完成本清单后，应至少记录以下内容：
- 最终渲染路线决策
- 包与工具链验证备注
- 材质 / Shader 阻塞项
- 战斗场景基线观察结果
- 如果已经做出相机原型，补一段文字说明

如果没有截图或视频，也至少要补一份文字结论。

## 步骤 1：确认分支并打开项目

- [x] 确认当前分支为 `feature-battle-presentation-planning`
- [x] 使用目标 Unity / Tuanjie 编辑器版本打开项目
- [ ] 在做任何改动前发现的包导入错误或编译错误：待补文字结论

## 步骤 2：确认当前渲染路径基线

- [x] 已在 `Project Settings` 中检查当前是否指定 `Render Pipeline Asset`
- [ ] 当前战斗竖切片是从 `Built-in` 还是 `URP` 起步：待补文字结论
- [ ] 如果是 `Built-in`，是否明显依赖旧版 Shader / 材质：待补文字结论
- [ ] 如果已经启用 `URP`，哪些 `URP Asset` 绑定到了哪些质量档位：待补文字结论

## 步骤 3：审查材质与 Shader 阻塞项

- [x] 已执行代表性战斗场景材质检查
- [ ] `Standard Shader` 使用情况：待补文字结论
- [ ] `Surface Shader` 使用情况：待补文字结论
- [ ] `GrabPass` 或旧式图像效果依赖：待补文字结论
- [ ] 在 URP 测试转换下会明显出错的自定义材质：待补文字结论

建议后续补录格式：
- 资源路径
- 阻塞类型
- 严重程度：`low / medium / high`
- 可能绕过方案

## 步骤 4：验证包状态

- [x] 已检查 `Cinemachine`
- [x] 已检查 `Timeline`
- [x] 已检查 `Shader Graph`
- [x] 已检查 `VFX Graph`

待补文字结论：
- [ ] `Cinemachine` 是否已安装且可正常使用
- [ ] `Timeline` 是否已安装且可正常使用
- [ ] `Shader Graph` 是否已安装或缺失
- [ ] `VFX Graph` 是否已安装或缺失
- [ ] 是否存在阻塞相机、Timeline、Shader、特效制作的版本或兼容性问题

## 步骤 5：采集战斗场景基线

- [x] 已打开当前战斗场景
- [ ] 默认全局战斗机位观察结果：待补文字结论
- [ ] 当前 3D 卡牌交互状态观察结果：待补文字结论
- [ ] 当前 HUD / 结果反馈可读性观察结果：待补文字结论

需要补录的问题：
- 玩家能否清楚读到角色站位？
- 当前镜头角度能否作为默认战斗基线机位？
- 当前 UI 是否会与后续更强的镜头演出产生冲突？

## 步骤 6：制作第一版相机原型

- [x] 已执行相机相关检查或验证
- [ ] 是否已搭建默认战斗相机：待补文字结论
- [ ] 是否已搭建动作强调相机：待补文字结论
- [ ] 是否已测试两者之间的 blend：待补文字结论

需要补录：
- blend 是否清晰可读
- 当前场景布局是否支持更强的相机运动
- 是否存在遮挡或构图问题

## 步骤 7：确定特效路线

- [x] 已执行特效路线相关检查
- [ ] 第一版竖切片的关键特效能否先依赖 `Particle System + Shader`：待补文字结论
- [ ] 如果测试了 `VFX Graph`，目标硬件要求是否可接受：待补文字结论
- [ ] 低配 / 移动兼容档的降级预期：待补文字结论

## 步骤 8：回填到仓库文档

本次已完成的最小回填：
- [x] `production/session-state/editor-required-handoff.md`
- [x] `production/session-state/development-checklist.md`
- [x] `production/backlog/battle-presentation-vertical-slice-backlog.md`

如后续补充了明确结论，还应同步更新：
- `Docs/architecture/battle-presentation-first-architecture.md`
- `Docs/planning/6-week-battle-presentation-roadmap.md`

## 当前完成标准

- [x] 已记录“编辑器检查已执行”
- [x] 已明确“本次未附截图”
- [ ] 渲染路线决策已明确
- [ ] 包 / 工具链状态已明确
- [ ] 场景可读性基线已明确
- [ ] 相机原型可行性已明确
- [ ] 特效降级预期已明确
