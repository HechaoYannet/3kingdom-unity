using System;
using UnityEngine;

namespace ReEndUnity
{
    public static class ReEndUI
    {
        public static ReEndTheme Theme => ReEndThemeManager.Current;

        public static void SetTheme(ReEndTheme theme) => ReEndThemeManager.Instance.SetTheme(theme);

        // ── Core Forms & Input ──

        public static ReEndButton Button(Transform parent, string text = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndButton>(parent, text ?? "Button");
            if (text != null) b.SetText(text);
            return b;
        }

        public static ReEndBadge Badge(Transform parent, string text = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndBadge>(parent, text ?? "Badge");
            if (text != null) b.SetText(text);
            return b;
        }

        public static ReEndInput Input(Transform parent, string placeholder = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndInput>(parent, placeholder ?? "Input");
            if (placeholder != null) b.SetPlaceholder(placeholder);
            return b;
        }

        public static ReEndTextarea Textarea(Transform parent, string placeholder = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndTextarea>(parent, placeholder ?? "Textarea");
            if (placeholder != null) b.SetPlaceholder(placeholder);
            return b;
        }

        public static ReEndCheckbox Checkbox(Transform parent, string label = null)
        {
            var b = ReEndBaseComponent.Create<ReEndCheckbox>(parent, label ?? "Checkbox");
            if (label != null) b.SetLabel(label);
            return b;
        }

        public static ReEndRadioGroup RadioGroup(Transform parent)
            => ReEndBaseComponent.Create<ReEndRadioGroup>(parent, "RadioGroup");

        public static ReEndSwitch Switch(Transform parent, string label = null)
        {
            var b = ReEndBaseComponent.Create<ReEndSwitch>(parent, label ?? "Switch");
            if (label != null) b.SetLabel(label);
            return b;
        }

        public static ReEndSelect Select(Transform parent, string placeholder = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndSelect>(parent, placeholder ?? "Select");
            if (placeholder != null) b.SetPlaceholder(placeholder);
            return b;
        }

        // ── Core Display & Layout ──

        public static ReEndCard Card(Transform parent, string title = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndCard>(parent, title ?? "Card");
            if (title != null) b.SetTitle(title);
            return b;
        }

        public static ReEndAvatar Avatar(Transform parent)
            => ReEndBaseComponent.CreateWithImage<ReEndAvatar>(parent, "Avatar");

        public static ReEndProgress Progress(Transform parent, float value = 0)
        {
            var b = ReEndBaseComponent.Create<ReEndProgress>(parent, "Progress");
            b.SetValue(value);
            return b;
        }

        public static ReEndAccordion Accordion(Transform parent, string title = null)
        {
            var b = ReEndBaseComponent.Create<ReEndAccordion>(parent, title ?? "Accordion");
            if (title != null) b.SetTitle(title);
            return b;
        }

        public static ReEndTabs Tabs(Transform parent)
            => ReEndBaseComponent.Create<ReEndTabs>(parent, "Tabs");

        public static ReEndPopover Popover(Transform parent, string content = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndPopover>(parent, "Popover");
            if (content != null) b.SetContent(content);
            return b;
        }

        public static ReEndDialog Dialog(Transform parent, string title = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndDialog>(parent, title ?? "Dialog");
            if (title != null) b.SetTitle(title);
            return b;
        }

        public static ReEndSeparator Separator(Transform parent, ReEndDirection dir = ReEndDirection.Horizontal)
        {
            var b = ReEndBaseComponent.Create<ReEndSeparator>(parent, "Separator");
            b.SetDirection(dir);
            return b;
        }

        public static ReEndTooltip Tooltip(Transform parent, string text = null)
        {
            var b = ReEndBaseComponent.Create<ReEndTooltip>(parent, "Tooltip");
            if (text != null) b.SetText(text);
            return b;
        }

        public static ReEndStat Stat(Transform parent, string label = null, string value = null)
        {
            var b = ReEndBaseComponent.Create<ReEndStat>(parent, label ?? "Stat");
            if (label != null) b.SetLabel(label);
            if (value != null) b.SetValue(value);
            return b;
        }

        // ── Feedback ──

        public static ReEndAlert Alert(Transform parent, string message = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndAlert>(parent, "Alert");
            if (message != null) b.SetMessage(message);
            return b;
        }

        // ── Signature HUD ──

        public static ReEndGlitchText GlitchText(Transform parent, string text = null)
        {
            var b = ReEndBaseComponent.Create<ReEndGlitchText>(parent, text ?? "GlitchText");
            if (text != null) b.SetText(text);
            return b;
        }

        public static ReEndDiamondLoader DiamondLoader(Transform parent)
            => ReEndBaseComponent.CreateWithImage<ReEndDiamondLoader>(parent, "DiamondLoader");

        public static ReEndTacticalPanel TacticalPanel(Transform parent, string title = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndTacticalPanel>(parent, title ?? "TacticalPanel");
            if (title != null) b.SetTitle(title);
            return b;
        }

        public static ReEndHoloCard HoloCard(Transform parent, string title = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndHoloCard>(parent, title ?? "HoloCard");
            if (title != null) b.SetTitle(title);
            return b;
        }

        public static ReEndDataStream DataStream(Transform parent)
            => ReEndBaseComponent.CreateWithImage<ReEndDataStream>(parent, "DataStream");

        public static ReEndTacticalBadge TacticalBadge(Transform parent, string text = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndTacticalBadge>(parent, text ?? "TacticalBadge");
            if (text != null) b.SetText(text);
            return b;
        }

        public static ReEndWarningBanner WarningBanner(Transform parent, string text = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndWarningBanner>(parent, text ?? "WarningBanner");
            if (text != null) b.SetMessage(text);
            return b;
        }

        public static ReEndScanDivider ScanDivider(Transform parent)
            => ReEndBaseComponent.Create<ReEndScanDivider>(parent, "ScanDivider");

        public static ReEndCoordinateTag CoordinateTag(Transform parent, string label = null)
        {
            var b = ReEndBaseComponent.Create<ReEndCoordinateTag>(parent, label ?? "Coord");
            if (label != null) b.SetLabel(label);
            return b;
        }

        public static ReEndHUDOverlay HUDOverlay(string systemLabel = null)
        {
            var b = ReEndBaseComponent.Create<ReEndHUDOverlay>(null, "HUDOverlay");
            if (systemLabel != null) b.SetSystemLabel(systemLabel);
            return b;
        }

        public static ReEndMissionCard MissionCard(Transform parent, string title = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndMissionCard>(parent, title ?? "MissionCard");
            if (title != null) b.SetTitle(title);
            return b;
        }

        public static ReEndOperatorCard OperatorCard(Transform parent, string name = null)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndOperatorCard>(parent, name ?? "OperatorCard");
            if (name != null) b.SetOperatorName(name);
            return b;
        }

        public static ReEndStatusBar StatusBar(Transform parent)
            => ReEndBaseComponent.Create<ReEndStatusBar>(parent, "StatusBar");

        public static ReEndCommandOutput CommandOutput(Transform parent)
            => ReEndBaseComponent.CreateWithImage<ReEndCommandOutput>(parent, "CommandOutput");

        public static ReEndMatrixGrid MatrixGrid(Transform parent, int cols = 8, int rows = 8)
        {
            var b = ReEndBaseComponent.CreateWithImage<ReEndMatrixGrid>(parent, "MatrixGrid");
            b.SetDimensions(cols, rows);
            return b;
        }

        public static ReEndFrequencyBars FrequencyBars(Transform parent, int barCount = 16)
        {
            var b = ReEndBaseComponent.Create<ReEndFrequencyBars>(parent, "FrequencyBars");
            b.SetBarCount(barCount);
            return b;
        }

        // ── Label ──

        public static ReEndLabel Label(Transform parent, string text = null)
        {
            var b = ReEndBaseComponent.Create<ReEndLabel>(parent, text ?? "Label");
            if (text != null) b.SetText(text);
            return b;
        }

        // ── Toast (静态，独立 Canvas) ──

        public static ReEndToast Toast(string message, ReEndStatus status = ReEndStatus.Default)
            => ReEndToast.Show(message, status);
    }
}
