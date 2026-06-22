# ReEndUnity API Reference (for AI Agents)

Arknights:Endfield 风格 UI 框架。纯代码驱动，无预制体依赖，所有 UI 通过 C# 创建。

## Quick Start (最小可用示例)

```csharp
using ReEndUnity;
using UnityEngine;

// 一个按钮
ReEndUI.Button(parentTransform, "START")
    .SetVariant(ReEndVariant.Primary)
    .SetOnClick(() => Debug.Log("clicked"));

// 一个战术面板
ReEndUI.TacticalPanel(parentTransform, "STATUS")
    .SetStatus(ReEndStatus.Online)
    .SetWidth(400).SetHeight(300);

// Toast 通知
ReEndToast.Show("完成", ReEndStatus.Success);
```

## 架构

```
ReEndUI.XXX(parent, args)    静态工厂，所有组件的入口，返回 Builder
    .SetXxx(value)            链式配置，返回 this
    .SetOnXxx(callback)       事件绑定
                              ↑ 访问 .RectTransform 或 .GameObject 时自动 Build
```

三层覆盖：Theme 全局默认 → 组件 Builder 覆盖 → 运行时直接改属性

## 核心类型

| 类型 | 说明 |
|------|------|
| `ReEndTheme` | ScriptableObject，120+ Token（颜色/间距/字体/动画），`ReEndThemeManager.Current` 全局访问 |
| `ReEndBaseComponent` | 所有组件的抽象基类，提供 `EnsureBuilt()`, `ApplyTheme()`, `SetWidth/SetHeight/SetSize` |
| `ReEndUI` | 静态工厂类，`ReEndUI.Button(...)` 等 75+ 工厂方法 |
| `ReEndAnimationHelper` | DOTween 封装，DOTween 不可用时优雅降级 |

## 枚举速查

```
ReEndVariant:     Primary | Secondary | Destructive | Outline | Ghost | Link
ReEndSize:        Xs | Sm | Md | Lg | Xl
ReEndStatus:      Default | Success | Warning | Danger | Info | Online | Offline
ReEndSeverity:    Info | Success | Warning | Error | Caution | Alert | Critical
ReEndCornerSize:  None | Sm(8px) | Md(12px) | Lg(16px)
ReEndDirection:   Horizontal | Vertical
ReEndSeparatorStyle: Solid | Gradient | Dashed | Glow | Diamond
ReEndTabsVariant: Underline | Pill | Bordered
ReEndChartType:   Line | Bar | Area | Pie
ReEndProgressColor: Primary | Success | Warning | Danger
ReEndInputState:  Default | Focused | Error | Disabled
ReEndEmptyStatePreset: NoData | NoResults | Error | Maintenance | Empty
ReEndSkeletonVariant: Line | Text | Avatar | Card
ReEndToastPosition: TopRight | TopCenter | TopLeft | BottomRight | BottomCenter | BottomLeft
ReEndAvatarSize:  Xs | Sm | Md | Lg | Xl | Xxl
ReEndDialogSize:  Xs(320) | Sm(400) | Md(480) | Lg(600) | Xl(720)
ReEndSnapPoint:   Hidden | Quarter | Half | Full
ReEndViewMode:    Grid | List
ReEndSortDirection: Ascending | Descending
ReEndAccentColor: Yellow | Lime | Blue | Cyan | Red | Green | Orange | Purple
ReEndTagVariant:  Default | Success | Warning | Danger | Info | Accent | Lime
```

## 完整 API

### 表单 & 输入 (16)

```csharp
// Label
ReEndUI.Label(parent, "text")
    .SetSize(ReEndSize.Md) .SetColor(Color.white) .SetMuted(true)

// Button
ReEndUI.Button(parent, "text")
    .SetVariant(ReEndVariant.Primary)  // Primary | Secondary | Destructive | Outline | Ghost | Link
    .SetSize(ReEndSize.Md)             // Xs | Sm | Md | Lg | Xl
    .SetIcon(sprite)                   // 可选左侧图标
    .SetDisabled(true)                 // 禁用
    .SetOnClick(() => { })             // 点击回调

// Badge
ReEndUI.Badge(parent, "text")
    .SetVariant(ReEndTagVariant.Default)  // Default | Success | Warning | Danger | Info | Accent | Lime
    .SetRemovable(true)                   // 显示移除按钮
    .SetOnRemove(() => { })

// Input
ReEndUI.Input(parent, "placeholder")
    .SetText("default") .SetPlaceholder("hint")
    .SetState(ReEndInputState.Default)
    .SetSize(ReEndSize.Md)

// Textarea
ReEndUI.Textarea(parent, "placeholder")
    .SetText("...") .SetMaxLength(500)
    .SetState(ReEndInputState.Default)

// Checkbox
ReEndUI.Checkbox(parent, "label")
    .SetChecked(true) .SetLabel("agree")
    .SetOnValueChanged(v => { })  // bool

// RadioGroup
ReEndUI.RadioGroup(parent)
    .AddOption("A") .AddOption("B")
    .SetSelected(0)
    .SetOnValueChanged(i => { })  // int

// Switch
ReEndUI.Switch(parent, "label")
    .SetOn(true) .SetLabels("ON", "OFF")
    .SetOnValueChanged(v => { })  // bool

// Select
ReEndUI.Select(parent, "placeholder")
    .AddOption("A") .AddOption("B")
    .SetValue("A")
    .SetOnValueChanged(v => { })  // string

// NumberInput
ReEndUI.NumberInput(parent)
    .SetValue(0) .SetRange(0, 100) .SetStep(1)
    .SetOnValueChanged(v => { })  // float

// OTPInput
ReEndUI.OTPInput(parent, length: 6)
    .SetOnComplete(code => { })  // string, 输入完成时
// 方法: .Clear() 清空所有位

// DatePicker
ReEndUI.DatePicker(parent)
    .SetValue(DateTime.Today)
    .SetRange(minDate, maxDate)
    .SetOnValueChanged(d => { })  // DateTime

// FileUpload
ReEndUI.FileUpload(parent)
    .SetLabel("拖拽文件到此处")
    .SetMultiFile(true) .SetMaxFiles(5)
    .SetOnFilesSelected(paths => { })  // string[]

// RichTextEditor
ReEndUI.RichTextEditor(parent)
    .SetText("markdown...")
// 方法: .GetMarkdown() → string

// Rating
ReEndUI.Rating(parent, max: 5)
    .SetValue(3.5f) .SetReadOnly(true)
    .SetOnValueChanged(v => { })  // float

// FilterBar
ReEndUI.FilterBar(parent)
    .AddFilter("Tag1") .AddFilter("Tag2")
    .SetOnFiltersChanged(active => { })  // List<string>
```

### 展示 & 布局 (13)

```csharp
// Card
ReEndUI.Card(parent, "title")
    .SetDescription("desc") .SetSelected(true)
    .SetWidth(300) .SetHeight(200)
// ContentArea (RectTransform) 属性可添加子元素

// Avatar
ReEndUI.Avatar(parent)
    .SetImage(sprite) .SetSize(ReEndAvatarSize.Md)
    .SetStatus(ReEndStatus.Online)  // 右下角状态点

// Progress
ReEndUI.Progress(parent, value: 0.5f)
    .SetColor(ReEndProgressColor.Primary)
    .SetIndeterminate(true)  // 不确定进度（持续动画）
    .SetWidth(200)

// Accordion
ReEndUI.Accordion(parent, "title")
    .SetExpanded(false) .SetContentHeight(200)
    .AddChild(childTransform)
    .SetOnToggle(v => { })

// Tabs
ReEndUI.Tabs(parent)
    .SetTabs(new List<string>{"Tab1","Tab2"})
    .SetActiveIndex(0) .SetVariant(ReEndTabsVariant.Underline)
    .SetOnTabChanged(i => { })

// Popover
ReEndUI.Popover(parent, "content").SetContent("text")
// 方法: .ShowAt(screenPos) .Hide()

// Dialog
ReEndUI.Dialog(parent, "title")
    .SetMessage("content") .SetSize(ReEndDialogSize.Md)
    .SetConfirm("确认") .SetCancel("取消")
    .SetOnConfirm(() => { }) .SetOnCancel(() => { })
    .SetOnClose(confirmed => { })
// 方法: .Open()

// Separator
ReEndUI.Separator(parent)
    .SetDirection(ReEndDirection.Horizontal)
    .SetStyle(ReEndSeparatorStyle.Gradient)  // Solid | Gradient | Dashed | Glow | Diamond
    .SetLength(300)

// Tooltip
ReEndUI.Tooltip(parent, "text")
// 方法: .ShowAt(screenPos) .Hide()

// Stat - KPI 统计卡片
ReEndUI.Stat(parent, "label", "value")
    .SetLabel("HP") .SetValue("12,480")
    .SetDelta("12%", positive: true)  // 绿色▲或红色▼

// Table
ReEndUI.Table(parent)
    .SetHeaders(new(){"Name","Value"})
    .AddRow(new(){"A","100"})
    .SetSortable(true)
    .SetOnSort((col, asc) => { })

// List
ReEndUI.List(parent)
    .AddItem("item1") .SetOrdered(true)
    .SetShowDividers(true)
    .SetOnItemClick(i => { })

// Chart
ReEndUI.Chart(parent, ReEndChartType.Bar)
    .AddData(100, "Jan") .AddData(200, "Feb")
    .SetChartType(ReEndChartType.Pie)
```

### 导航 (5)

```csharp
// Timeline
var tl = ReEndUI.Timeline(parent);
tl.AddEntry(new ReEndTimeline.TimelineEntry {
    Date = "2024.01.15", Title = "Event", Description = "detail", Status = ReEndStatus.Success
});

// Stepper
ReEndUI.Stepper(parent, steps: 3)
    .SetCurrent(1)
    .SetLabels(new(){"Step1","Step2","Step3"})
    .SetDirection(ReEndDirection.Horizontal)

// Pagination
ReEndUI.Pagination(parent, total: 10)
    .SetCurrent(1)
    .SetOnPageChanged(page => { })

// Breadcrumb
ReEndUI.Breadcrumb(parent)
    .AddSegment("Home").AddSegment("Docs").AddSegment("API")
    .SetOnSegmentClick(i => { })

// Footer
ReEndUI.Footer(parent)
    .SetBrand("BRAND")
    .AddColumn(new ReEndFooter.FooterColumn { Title = "Links", Links = new(){"A","B"} })
```

### 反馈 (4)

```csharp
// Alert
ReEndUI.Alert(parent, "message")
    .SetSeverity(ReEndSeverity.Warning)  // Info | Success | Warning | Error | Caution | Critical
    .SetDismissible(true)
    .SetOnDismiss(() => { })

// EmptyState
ReEndUI.EmptyState(parent, ReEndEmptyStatePreset.NoData)
    .SetTitle("custom title") .SetDescription("custom desc")
    .SetAction("Refresh", () => { })

// Skeleton
ReEndUI.Skeleton(parent, ReEndSkeletonVariant.Card)  // Line | Text | Avatar | Card

// Toast
ReEndToast.Show("message", ReEndStatus.Success, duration: 3f);
ReEndToast.DismissAll();  // 静态方法，清除全部
```

### 叠层 & 交互 (17)

```csharp
// Dropdown
ReEndUI.Dropdown(parent, "label")
    .AddOption("A").AddOption("B")
    .SetOnValueChanged(v => { })

// ContextMenu
var menu = ReEndUI.ContextMenu(parent);
menu.AddItem("Copy", () => { }).AddSeparator().AddItem("Delete", () => { });
menu.Show(screenPos); menu.Hide();

// CommandPalette
var palette = ReEndUI.CommandPalette();
palette.AddCommand(new ReEndCommandPalette.CommandItem {
    Id = "open", Label = "Open File", Category = "File", OnExecute = () => { }
});
palette.Open(); palette.Close();

// CopyClipboard
ReEndUI.CopyClipboard(parent, "text to copy")

// BottomSheet
ReEndUI.BottomSheet(parent, "title")
    .SetSnapPoint(ReEndSnapPoint.Half)
// 方法: .Show() .Hide()

// Carousel
ReEndUI.Carousel(parent)
    .AddSlide(sprite) .SetAutoPlay(true)
    .SetOnSlideChanged(i => { })

// Resizable
ReEndUI.Resizable(parent)
// 拖拽右下角改变尺寸

// BackToTop
ReEndUI.BackToTop(parent)
// ScrollRect 属性需手动赋值

// ScrollProgress
ReEndUI.ScrollProgress(parent)
// ScrollRect 属性需手动赋值，自动显示滚动进度

// ViewToggle
ReEndUI.ViewToggle(parent)
    .SetOnModeChanged(mode => { })  // ReEndViewMode

// SortControl
ReEndUI.SortControl(parent, "Sort")
    .SetOnDirectionChanged(dir => { })  // ReEndSortDirection

// SpoilerBlock
ReEndUI.SpoilerBlock(parent, "hidden content")
// 点击揭示/隐藏

// ThemeSwitcher
ReEndUI.ThemeSwitcher(parent)
// 自动切换 ReEndTheme-Dark / ReEndTheme-Light

// PullToRefresh
ReEndUI.PullToRefresh(parent)
// ScrollRect 属性需手动赋值，下拉触发 OnRefresh

// SwipeableItem
ReEndUI.SwipeableItem(parent)
    .SetText("item")
    .SetActions("Edit", () => { }, "Delete", () => { })

// SessionTimeoutModal
ReEndUI.SessionTimeoutModal(parent)
    .SetOnContinue(() => { }) .SetOnLogout(() => { })
// 方法: .StartCountdown()

// CookieConsent
ReEndUI.CookieConsent(parent)
    .SetOnAccept(() => { }) .SetOnReject(() => { })
```

### 签名 HUD 组件 (18)

```csharp
// GlitchText - 故障文字效果
ReEndUI.GlitchText(parent, "text")
    .SetFontSize(28)

// DiamondLoader - 菱形旋转加载器
ReEndUI.DiamondLoader(parent)

// TacticalPanel - HUD 战术面板
ReEndUI.TacticalPanel(parent, "title")
    .SetStatus(ReEndStatus.Online)
    .SetWidth(400).SetHeight(280)
// ContentSlot (RectTransform) 属性可添加子元素

// HoloCard - 全息卡片（大数字+发光）
ReEndUI.HoloCard(parent, "title")
    .SetValue("12,480") .SetSubtitle("POWER")

// DataStream - 滚动数据流终端
ReEndUI.DataStream(parent)
    .AddLine("System boot...") .AddLines(list)

// TacticalBadge - 战术状态徽标
ReEndUI.TacticalBadge(parent, "ONLINE")
    .SetStatus(ReEndStatus.Online)

// WarningBanner - 警告横幅
ReEndUI.WarningBanner(parent, "WARNING")
    .SetSeverity(ReEndSeverity.Critical)
    .SetAction("Details", () => { })

// ScanDivider - 扫描线分割符（动画点扫过）
ReEndUI.ScanDivider(parent)

// CoordinateTag - HUD 坐标标签
ReEndUI.CoordinateTag(parent, "SECT")
    .SetCoords("128", "064")

// RadarChart - SVG 风格雷达图
ReEndUI.RadarChart(parent)
    .AddAxis("ATK", 88).AddAxis("DEF", 62).AddAxis("TECH", 95)
    .SetSize(200, 200)

// HUDOverlay - 全屏 HUD 覆盖层（角标+十字准星+坐标）
ReEndUI.HUDOverlay("SYSTEM::ONLINE")
// 自动占满全屏，blockRaycasts=false

// MissionCard - 任务卡片
ReEndUI.MissionCard(parent, "MISSION")
    .SetDescription("objective") .SetProgress(0.6f)
    .SetOnClick(() => { })

// OperatorCard - 干员卡片
ReEndUI.OperatorCard(parent, "NAME")
    .SetRole("Sniper") .SetClass("ELITE")
    .SetPortrait(sprite) .SetStats(atk, def, tech)

// StatusBar - 系统状态栏
ReEndUI.StatusBar(parent)
    .AddIndicator("SYS", ReEndStatus.Online)
    .AddIndicator("NET", ReEndStatus.Warning)

// CommandOutput - 终端命令输出
ReEndUI.CommandOutput(parent)
    .Execute("scan --target", "Scanning... 100%")
    .Clear()

// MatrixGrid - 矩阵网格（动画点阵）
ReEndUI.MatrixGrid(parent, cols: 8, rows: 8)

// FrequencyBars - 频谱柱状图（动画）
ReEndUI.FrequencyBars(parent, barCount: 16)

// TacticalTable - 战术数据表（可选行）
ReEndUI.TacticalTable(parent)
    .SetHeaders(new(){"ID","Status"})
    .AddRow(new(){"OP-01","Active"})
    .SetOnRowSelect(i => { })
```

## 公共方法（所有组件继承自 ReEndBaseComponent）

```csharp
// 尺寸
.SetWidth(float) .SetHeight(float) .SetSize(w, h)
// 命名
.SetName("name")
// 位置（RectTransform anchoredPosition）
.SetAnchoredPos(x, y)
// 锚点
.SetAnchor(min, max) .SetPivot(x, y)
// 父级
.SetParent(transform)
// 主题
.ApplyTheme()  // 强制刷新主题
.RefreshTheme() // 重载主题
.Rebuild()     // 重建子物体结构
// 属性
.RectTransform  // 访问时自动 EnsureBuilt()
.BackgroundImage // Image 组件引用
```

## Theme 定制

```csharp
// 运行时切换
ReEndUI.SetTheme(myThemeAsset);

// 代码覆盖单个属性
var btn = ReEndUI.Button(parent, "text");
btn.Label.color = Color.red;  // 直接访问内部 TMP_Text

// 创建自定义主题
var theme = ScriptableObject.CreateInstance<ReEndTheme>();
theme.primary = Color.red;
ReEndUI.SetTheme(theme);
```

## 模式：动态构建 UI

```csharp
// 声明式
ReEndUI.TacticalPanel(transform, "DASHBOARD")
    .SetStatus(ReEndStatus.Online)
    .SetWidth(500).SetHeight(400);

// 访问内部 Slot 添加子元素
var panel = ReEndUI.TacticalPanel(transform, "DATA");
ReEndUI.Stat(panel.ContentSlot, "HP", "12,480");
ReEndUI.Progress(panel.ContentSlot, 0.7f).SetWidth(300);

// 从 JSON 批量构建（适合 AI Agent）
void BuildFromSpec(string json) {
    foreach (var spec in ParseSpec(json)) {
        var comp = CreateByName(spec.type, spec.parent);
        comp.SetWidth(spec.w).SetHeight(spec.h);
        if (spec.text != null) comp.SetText(spec.text);
        if (spec.action != null) comp.SetOnClick(() => Execute(spec.action));
    }
}
```

## 依赖

| 依赖 | 必需？ | 说明 |
|------|--------|------|
| TextMeshPro | 是 | 所有文字组件 |
| DOTween | 推荐 | 动画（不可用时自动降级为无动画） |
| URP | 是 | Shader 目标管线 |
| Unity 2022.3+ | 推荐 | `FindAnyObjectByType` API |

## 注意事项

- 所有组件在构造时不立即 Build，首次访问 `.RectTransform` 或 `.GameObject` 时自动 Build
- `BuildInternal()` 创建子物体结构，`ApplyTheme()` 应用颜色/字体
- DOTween 不可用时，`ReEndAnimationHelper` 直接设置终态（无动画，不报错）
- HUDOverlay 使用 `CanvasGroup.blocksRaycasts = false`，不阻挡点击
- Toast 自动创建独立 Canvas（sortingOrder=3000, DontDestroyOnLoad）
- `ReEndBootstrap` 在编辑器启动时自动在 `Resources/` 下创建默认 Theme
