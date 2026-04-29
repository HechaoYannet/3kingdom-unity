# Unity Editor Required Handoff

**Created**: 2026-04-27
**Purpose**: Track actions that cannot be safely completed from the CLI and must be executed in Unity/Tuanjie Editor by the user.

## Pending

- [ ] EDITOR-001 补录当前 URP 路线下绑定的质量档与 URP Asset 细节。
- [ ] EDITOR-002 如有必要，将本次材质 / Shader 审查补充为更细的文字结论：
  - Standard shader usage
  - surface shaders
  - `GrabPass`-style effects
  - custom post-process dependencies
- [ ] EDITOR-003 若选择 URP，安装 / 配置 URP 资产并准备高低质量档配置。
- [ ] EDITOR-004 补录是否存在阻塞 Timeline、Cinemachine、Shader Graph、VFX Graph 的兼容性问题。
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
- [x] EDITOR-001 已确认竖切片渲染路线为 `URP`。
- [x] EDITOR-002 已确认本次检查未发现明显的 Built-in 迁移阻塞项。
- [x] EDITOR-004 已确认 `Timeline`、`Cinemachine`、`Shader Graph`、`VFX Graph` 均已安装，其中前两者可正常使用。

## Current Notes

- 编辑器检查已执行，但当前对话未提供可验证的具体检查结果。
- 本次仓库回填只记录“已完成检查”这一事实，以及后续需要补录的文字结论项。
- 当前已知结论：
  - 渲染路线：`URP`
  - `Cinemachine`、`Timeline` 已安装且可正常使用
  - `Shader Graph`、`VFX Graph` 已安装
  - 未发现明显的 Built-in 迁移阻塞项
