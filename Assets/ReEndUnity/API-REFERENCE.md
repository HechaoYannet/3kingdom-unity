# ReEndUnity API Reference

Arknights:Endfield 风格 UI 框架。纯代码驱动，无预制体依赖。所有 UI 通过 C# 静态工厂创建。

## Quick Start

```csharp
using ReEndUnity;
using UnityEngine;

// 按钮
ReEndUI.Button(parent, "START")
    .SetVariant(ReEndVariant.Primary)
    .SetOnClick(() => Debug.Log("clicked"));

// 战术面板（切角 + 角括号 + 扫描线）
ReEndUI.TacticalPanel(parent, "STATUS")
    .SetStatus(ReEndStatus.Online)
    .SetWidth(400).SetHeight(280);

// Toast 通知（自动管理独立 Canvas）
ReEndToast.Show("完成", ReEndStatus.Success);

// 自定义时长 Toast
ReEndToast.Show("请稍候...", ReEndStatus.Warning, duration: 5f);
```

## 架构

```
ReEndUI.XXX(parent, args)    静态工厂 → 返回 Builder
    .SetXxx(value)            链式配置 → 返回 this
    .SetOnXxx(callback)       事件绑定
                              ↑ 首次访问 RectTransform 自动触发 Build
```

三层覆盖：**Theme 全局默认 → Builder 覆盖 → 运行时直接改属性**

## 核心类型

| 类型 | 说明 |
|------|------|
| `ReEndTheme` | ScriptableObject，~100 Token（颜色/间距/字体/动画/切角），`ReEndThemeManager.Current` |
| `ReEndBaseComponent` | 所有组件基类，提供 `EnsureBuilt()`, `ApplyTheme()`, `RefreshTheme()`, `SetWidth/Height/Size` |
| `ReEndUI` | 静态工厂，37 个 `ReEndUI.Xxx(...)` 工厂方法 |
| `ReEndAnimationHelper` | DOTween 封装（ScaleIn / PulseGlow），DOTween 不可用时优雅降级 |

## 枚举速查

```
ReEndVariant:       Primary | Secondary | Destructive | Outline | Ghost | Link
ReEndSize:          Xs | Sm | Md | Lg | Xl
ReEndStatus:        Default | Success | Warning | Danger | Info | Online | Offline | Scanning
ReEndSeverity:      Info | Success | Warning | Error | Caution | Alert | Critical
ReEndCornerSize:    None | Sm(0.08) | Md(0.12) | Lg(0.16)
ReEndDirection:     Horizontal | Vertical
ReEndSeparatorStyle: Solid | Gradient | Dashed | Glow | Diamond
ReEndTabsVariant:   Underline | Pill | Bordered
ReEndProgressColor: Primary | Success | Warning | Danger
ReEndInputState:    Default | Focused | Error | Disabled
ReEndToastPosition: TopRight | TopCenter | TopLeft | BottomRight | BottomCenter | BottomLeft
ReEndAvatarSize:    Xs(24) | Sm(32) | Md(40) | Lg(56) | Xl(72) | Xxl(96)
ReEndDialogSize:    Xs(320) | Sm(400) | Md(480) | Lg(600) | Xl(720)
ReEndAccentColor:   Yellow | Lime | Blue | Cyan | Red | Green | Orange | Purple
ReEndTagVariant:    Default | Success | Warning | Danger | Info | Accent | Lime
```

---

## 完整 API

### 表单 & 输入 (8)

```csharp
// Button — 切角按钮，6 种变体
ReEndUI.Button(parent, "text")
    .SetVariant(ReEndVariant.Primary)  // Primary|Secondary|Destructive|Outline|Ghost|Link
    .SetSize(ReEndSize.Md)             // Xs|Sm|Md|Lg|Xl
    .SetIcon(sprite)
    .SetDisabled(true)
    .SetOnClick(() => { })
// IPointerEnter/Exit/Down/Up 实现 hover/press 反馈

// Badge — 小标签，7 种颜色变体
ReEndUI.Badge(parent, "text")
    .SetVariant(ReEndTagVariant.Default)  // Default|Success|Warning|Danger|Info|Accent|Lime
    .SetRemovable(true)
    .SetOnRemove(() => { })

// Input — 单行输入框
ReEndUI.Input(parent, "placeholder")
    .SetText("value")
    .SetPlaceholder("Search...")
    .SetState(ReEndInputState.Error)  // Default|Focused|Error|Disabled
    .SetSize(ReEndSize.Md)
    .SetWidth(300)

// Textarea — 多行文本域，带字符计数
ReEndUI.Textarea(parent, "placeholder")
    .SetText("...")
    .SetPlaceholder("Notes...")
    .SetMaxLength(200)
    .SetState(ReEndInputState.Disabled)

// Checkbox — 复选框（RadioGroup 内部也使用）
ReEndUI.Checkbox(parent, "label")
    .SetChecked(true)
    .SetOnValueChanged(v => { })  // bool

// RadioGroup — 单选组（委托 Checkbox，防止取消选中）
ReEndUI.RadioGroup(parent)
    .AddOption("A").AddOption("B")
    .SetSelected(0)
    .SetOnValueChanged(i => { })  // int

// Switch — 开关，C# 动画驱动
ReEndUI.Switch(parent, "label")
    .SetOn(true)
    .SetLabels("ON", "OFF")
    .SetOnValueChanged(v => { })  // bool

// Select — 下拉选择
ReEndUI.Select(parent, "placeholder")
    .AddOption("A").AddOption("B")
    .SetValue("A")
    .SetOnValueChanged(v => { })  // string
```

### 展示 & 布局 (10)

```csharp
// Card — 切角卡片 + LT/RB 角括号装饰
ReEndUI.Card(parent, "title")
    .SetDescription("desc")
    .SetSelected(true)
    .SetWidth(300).SetHeight(200)
// ContentArea (RectTransform) — 子元素添加到此区域

// Avatar — RT-only 切角头像 + 右下角状态点
ReEndUI.Avatar(parent)
    .SetImage(sprite)
    .SetSize(ReEndAvatarSize.Md)
    .SetStatus(ReEndStatus.Online)
    .SetShowStatus(false)

// Progress — 进度条
ReEndUI.Progress(parent, value: 0.5f)
    .SetColor(ReEndProgressColor.Primary)  // Primary|Success|Warning|Danger
    .SetIndeterminate(true)
    .SetWidth(200)

// Accordion — 折叠面板
ReEndUI.Accordion(parent, "title")
    .SetExpanded(false)
    .SetContentHeight(200)
    .AddChild(childTransform)
    .SetOnToggle(expanded => { })

// Tabs — 标签页（Underline/Pill/Bordered 变体）
ReEndUI.Tabs(parent)
    .SetTabs(new(){"Tab1", "Tab2"})
    .SetVariant(ReEndTabsVariant.Underline)
    .SetActiveIndex(0)
    .SetOnTabChanged(i => { })

// Popover — 弹出气泡
ReEndUI.Popover(parent, "content")
    .SetContent("text")
// .ShowAt(screenPos) .Hide()

// Dialog — 模态对话框（遮罩点击关闭 + ✕ 按钮）
ReEndUI.Dialog(parent, "title")
    .SetMessage("Are you sure?")
    .SetConfirm("OK").SetCancel("Cancel")
    .SetSize(ReEndDialogSize.Md)
    .SetOnConfirm(() => { })
    .SetOnCancel(() => { })
    .SetOnClose(confirmed => { })  // bool
// .Open()

// Separator — 5 种 shader 样式
ReEndUI.Separator(parent, ReEndDirection.Horizontal)
    .SetStyle(ReEndSeparatorStyle.Glow)  // Solid|Gradient|Dashed|Glow|Diamond
    .SetLength(400)

// Tooltip — 工具提示
ReEndUI.Tooltip(parent, "text")
    .SetText("Click to deploy")
// .ShowAt(screenPos) .Hide()

// Stat — 统计数值（标签 + 大数值 + ▲/▼ 变化量）
ReEndUI.Stat(parent, "ATK", "1,200")
    .SetDelta("+15", positive: true)
    .SetPositive(false)  // 红色▼
```

### 反馈 (2)

```csharp
// Alert — 警告提示条，7 种严重等级
ReEndUI.Alert(parent, "message")
    .SetSeverity(ReEndSeverity.Warning)  // Info|Success|Warning|Error|Caution|Alert|Critical
    .SetDismissible(true)
    .SetOnDismiss(() => { })

// Toast — 轻量通知，独立 Canvas 自动管理
ReEndToast.Show("message", ReEndStatus.Success);
ReEndToast.Show("error", ReEndStatus.Danger, duration: 5f);
ReEndToast.DismissAll();
```

### Label

```csharp
ReEndUI.Label(parent, "text")
    .SetSize(ReEndSize.Md)
    .SetColor(Color.white)
    .SetMuted(true)  // Theme.textMuted
```

---

### Signature — 特色战术组件 (16)

```csharp
// TacticalPanel — 战术面板（ClipCorner + CornerBracket + Scanline 三层 shader）
ReEndUI.TacticalPanel(parent, "PANEL")
    .SetStatus(ReEndStatus.Online)  // Online|Offline|Warning|Danger|Scanning
    .SetWidth(400).SetHeight(280)
// ContentSlot (RectTransform) — 子元素添加到此区域

// HoloCard — 全息卡片（ClipCorner + Glow + HoloSweep）
ReEndUI.HoloCard(parent, "ENERGY")
    .SetValue("8,421")
    .SetSubtitle("MW/h")
// IPointerEnter/Exit 实现悬停扫光线 + 对角微光 + 发光强度变化

// HUDOverlay — 全屏 HUD 覆盖层（四角括号 + 扫描线 + 十字准星）
ReEndUI.HUDOverlay("SYSTEM::ONLINE")
// 自动全屏，CanvasGroup.blocksRaycasts=false

// DataStream — 实时数据流终端（ClipCorner + Scanline）
ReEndUI.DataStream(parent)
    .AddLine("> data received")
    .AddLines(list)

// CommandOutput — 终端输出（Scanline CRT 效果）
ReEndUI.CommandOutput(parent)
    .Execute("scan --target", "3 hosts found")
    .Clear()

// MissionCard — 任务卡片（ClipCorner + 进度条）
ReEndUI.MissionCard(parent, "MISSION")
    .SetDescription("objective")
    .SetProgress(0.75f)
    .SetReward("5000 CREDITS")
    .SetOnClick(() => { })

// OperatorCard — 干员卡片（RT-only 切角 + 头像 + 属性）
ReEndUI.OperatorCard(parent, "NAME")
    .SetRole("Vanguard")
    .SetClass("GUARD")
    .SetPortrait(sprite)
    .SetStats(800, 400, 150)  // Atk/Def/Tech

// StatusBar — 状态栏（多指示器：彩色圆点 + 标签）
ReEndUI.StatusBar(parent)
    .AddIndicator("NET", ReEndStatus.Online)
    .AddIndicator("DB", ReEndStatus.Warning)

// TacticalBadge — 战术徽章（ClipCorner + 状态颜色点）
ReEndUI.TacticalBadge(parent, "ONLINE")
    .SetStatus(ReEndStatus.Online)

// WarningBanner — 警告横幅（ClipCorner + 操作按钮）
ReEndUI.WarningBanner(parent, "WARNING")
    .SetSeverity(ReEndSeverity.Critical)
    .SetAction("VIEW", () => { })

// ScanDivider — 扫描分隔线（动画移动光点）
ReEndUI.ScanDivider(parent)

// CoordinateTag — 坐标标签（半透明黑底 + ClipCorner）
ReEndUI.CoordinateTag(parent, "SECT")
    .SetCoords("128", "064")

// DiamondLoader — 菱形加载动画（UI-Diamond shader）
ReEndUI.DiamondLoader(parent)
    .SetSpinDuration(1.2f)

// GlitchText — 故障文字（CPU 协程字符替换 + 位置抖动）
ReEndUI.GlitchText(parent, "SYSTEM ERROR")
    .SetFontSize(36)

// MatrixGrid — 矩阵点阵背景（UI-MatrixDot shader，单 Image）
ReEndUI.MatrixGrid(parent, cols: 20, rows: 10)

// FrequencyBars — 频谱柱（UI-FrequencyBar shader，单 Image）
ReEndUI.FrequencyBars(parent, barCount: 16)
```

---

## Shader 参考

| Shader | 用途 | 使用组件 |
|--------|------|---------|
| `ReEnd/UI/ClipCorner` | 切角背景（RT+LB 或 RT-only） | 19 个组件 |
| `ReEnd/UI/CornerBracket` | L 形角括号装饰 | Card, TacticalPanel, HUDOverlay |
| `ReEnd/UI/Scanline` | CRT 扫描线覆盖 | CommandOutput, DataStream, HUDOverlay, TacticalPanel |
| `ReEnd/UI/Glow` | 边缘发光 | HoloCard, Separator |
| `ReEnd/UI/Diamond` | SDF 菱形标记 | DiamondLoader, Separator |
| `ReEnd/UI/FrequencyBar` | 动画频谱柱 | FrequencyBars |
| `ReEnd/UI/MatrixDot` | 动画点阵网格 | MatrixGrid |
| `ReEnd/UI/HoloSweep` | 全息扫光线 + 对角微光 | HoloCard |
| `ReEnd/UI/GradientLine` | 渐变线 | Separator |
| `ReEnd/UI/GridBackground` | 网格背景 | Editor Tuner 引用 |
| `ReEnd/UI/Glitch` | 图像故障效果 | 预留（TMP 文本不兼容） |

---

## 预留装饰 Shader（按需灵活使用）

> **Agent 应在以下场景主动推荐这些 shader。** 使用 `GetOrCreateMaterial("ReEnd/UI/...")` 加载到任意 Image 上，设置为全屏或指定尺寸的覆盖层（`raycastTarget = false`）。

### `ReEnd/UI/TopoContour` — 地形等高线

模拟地形图等高线纹理，可缓慢滚动。

| 参数 | Range | 默认 | 说明 |
|------|-------|------|------|
| `_ContourColor` | Color | (1,1,1,0.07) | 次要等高线颜色 |
| `_ContourMajorColor` | Color | (1,1,1,0.12) | 主要等高线颜色 |
| `_ContourLevels` | 4~32 | 16 | 等高线密度 |
| `_ContourWidth` | 0.001~0.05 | 0.008 | 线宽 |
| `_ContourScale` | 0.5~4.0 | 1.5 | 噪声缩放 |
| `_ContourSpeed` | 0~0.5 | 0.02 | 滚动速度 |
| `_NoiseSeed` | 0~100 | 42 | 噪声种子 |

**适用**: 战术地图背景 · 雷达面板底纹 · 地形分析视图 · 科幻地表装饰

```csharp
var mat = GetOrCreateMaterial("ReEnd/UI/TopoContour");
mat.SetFloat("_ContourLevels", 12);
mat.SetFloat("_ContourSpeed", 0.01f);
mat.SetColor("_ContourColor", new Color(1, 0.83f, 0.16f, 0.05f));
overlayImage.material = mat;
```

### `ReEnd/UI/RadialGlow` — 径向发光

从指定中心辐射的光晕，支持椭圆变形和呼吸脉冲。

| 参数 | Range | 默认 | 说明 |
|------|-------|------|------|
| `_GlowColor` | Color | (1,0.83,0.16,0.12) | 发光颜色 |
| `_GlowCenterX` | 0~1 | 0.5 | 中心 X (UV) |
| `_GlowCenterY` | 0~1 | 0.0 | 中心 Y (0=底部) |
| `_GlowRadiusX` | 0~2 | 0.8 | 水平半径 |
| `_GlowRadiusY` | 0~2 | 0.5 | 垂直半径 |
| `_GlowFalloff` | 0.1~1.5 | 0.7 | 衰减曲线 |
| `_GlowPulse` | 0~0.5 | 0.0 | 脉冲幅度 (>0 呼吸) |

**适用**: 卡片背后氛围光 · 高亮区域指示 · 能量源光效 · 战术标记背景

```csharp
// 底部暖色光晕
var mat = GetOrCreateMaterial("ReEnd/UI/RadialGlow");
mat.SetFloat("_GlowCenterY", 0f);
mat.SetColor("_GlowColor", new Color(1f, 0.83f, 0.16f, 0.1f));
mat.SetFloat("_GlowPulse", 0.05f);
```

### `ReEnd/UI/Noise` — 噪声纹理

胶片颗粒/噪点覆盖层，可配置密度和动画速度。

| 参数 | Range | 默认 | 说明 |
|------|-------|------|------|
| `_NoiseOpacity` | 0~0.15 | 0.025 | 噪点透明度 |
| `_NoiseScale` | 0.2~10 | 2 | 颗粒大小 |
| `_NoiseSpeed` | 0~0.05 | 0 | 动画速度 (0=静态) |
| `_NoiseOctaves` | 1~5 | 4 | 分形叠加层数 |

**适用**: CRT 屏幕老化 · 夜视/热成像噪点 · 全息投影抖动 · 背景质感

```csharp
var mat = GetOrCreateMaterial("ReEnd/UI/Noise");
mat.SetFloat("_NoiseOpacity", 0.03f);
mat.SetFloat("_NoiseScale", 3f);
mat.SetFloat("_NoiseSpeed", 0.01f);
```

### `ReEnd/UI/Glass` — 玻璃面板

FBM 噪声毛玻璃 + RT/LB 切角 + 可配边框。

| 参数 | Range | 默认 | 说明 |
|------|-------|------|------|
| `_GlassColor` | Color | (0.078,0.078,0.078,0.55) | 底色 |
| `_GlassNoiseStrength` | 0~0.1 | 0.015 | 磨砂强度 |
| `_GlassNoiseScale` | 1~50 | 12 | 磨砂颗粒大小 |
| `_BorderColor` | Color | (1,1,1,0.1) | 边框颜色 |
| `_BorderWidth` | 0~0.05 | 0.005 | 边框粗细 |
| `_CornerSize` | 0~0.5 | 0.12 | 切角 (SDF) |
| `_EdgeSoftness` | 0~0.1 | 0.02 | 切角柔和度 |

**适用**: 半透明浮层面板 · 设置菜单背景 · Dialog 轻量替代 · 悬浮信息卡

```csharp
var mat = GetOrCreateMaterial("ReEnd/UI/Glass");
mat.SetFloat("_CornerSize", 0.12f);
mat.SetColor("_GlassColor", new Color(0.05f, 0.05f, 0.05f, 0.6f));
mat.SetFloat("_GlassNoiseStrength", 0.02f);
```

---

## 基础链式 API

所有组件继承自 `ReEndBaseComponent`：

```csharp
// 尺寸
.SetWidth(300)                  // 设置宽度
.SetHeight(44)                  // 设置高度
.SetSize(300, 44)               // 同时设置宽高

// 布局（可用但较少使用）
.SetName("MyComponent")         // GameObject 名称
.SetAnchoredPos(16, -16)        // 锚点位置
.SetPivot(0, 1)                 // 轴心
.SetAnchor(min, max)            // 锚点范围
.SetParent(parentTransform)     // 父级变换

// 主题
.RefreshTheme()                 // 重新应用主题（切换皮肤时）
.EnsureBuilt()                  // 确保 Build 完成

// 静态创建
ReEndBaseComponent.Create<T>(parent, "name")             // 创建容器
ReEndBaseComponent.CreateWithImage<T>(parent, "name")    // 创建带 Image 的容器
```

## Theme 访问

```csharp
var theme = ReEndThemeManager.Current;  // 全局主题实例

// 颜色
theme.primary, theme.card, theme.surface1, theme.surface2, theme.surface3
theme.surfaceHover, theme.textPrimary, theme.textSecondary, theme.textMuted
theme.textPlaceholder, theme.textLink, theme.borderDefault, theme.borderAccent
theme.popover, theme.popoverForeground, theme.destructive, theme.destructiveForeground

// 效果色
theme.efBlack, theme.efGreen, theme.efRed, theme.efYellow, theme.efOrange
theme.efBlue, theme.efCyan, theme.efPurple, theme.efLime, theme.efGrayMid
theme.efWhiteMuted, theme.greenSoft, theme.redSoft, theme.orangeSoft

// 切角 & 括号
theme.clipCornerSm(0.08), theme.clipCornerMd(0.12), theme.clipCornerLg(0.16)
theme.bracketColor, theme.bracketSize, theme.bracketWidth

// 间距
theme.space1(4) ~ theme.space32(128)

// 字体大小
theme.captionSize(12), theme.bodySmSize(14), theme.bodySize(16), theme.bodyLgSize(18)
theme.h4Size(22), theme.h3Size(28), theme.h2Size(36), theme.h1Size(48)
theme.displayLgSize(56), theme.overlineSize(11)

// 图标大小
theme.iconSm(20), theme.iconMd(24), theme.iconLg(32)

// 动画时长
theme.durationFast(0.15), theme.durationNormal(0.3)

// 计算方法
theme.GetSpace(level)
theme.GetSizeValue(ReEndSize)
theme.GetFontSize(ReEndSize)
```

---

## 组件速查（按场景）

| 场景 | 推荐组件 |
|------|---------|
| 点击触发操作 | Button |
| 二元选择 | Switch, Checkbox |
| 多选一 | RadioGroup |
| 文本输入 | Input, Textarea |
| 下拉选择 | Select |
| KPI 大数展示 | HoloCard, Stat |
| 实时滚动数据 | DataStream |
| 终端风格输出 | CommandOutput |
| 进度展示 | Progress |
| 多状态指示 | StatusBar |
| 结构化容器 | Card, TacticalPanel |
| 折叠内容 | Accordion |
| 多页切换 | Tabs |
| 内容分隔 | Separator, ScanDivider |
| 模态确认 | Dialog |
| 弹出信息 | Popover, Tooltip |
| 自动通知 | Toast |
| 警告提示 | Alert, WarningBanner |
| 全屏框架 | HUDOverlay |
| 标签/徽章 | Badge, TacticalBadge, CoordinateTag |
| 角色展示 | Avatar, OperatorCard |
| 任务展示 | MissionCard |
| 加载动画 | DiamondLoader |
| 氛围装饰 | MatrixGrid, FrequencyBars |
| 文字特效 | GlitchText |
| 纯文本 | Label |
