# ReEndUnity 组件参考手册

> 面向 Agent 的组件选择指南。包含所有 37 个组件的 DOM 映射、信息承载能力、典型用法与完整 API。

---

## 组件速查表

| 场景 | 推荐组件 |
|------|---------|
| 战术面板/容器 | TacticalPanel, Card, HoloCard |
| 状态指示 | StatusBar, TacticalBadge, Badge, Avatar |
| 数据展示 | Stat, HoloCard, DataStream, MatrixGrid |
| 操作输入 | Button, Switch, Checkbox, RadioGroup, Input, Select, Textarea |
| 导航布局 | Tabs, Accordion, Separator, ScanDivider |
| 反馈提示 | Toast, Alert, Dialog, Popover, Tooltip, WarningBanner |
| 氛围装饰 | HUDOverlay, CommandOutput, FrequencyBars, MatrixGrid, DiamondLoader |
| 文字特效 | GlitchText, Label, CoordinateTag |

---

## 一、容器与面板

### ReEndTacticalPanel — 战术面板

- **DOM**: `<div class="tactical-panel">`
- **Shader**: ClipCorner (切角背景) + CornerBracket (角括号) + Scanline (扫描线)
- **承载信息**: 标题、状态指示、任意子内容
- **典型用途**: HUD 主面板、任务详情面板、装备/技能面板、系统状态窗口
- **创建**: `ReEndUI.TacticalPanel(parent, title)`
- **属性**: `Title`, `Status` (ReEndStatus)
- **方法**: `.SetTitle("PANEL")`, `.SetStatus(ReEndStatus.Online)`, `.SetWidth(400)`, `.SetHeight(280)`
- **内容槽**: `ContentSlot` (RectTransform) — 子元素放入此槽
- **视觉效果**: 切角卡片 + LT/RB 角括号 + CRT 扫描线覆盖

### ReEndCard — 标准卡片

- **DOM**: `<div class="card clip-corner corner-brackets">`
- **Shader**: ClipCorner (切角) + CornerBracket (角括号)
- **承载信息**: 标题、描述文本、子内容
- **典型用途**: 信息卡片、选项卡片、设置项容器、列表项
- **创建**: `ReEndUI.Card(parent, title)`
- **属性**: `Title`, `Description`, `Hoverable`, `Selected`
- **方法**: `.SetTitle("Title")`, `.SetDescription("desc")`, `.SetHoverable(true)`, `.SetSelected(false)`
- **内容槽**: `ContentArea` (RectTransform)

### ReEndHoloCard — 全息卡片

- **DOM**: `<div class="holo-card">`
- **Shader**: ClipCorner + Glow (发光) + HoloSweep (扫光+对角微光)
- **承载信息**: 标题 (overline)、大数值、副标题
- **典型用途**: KPI 展示、资源数量、战力值、倒计时、关键指标
- **创建**: `ReEndUI.HoloCard(parent, title)`
- **属性**: `Title`, `Value`, `Subtitle`, `BackgroundSprite`, `TiltIntensity`
- **方法**: `.SetTitle("ENERGY")`, `.SetValue("8,421")`, `.SetSubtitle("MW/h")`
- **视觉效果**: 切角 + 中心脉冲发光 + 135° 对角微光 + 悬停扫光线

### ReEndDialog — 模态对话框

- **DOM**: `<div class="dialog">` + `<div class="overlay">`
- **Shader**: ClipCorner (切角) + 全屏半透明遮罩
- **承载信息**: 标题、消息正文、操作按钮
- **典型用途**: 确认对话框、系统警告、输入弹窗
- **创建**: `ReEndUI.Dialog(parent, title)`
- **属性**: `Title`, `Message`, `ConfirmText`, `CancelText`, `Size` (ReEndDialogSize), `IsOpen`, `OnConfirm`, `OnCancel`, `OnClose`
- **方法**: `.SetTitle("Confirm")`, `.SetMessage("Proceed?")`, `.SetConfirm("OK")`, `.SetCancel("Cancel")`, `.SetSize(ReEndDialogSize.Md)`, `.SetOnConfirm(callback)`, `.SetOnCancel(callback)`, `.Open()`

### ReEndPopover — 弹出气泡

- **DOM**: `<div class="popover">`
- **Shader**: ClipCorner (小切角)
- **承载信息**: 简短文本内容
- **典型用途**: 点击展开的简短信息、上下文菜单触发器、快速操作面板
- **创建**: `ReEndUI.Popover(parent, content)`
- **属性**: `Content`, `Show`
- **方法**: `.SetContent("text")`, `.ShowAt(screenPos)`, `.Hide()`

### ReEndAccordion — 手风琴折叠面板

- **DOM**: `<details><summary>`
- **Shader**: 无 (纯布局)
- **承载信息**: 标题 + 可展开的内容区域
- **典型用途**: 设置面板、FAQ、任务链展开详情、技能树
- **创建**: `ReEndUI.Accordion(parent, title)`
- **属性**: `Title`, `Expanded`, `OnToggle`
- **方法**: `.SetTitle("Settings")`, `.SetExpanded(true)`, `.SetContentHeight(200)`, `.AddChild(transform)`

---

## 二、按钮与操作

### ReEndButton — 按钮

- **DOM**: `<button class="btn btn-primary">`
- **Shader**: ClipCorner (切角)
- **承载信息**: 文本 + 可选图标
- **典型用途**: 确认/取消、提交、导航、技能触发、任何交互触发
- **创建**: `ReEndUI.Button(parent, text)`
- **属性**: `Text`, `Variant` (Primary/Secondary/Destructive/Outline/Ghost/Link), `Size`, `Icon`, `Disabled`, `Loading`, `OnClick`
- **方法**: `.SetText("Submit")`, `.SetVariant(ReEndVariant.Primary)`, `.SetSize(ReEndSize.Md)`, `.SetIcon(sprite)`, `.SetDisabled()`, `.SetOnClick(callback)`
- **状态**: Hover (背景变亮), Press (缩放 0.96), Disabled (灰色+无交互)

### ReEndCheckbox — 复选框

- **DOM**: `<input type="checkbox">`
- **Shader**: ClipCorner (小切角) 在选中标记框
- **承载信息**: 标签文本、选中状态
- **典型用途**: 选项勾选、筛选器、设置开关（多选）、确认条款
- **创建**: `ReEndUI.Checkbox(parent, label)`
- **属性**: `Checked`, `LabelText`, `OnValueChanged`
- **方法**: `.SetChecked(true)`, `.SetLabel("Remember me")`, `.SetOnValueChanged(callback)`

### ReEndRadioGroup — 单选组

- **DOM**: `<input type="radio" name="group">` ×N
- **Shader**: 委托给 ReEndCheckbox
- **承载信息**: 选项列表 + 选中项索引
- **典型用途**: 单选设置、游戏模式选择、难度选择、阵营选择
- **创建**: `ReEndUI.RadioGroup(parent)`
- **属性**: `Options` (List\<string\>), `SelectedIndex`, `OnValueChanged`
- **方法**: `.AddOption("Easy")`, `.SetOptions(list)`, `.SetSelected(0)`, `.SetOnValueChanged(callback)`

### ReEndSwitch — 开关

- **DOM**: `<input type="checkbox" class="switch">`
- **Shader**: 无 (纯 C# 动画)
- **承载信息**: 标签 + 开/关状态文本
- **典型用途**: 功能开关、启用/禁用设置、模式切换
- **创建**: `ReEndUI.Switch(parent, label)`
- **属性**: `IsOn`, `LabelText`, `OnLabel`, `OffLabel`, `OnValueChanged`
- **方法**: `.SetOn(true)`, `.SetLabel("Sound")`, `.SetLabels("ENABLED", "MUTED")`, `.SetOnValueChanged(callback)`
- **动画**: Thumb 平移 + ScaleIn 弹跳

### ReEndSelect — 下拉选择

- **DOM**: `<select><option>`
- **Shader**: ClipCorner (小切角)
- **承载信息**: 选中值 + 下拉选项列表
- **典型用途**: 服务器选择、语言切换、排序方式、分辨率选择
- **创建**: `ReEndUI.Select(parent, placeholder)`
- **属性**: `Value`, `Placeholder`, `Options` (List\<string\>), `OnValueChanged`
- **方法**: `.AddOption("Option A")`, `.SetOptions(list)`, `.SetValue("A")`, `.SetPlaceholder("Select...")`, `.SetOnValueChanged(callback)`

---

## 三、文本与标签

### ReEndLabel — 文本标签

- **DOM**: `<span>` / `<p>`
- **Shader**: 无
- **承载信息**: 纯文本
- **典型用途**: 说明文字、字段标签、任何需要文本显示的位置
- **创建**: `ReEndUI.Label(parent, text)`
- **属性**: `Text`, `Size` (ReEndSize), `TextColor`, `IsMuted`
- **方法**: `.SetText("Hello")`, `.SetSize(ReEndSize.Md)`, `.SetColor(Color.red)`, `.SetMuted(true)`

### ReEndStat — 统计数值

- **DOM**: `<div class="stat">` (label + value + delta)
- **Shader**: 无
- **承载信息**: 标签 (overline) + 大数值 + 变化量 (▲/▼)
- **典型用途**: 属性面板 (ATK/DEF/SPD)、资源统计、战斗数据、性能指标
- **创建**: `ReEndUI.Stat(parent, label, value)`
- **属性**: `LabelText`, `ValueText`, `DeltaText`, `Positive`, `OnClickAction`
- **方法**: `.SetLabel("ATK")`, `.SetValue("1,200")`, `.SetDelta("+15", true)`

### ReEndGlitchText — 故障文字

- **DOM**: `<span class="animate-glitch">`
- **Shader**: 无 (CPU 协程)
- **承载信息**: 有故障动画效果的文本
- **典型用途**: 标题/Logo、警告文字、系统故障提示、氛围文字
- **创建**: `ReEndUI.GlitchText(parent, text)`
- **属性**: `Text`, `GlitchInterval` (默认 3s), `GlitchIntensity` (默认 2), `Animate`
- **方法**: `.SetText("SYSTEM ERROR")`, `.SetFontSize(36)`

### ReEndCoordinateTag — 坐标标签

- **DOM**: `<div class="coord-tag">`
- **Shader**: ClipCorner (小切角) + 半透明黑底
- **承载信息**: 扇区标签 + X/Y 坐标值
- **典型用途**: HUD 定位信息、地图坐标显示、目标位置、导航数据
- **创建**: `ReEndUI.CoordinateTag(parent, label)`
- **属性**: `Label`, `X`, `Y`
- **方法**: `.SetLabel("SECT")`, `.SetCoords("042", "187")`

### ReEndBadge — 徽章/标签

- **DOM**: `<span class="badge">`
- **Shader**: ClipCorner (小切角)
- **承载信息**: 短文本标签
- **典型用途**: 状态标签 (成功/失败/进行中)、分类标签、计数标签、技能标签
- **创建**: `ReEndUI.Badge(parent, text)`
- **属性**: `Text`, `Variant` (ReEndTagVariant), `Removable`, `OnRemove`
- **方法**: `.SetText("ACTIVE")`, `.SetVariant(ReEndTagVariant.Success)`, `.SetRemovable(true)`, `.SetOnRemove(callback)`

### ReEndTacticalBadge — 战术徽章

- **DOM**: `<span class="tactical-badge">`
- **Shader**: ClipCorner (小切角) + 状态颜色点
- **承载信息**: 状态文本 + 颜色指示点
- **典型用途**: 任务状态标记、干员状态、系统状态指示 (ONLINE/OFFLINE/WARNING)
- **创建**: `ReEndUI.TacticalBadge(parent, text)`
- **属性**: `Text`, `Status` (ReEndStatus)
- **方法**: `.SetText("ONLINE")`, `.SetStatus(ReEndStatus.Online)`
- **颜色映射**: Online→绿, Offline→灰, Warning→橙, Danger→红, Info→蓝

---

## 四、输入控件

### ReEndInput — 单行输入

- **DOM**: `<input type="text">`
- **Shader**: ClipCorner (小切角)
- **承载信息**: 占位提示 + 用户输入文本
- **典型用途**: 名称输入、搜索框、命令输入、ID 输入
- **创建**: `ReEndUI.Input(parent, placeholder)`
- **属性**: `Value`, `Placeholder`, `State` (ReEndInputState), `Size`
- **方法**: `.SetText("hello")`, `.SetPlaceholder("Search...")`, `.SetState(ReEndInputState.Error)`, `.SetSize(ReEndSize.Md)`, `.SetWidth(300)`

### ReEndTextarea — 多行输入

- **DOM**: `<textarea>`
- **Shader**: ClipCorner (小切角)
- **承载信息**: 占位提示 + 多行文本 + 字符计数
- **典型用途**: 备注输入、日志查看、消息编辑、多行配置
- **创建**: `ReEndUI.Textarea(parent, placeholder)`
- **属性**: `Value`, `Placeholder`, `State`, `MaxLength` (默认 500), `ShowCharCount`
- **方法**: `.SetText("...")`, `.SetPlaceholder("Notes...")`, `.SetMaxLength(200)`

---

## 五、反馈与通知

### ReEndToast — 轻量通知

- **DOM**: `<div class="toast">` (position: fixed, z-index: 3000)
- **Shader**: ClipCorner (小切角)
- **承载信息**: 简短消息文本 + 状态类型
- **典型用途**: 操作成功提示、保存确认、错误通知、临时状态通知
- **创建**: `ReEndUI.Toast(message, status)` (静态方法，自动管理 Canvas)
- **属性**: `Message`, `Status`, `Duration` (默认 3s), `Position` (预留，暂未实现定位)
- **静态**: `.DismissAll()`, `ReEndToast.Show(message, status, duration)` (自定义时长需直接调用 Show)
- **生命周期**: 自动在 Duration 秒后销毁

### ReEndAlert — 警告提示条

- **DOM**: `<div class="alert alert-warning">`
- **Shader**: ClipCorner (小切角)
- **承载信息**: 消息文本 + 严重等级 + 可选关闭按钮
- **典型用途**: 表单验证错误、系统通知横幅、操作结果反馈
- **创建**: `ReEndUI.Alert(parent, message)`
- **属性**: `Message`, `Severity` (ReEndSeverity), `Dismissible`, `OnDismiss`
- **方法**: `.SetMessage("Connection lost")`, `.SetSeverity(ReEndSeverity.Warning)`, `.SetDismissible()`
- **严重等级颜色**: Info→蓝, Success→绿, Warning→橙, Error→红, Caution→黄, Critical→深红

### ReEndWarningBanner — 警告横幅

- **DOM**: `<div class="warning-banner">`
- **Shader**: ClipCorner (小切角)
- **承载信息**: 警告消息 + 严重等级 + 可选操作按钮
- **典型用途**: 全屏/局部安全警告、合规通知、系统级警告
- **创建**: `ReEndUI.WarningBanner(parent, text)`
- **属性**: `Message`, `Severity` (Caution/Alert/Critical), `ActionLabel`, `OnAction`
- **方法**: `.SetMessage("Detected")`, `.SetSeverity(ReEndSeverity.Critical)`, `.SetAction("VIEW", callback)`

### ReEndTooltip — 工具提示

- **DOM**: `<div class="tooltip">` (position: absolute)
- **Shader**: ClipCorner (小切角)
- **承载信息**: 单行简短说明文本
- **典型用途**: 图标说明、按钮功能提示、缩写解释
- **创建**: `ReEndUI.Tooltip(parent, text)`
- **属性**: `Text`
- **方法**: `.SetText("Click to deploy")`, `.ShowAt(screenPos)`, `.Hide()`

---

## 六、进度与状态

### ReEndProgress — 进度条

- **DOM**: `<progress value="0.6">`
- **Shader**: 无 (Image.fillAmount / rect width)
- **承载信息**: 进度百分比 (0~1)
- **典型用途**: 任务进度、下载/上传进度、HP/能量条、加载进度
- **创建**: `ReEndUI.Progress(parent, value)`
- **属性**: `Value`, `ColorVariant` (Primary/Success/Warning/Danger), `Indeterminate`
- **方法**: `.SetValue(0.6f)`, `.SetColor(ReEndProgressColor.Danger)`, `.SetIndeterminate()` (不确定进度动画)

### ReEndStatusBar — 状态栏

- **DOM**: `<div class="status-bar">` (一排指示点 + 标签)
- **Shader**: 无
- **承载信息**: 多个状态指示器 (标签 + 颜色点)
- **典型用途**: 系统状态摘要、服务健康状态、多模块状态、连接状态
- **创建**: `ReEndUI.StatusBar(parent)`
- **属性**: `Indicators` (List\<StatusIndicator\>)
- **方法**: `.AddIndicator("NET", ReEndStatus.Online)`, `.AddIndicator(label, status)`

### ReEndAvatar — 头像

- **DOM**: `<img class="avatar">`
- **Shader**: ClipCorner (RT-only 切角) + 状态点
- **承载信息**: 头像图片 + 在线状态
- **典型用途**: 用户/干员头像、NPC 头像、队友状态指示
- **创建**: `ReEndUI.Avatar(parent)`
- **属性**: `Image` (Sprite), `Size` (ReEndAvatarSize: Xs~Xxl), `Status`, `ShowStatus`
- **方法**: `.SetImage(sprite)`, `.SetSize(ReEndAvatarSize.Lg)`, `.SetStatus(ReEndStatus.Online)`, `.SetShowStatus(false)`
- **尺寸**: Xs=24, Sm=32, Md=40, Lg=56, Xl=72, Xxl=96

---

## 七、导航与分隔

### ReEndTabs — 标签页

- **DOM**: `<nav class="tabs">`
- **Shader**: 无
- **承载信息**: 标签名称列表 + 激活索引
- **典型用途**: 多页内容切换、分类筛选、视图切换 (列表/网格/详情)
- **创建**: `ReEndUI.Tabs(parent)`
- **属性**: `TabLabels` (List\<string\>), `ActiveIndex`, `Variant` (Underline/Pill/Bordered), `OnTabChanged`
- **方法**: `.SetTabs(tabs)`, `.SetActiveIndex(0)`, `.SetOnTabChanged(callback)`

### ReEndSeparator — 分隔线

- **DOM**: `<hr>` / `<div class="separator">`
- **Shader**: 按样式: GradientLine / Glow / Diamond
- **承载信息**: 无 (纯视觉分隔)
- **典型用途**: 内容区域分隔、列表分隔、面板内分组
- **创建**: `ReEndUI.Separator(parent, direction)`
- **属性**: `Direction` (Horizontal/Vertical), `Style` (Solid/Gradient/Dashed/Glow/Diamond)
- **方法**: `.SetStyle(ReEndSeparatorStyle.Glow)`, `.SetLength(400)`
- **样式效果**: Solid→纯色线, Gradient→渐隐线, Glow→发光边缘线, Diamond→菱形装饰线

### ReEndScanDivider — 扫描分隔线

- **DOM**: `<div class="scan-divider">` (水平线 + 移动光点)
- **Shader**: 无 (C# 动画)
- **承载信息**: 无 (装饰性扫描线)
- **典型用途**: HUD 扫描效果、雷达分隔、数据流分隔
- **创建**: `ReEndUI.ScanDivider(parent)`
- **属性**: `ScanDuration` (默认 2s)
- **动画**: 亮点从左到右循环移动

---

## 八、数据与终端

### ReEndCommandOutput — 终端输出

- **DOM**: `<pre class="terminal-output">`
- **Shader**: Scanline (CRT 扫描线)
- **承载信息**: 命令行风格的多行文本输出
- **典型用途**: 控制台日志、系统日志查看器、调试终端、黑客主题 UI
- **创建**: `ReEndUI.CommandOutput(parent)`
- **属性**: `Prompt` (默认 "$>"), `Lines`, `AutoScroll`
- **方法**: `.Execute("scan --target", "3 hosts found")`, `.Clear()`

### ReEndDataStream — 数据流

- **DOM**: `<div class="data-stream">` (终端 + 切角边框)
- **Shader**: ClipCorner (切角) + Scanline (扫描线)
- **承载信息**: 循环滚动的多行数据
- **典型用途**: 实时数据馈送、传感器数据流、网络流量监控、战斗日志
- **创建**: `ReEndUI.DataStream(parent)`
- **属性**: `MaxLines` (默认 20), `ScrollSpeed`
- **方法**: `.AddLine("> data received")`, `.AddLines(list)`

### ReEndMatrixGrid — 矩阵点阵

- **DOM**: `<div class="matrix-grid">`
- **Shader**: MatrixDot (动画点阵)
- **承载信息**: 无 (纯装饰/氛围)
- **典型用途**: 背景氛围、数据可视化背景、黑客/赛博主题装饰
- **创建**: `ReEndUI.MatrixGrid(parent, cols, rows)`
- **属性**: `Columns` (默认 20), `Rows` (默认 10), `UpdateInterval`
- **方法**: `.SetDimensions(30, 15)`

### ReEndFrequencyBars — 频谱柱

- **DOM**: `<div class="frequency-bars">`
- **Shader**: FrequencyBar (动画柱状图)
- **承载信息**: 无 (纯装饰/氛围，模拟音频频谱)
- **典型用途**: 音频可视化(装饰)、处理活动指示器、数据活跃度背景
- **创建**: `ReEndUI.FrequencyBars(parent, barCount)`
- **属性**: `BarCount` (默认 16), `UpdateSpeed`, `MaxHeight`
- **方法**: `.SetBarCount(24)`

---

## 九、特色卡片

### ReEndMissionCard — 任务卡片

- **DOM**: `<div class="mission-card">`
- **Shader**: ClipCorner (sm 切角)
- **承载信息**: 任务标题、描述、进度条、状态文本
- **典型用途**: 任务列表、成就卡片、目标追踪、挑战卡片
- **创建**: `ReEndUI.MissionCard(parent, title)`
- **属性**: `Title`, `Description`, `Progress` (0~1), `StatusText`, `RewardText`, `OnClick`
- **方法**: `.SetTitle("Eliminate Target")`, `.SetDescription("...")`, `.SetProgress(0.75f)`, `.SetOnClick(callback)`

### ReEndOperatorCard — 干员卡片

- **DOM**: `<div class="operator-card">`
- **Shader**: ClipCorner (RT-only 切角)
- **承载信息**: 头像 + 名称 + 角色 + 职阶 + 等级 + 属性
- **典型用途**: 角色/干员信息卡、角色选择、队伍编成、角色详情
- **创建**: `ReEndUI.OperatorCard(parent, name)`
- **属性**: `OperatorName`, `Role`, `Class`, `Level`, `Portrait` (Sprite), `Atk`, `Def`, `Tech`
- **方法**: `.SetOperatorName("Ray")`, `.SetRole("Vanguard")`, `.SetClass("GUARD")`, `.SetPortrait(sprite)`, `.SetStats(800, 400, 150)`

### ReEndDiamondLoader — 菱形加载器

- **DOM**: `<span class="diamond-spin">`
- **Shader**: Diamond (旋转菱形)
- **承载信息**: 无 (加载指示)
- **典型用途**: 加载中指示器、处理中动画、等待状态
- **创建**: `ReEndUI.DiamondLoader(parent)`
- **属性**: `SpinDuration` (默认 0.8s)
- **方法**: `.SetSpinDuration(1.2f)`

---

## 十、全屏覆盖

### ReEndHUDOverlay — HUD 覆盖层

- **DOM**: `<div class="hud-overlay">` (position: fixed, inset: 0)
- **Shader**: CornerBracket (四角括号) + Scanline (扫描线)
- **承载信息**: 系统标签、坐标显示、十字准星
- **典型用途**: 全屏 HUD 框架、战斗界面、模拟驾驶 HUD、战术覆盖
- **创建**: `ReEndUI.HUDOverlay(systemLabel)`
- **属性**: `SystemLabel` (默认 "ENDFIELD::OPS")
- **方法**: `.SetSystemLabel("SYS::NOMINAL")`
- **自动**: CanvasGroup.blocksRaycasts=false (不阻挡点击)
- **元素**: 四角括号 + CRT 扫描线 + 左上系统标签 + 中心十字准星 + 右下坐标时间

---

## 十一、共享枚举

| 枚举 | 值 | 使用组件 |
|------|-----|---------|
| **ReEndVariant** | Primary, Secondary, Destructive, Outline, Ghost, Link | Button |
| **ReEndSize** | Xs, Sm, Md, Lg, Xl | Button, Input, Select, Label |
| **ReEndStatus** | Default, Success, Warning, Danger, Info, Online, Offline, Scanning | Avatar, StatusBar, TacticalBadge, TacticalPanel, Toast |
| **ReEndSeverity** | Info, Success, Warning, Error, Caution, Alert, Critical | Alert, WarningBanner |
| **ReEndCornerSize** | None, Sm(0.08), Md(0.12), Lg(0.16) | Theme.GetCornerSize |
| **ReEndTagVariant** | Default, Success, Warning, Danger, Info, Accent, Lime | Badge |
| **ReEndSeparatorStyle** | Solid, Gradient, Dashed, Glow, Diamond | Separator |
| **ReEndDirection** | Horizontal, Vertical | Separator |
| **ReEndInputState** | Default, Focused, Error, Disabled | Input, Textarea |
| **ReEndProgressColor** | Primary, Success, Warning, Danger | Progress |
| **ReEndToastPosition** | TopRight, TopCenter, TopLeft, BottomRight, BottomCenter, BottomLeft | Toast |
| **ReEndAvatarSize** | Xs(24), Sm(32), Md(40), Lg(56), Xl(72), Xxl(96) | Avatar |
| **ReEndDialogSize** | Xs(320), Sm(400), Md(480), Lg(600), Xl(720) | Dialog |
| **ReEndTabsVariant** | Underline, Pill, Bordered | Tabs |
| **ReEndAccentColor** | Yellow, Lime, Blue, Cyan, Red, Green, Orange, Purple | Theme.GetAccentColor |

---

## 十二、预留装饰 Shader（按需灵活叠加）

以下 4 个 shader 未绑定到特定组件，但可加载到任意 Image 上作为氛围增强层。适合 Agent 在需要增强视觉效果时灵活选用。

| Shader | 效果 | 推荐场景 |
|--------|------|---------|
| `ReEnd/UI/TopoContour` | 滚动等高线纹理 | 战术地图背景、雷达底纹、地形视图 |
| `ReEnd/UI/RadialGlow` | 径向椭圆光晕+脉冲 | 卡片氛围光、能量源光效、高亮指示 |
| `ReEnd/UI/Noise` | 胶片颗粒/噪点 | CRT老化、夜视噪点、全息抖动 |
| `ReEnd/UI/Glass` | FBM磨砂玻璃+切角 | 半透明浮层面板、Dialog轻量替代 |

使用方式：创建子物体 Image → `GetOrCreateMaterial(shaderName)` → 设置参数 → `raycastTarget = false`。

```csharp
// 例：为 TacticalPanel 添加地形等高线背景
var topoGo = CreateChild(panel.transform, "TopoBg");
var topoImg = topoGo.AddComponent<Image>();
var mat = GetOrCreateMaterial("ReEnd/UI/TopoContour");
mat.SetFloat("_ContourLevels", 12);
mat.SetColor("_ContourColor", new Color(1, 0.83f, 0.16f, 0.05f));
topoImg.material = mat;
topoImg.raycastTarget = false;
Stretch(topoImg.rectTransform);
// 将 topoGo 移到第一个子位置（背景层）
topoGo.transform.SetAsFirstSibling();
```

---

## 十三、基础链式 API

所有组件继承自 `ReEndBaseComponent`，可用以下链式方法：

```csharp
.SetName("MyComponent")       // 设置 GameObject 名称
.SetWidth(300)                 // 设置宽度
.SetHeight(44)                 // 设置高度
.SetSize(300, 44)              // 同时设置宽高
.SetAnchoredPos(16, -16)       // 设置锚点位置
.SetPivot(0, 1)                // 设置轴心
.SetAnchor(min, max)           // 设置锚点范围
.SetParent(parentTransform)    // 设置父级变换
```

---

## 组件选择决策树

```
需要反馈/提示用户?
├─ 短暂自动消失 → Toast
├─ 可关闭的横幅 → Alert (信息), WarningBanner (警告)
├─ 悬停说明 → Tooltip
└─ 模态确认 → Dialog

需要用户输入?
├─ 点击触发 → Button
├─ 二元选择 → Switch (开关), Checkbox (复选框)
├─ 多选一 → RadioGroup
├─ 多选 → 多个 Checkbox
├─ 下拉选择 → Select
├─ 文本输入 → Input (单行), Textarea (多行)

需要展示数据?
├─ 键值对 → Stat
├─ KPI 大数 → HoloCard
├─ 实时滚动数据 → DataStream
├─ 终端风格 → CommandOutput
├─ 进度 → Progress
├─ 多个状态 → StatusBar

需要结构化布局?
├─ 通用容器 → Card (简单), TacticalPanel (战术风)
├─ 折叠内容 → Accordion
├─ 多页切换 → Tabs
├─ 内容分隔 → Separator, ScanDivider

需要角色/实体展示?
├─ 头像+状态 → Avatar
├─ 完整角色信息 → OperatorCard
├─ 任务/目标 → MissionCard

需要氛围/装饰?
├─ 全屏框架 → HUDOverlay
├─ 动态背景 → MatrixGrid, FrequencyBars
├─ 加载动画 → DiamondLoader
├─ 文字特效 → GlitchText
```
