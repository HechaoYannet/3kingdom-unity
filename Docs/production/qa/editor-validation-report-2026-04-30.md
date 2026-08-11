# 编辑器验证报告：2026-04-30

**分支**: `feature-battle-presentation-planning`  
**验证日期**: 2026-04-30  
**验证方式**: Unity / Tuanjie 编辑器手动检查，由用户执行，Codex 根据工程改动与口头结论回填  
**截图**: 未提供

## 结论摘要

- 本次竖切片渲染路线已经切换为 `URP`
- `Cinemachine`、`Timeline` 已安装且可正常使用
- `Shader Graph`、`VFX Graph` 已安装
- 当前未发现明显的 Built-in 迁移阻塞项
- 当前战斗场景布置和默认基线机位不能直接定稿，仍需进一步讨论和调整

## 工程侧已观测到的实际变更

### 1. 包与渲染管线

根据 [manifest.json](D:/Users/yhcne/UnityProject/three-king-dom-master/Packages/manifest.json)：
- 已引入 `com.unity.render-pipelines.universal` `14.1.0`
- 已引入 `com.unity.visualeffectgraph` `14.1.0`
- 已引入 `com.unity.cinemachine` `2.10.7`
- 已引入 `com.unity.timeline` `1.7.7`
- 已引入 `cn.tuanjie.ai.graph` `1.0.6`

### 2. Graphics Settings

根据 [GraphicsSettings.asset](D:/Users/yhcne/UnityProject/three-king-dom-master/ProjectSettings/GraphicsSettings.asset)：
- `m_CustomRenderPipeline` 已指向 URP 资产
- `m_SRPDefaultSettings` 已设置 `UniversalRenderPipeline`

### 3. URP 资产

工程中新增了以下资产：
- `Assets/Universal Render Pipeline Asset.asset`
- `Assets/Universal Render Pipeline Asset_Renderer.asset`
- `Assets/UniversalRenderPipelineGlobalSettings.asset`

### 4. 材质迁移

以下材质已从旧的内置管线 Shader 指向 URP Shader：
- `Assets/Resources/Materials/Card.mat`
- `Assets/Resources/Materials/CardBack.mat`
- `Assets/Resources/Materials/Othpath.mat`
- `Assets/Resources/Materials/Table.mat`

### 5. 场景侧相机数据

根据 [BattleScene.scene](D:/Users/yhcne/UnityProject/three-king-dom-master/Assets/Scenes/BattleScene.scene)：
- `Main Camera` 已附加 URP 相机数据
- `UICamera(Screen)`、`UICamera(Space)` 也附加了 URP 相机数据
- 至少两个 UI 相机当前处于禁用状态

### 6. 编辑器版本

根据 [ProjectVersion.txt](D:/Users/yhcne/UnityProject/three-king-dom-master/ProjectSettings/ProjectVersion.txt)：
- Unity Editor: `2022.3.62t7`
- Tuanjie Editor: `1.8.5`

## 当前仍未落定的事项

- 默认战斗基线机位如何设定
- 战斗场景空间布局如何配合镜头语言调整
- UI 相机与主战斗相机的长期组织方式
- 是否要立刻细分 PC / 低配质量档的 URP 资产策略
- 第一批关键特效是否优先使用 `Particle System + Shader`，还是直接引入 `VFX Graph`

## 建议后续动作

1. 先讨论并确定战斗空间布置原则和默认基线机位。
2. 再基于该机位重审 UI 相机启停策略。
3. 尽快补第一版相机验证文字结论。
4. 在此基础上再推进 `BTL-03` 的第一条完整动作链。
