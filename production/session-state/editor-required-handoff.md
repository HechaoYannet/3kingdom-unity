# Unity Editor Required Handoff

**Created**: 2026-04-27
**Purpose**: Track actions that cannot be safely completed from the CLI and must be executed in Unity/Tuanjie Editor by the user.

## Pending

- [ ] EDITOR-001 确认当前渲染路线的最终书面结论，并记录竖切片是否以 Built-in 过渡还是立即转 URP。
- [ ] EDITOR-002 将本次材质 / Shader 审查结果补录为文字结论：
  - Standard shader usage
  - surface shaders
  - `GrabPass`-style effects
  - custom post-process dependencies
- [ ] EDITOR-003 若选择 URP，安装 / 配置 URP 资产并准备高低质量档配置。
- [ ] EDITOR-004 将 Timeline、Cinemachine、Shader Graph 的编辑器验证结果补录为文字结论。
- [ ] EDITOR-005 若不提供截图，则至少补录当前战斗场景的文字观察结论：
  - default battlefield readability
  - 3D card drag flow
  - player/target framing
  - current UI feedback
- [ ] EDITOR-006 将 baseline battle camera / action camera 的验证结果补录为文字结论。
- [ ] EDITOR-007 将关键特效是否可先依赖 `Particle System + Shader` 的判断补录为文字结论。
- [ ] EDITOR-008 若测试了 VFX Graph，将硬件兼容与降级要求补录为文字结论。

## Completed

- [x] EDITOR-009 已于 2026-04-30 执行 `production/qa/editor-validation-checklist-battle-presentation.md`，并完成最小回填；截图未提供，详细结论待补。

## Current Notes

- 编辑器检查已执行，但当前对话未提供可验证的具体检查结果。
- 本次仓库回填只记录“已完成检查”这一事实，以及后续需要补录的文字结论项。
