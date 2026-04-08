using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GitConsole
{
    /// <summary>
    /// Git Console — Unity Editor 内置的 Git 操作面板。
    /// 打开方式：菜单 Tools > Git Console
    /// </summary>
    public class GitConsoleWindow : EditorWindow
    {
        // ── Tab 枚举 ──────────────────────────────────────────────────────────
        private enum Tab { Status, Log, Branches, Diff }

        // ── 持久化状态 ─────────────────────────────────────────────────────────
        private Tab   _currentTab     = Tab.Status;
        private string _commitMessage = "";
        private string _newBranchName = "";
        private string _customCommand = "";
        private string _diffTarget    = "HEAD";
        private int    _logCount      = 30;

        // ── 运行时数据 ─────────────────────────────────────────────────────────
        private string _currentBranch  = "";
        private string _statusText     = "";
        private string _logText        = "";
        private string _branchesText   = "";
        private string _diffText       = "";
        private string _consoleOutput  = "";
        private bool   _isBusy         = false;

        // 每个文件的暂存勾选状态 key = 文件路径
        private readonly Dictionary<string, bool> _stageChecks = new Dictionary<string, bool>();
        private List<FileEntry> _statusEntries = new List<FileEntry>();

        // 滚动位置
        private Vector2 _statusScroll;
        private Vector2 _logScroll;
        private Vector2 _branchScroll;
        private Vector2 _diffScroll;
        private Vector2 _consoleScroll;

        // ── 颜色常量 ───────────────────────────────────────────────────────────
        private static readonly Color ColModified  = new Color(1f, 0.85f, 0.3f);
        private static readonly Color ColAdded     = new Color(0.4f, 0.9f, 0.4f);
        private static readonly Color ColDeleted   = new Color(0.9f, 0.35f, 0.35f);
        private static readonly Color ColUntracked = new Color(0.7f, 0.7f, 0.7f);
        private static readonly Color ColStaged    = new Color(0.3f, 0.85f, 1f);

        // ── 文件条目模型 ───────────────────────────────────────────────────────
        private struct FileEntry
        {
            public string XY;   // git status 的两字符标志
            public string Path;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  菜单入口
        // ══════════════════════════════════════════════════════════════════════
        [MenuItem("Tools/Git Console")]
        public static void Open()
        {
            var win = GetWindow<GitConsoleWindow>(false, "Git Console", true);
            win.minSize = new Vector2(520, 480);
            win.Refresh();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  生命周期
        // ══════════════════════════════════════════════════════════════════════
        private void OnEnable()
        {
            Refresh();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  主绘制
        // ══════════════════════════════════════════════════════════════════════
        private void OnGUI()
        {
            DrawTopBar();
            EditorGUILayout.Space(2);
            DrawTabBar();
            EditorGUILayout.Space(2);

            switch (_currentTab)
            {
                case Tab.Status:   DrawStatusTab();   break;
                case Tab.Log:      DrawLogTab();      break;
                case Tab.Branches: DrawBranchesTab(); break;
                case Tab.Diff:     DrawDiffTab();     break;
            }

            EditorGUILayout.Space(4);
            DrawConsoleArea();
            DrawCustomCommandBar();
        }

        // ── 顶部工具栏 ────────────────────────────────────────────────────────
        private void DrawTopBar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label($"Branch: {_currentBranch}", EditorStyles.boldLabel, GUILayout.Width(200));
                GUILayout.FlexibleSpace();

                using (new EditorGUI.DisabledScope(_isBusy))
                {
                    if (GUILayout.Button("↻ Refresh", EditorStyles.toolbarButton, GUILayout.Width(72)))
                        Refresh();

                    if (GUILayout.Button("⬇ Pull", EditorStyles.toolbarButton, GUILayout.Width(58)))
                        RunAndLog("pull");

                    if (GUILayout.Button("⬆ Push", EditorStyles.toolbarButton, GUILayout.Width(58)))
                        RunAndLog("push");

                    if (GUILayout.Button("Fetch", EditorStyles.toolbarButton, GUILayout.Width(48)))
                        RunAndLog("fetch --all");
                }
            }
        }

        // ── Tab 选择栏 ────────────────────────────────────────────────────────
        private void DrawTabBar()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawTabButton(Tab.Status,   "Status");
                DrawTabButton(Tab.Log,      "Log");
                DrawTabButton(Tab.Branches, "Branches");
                DrawTabButton(Tab.Diff,     "Diff");
            }
        }

        private void DrawTabButton(Tab tab, string label)
        {
            bool isActive = _currentTab == tab;
            var style     = isActive ? EditorStyles.miniButtonMid : EditorStyles.miniButton;
            if (isActive) GUI.backgroundColor = new Color(0.5f, 0.8f, 1f);
            if (GUILayout.Button(label, style)) _currentTab = tab;
            GUI.backgroundColor = Color.white;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Status Tab
        // ══════════════════════════════════════════════════════════════════════
        private void DrawStatusTab()
        {
            if (_statusEntries.Count == 0)
            {
                EditorGUILayout.HelpBox("工作区干净，没有待提交的文件。", MessageType.Info);
            }
            else
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("全选", EditorStyles.miniButton, GUILayout.Width(50)))
                        SetAllChecks(true);
                    if (GUILayout.Button("全不选", EditorStyles.miniButton, GUILayout.Width(55)))
                        SetAllChecks(false);
                    if (GUILayout.Button("Stage 已勾选", EditorStyles.miniButton, GUILayout.Width(90)))
                        StageChecked();
                    if (GUILayout.Button("Unstage All", EditorStyles.miniButton, GUILayout.Width(85)))
                        RunAndLog("restore --staged .");
                }

                EditorGUILayout.Space(2);

                _statusScroll = EditorGUILayout.BeginScrollView(_statusScroll, GUILayout.Height(200));
                foreach (var entry in _statusEntries)
                {
                    DrawFileRow(entry);
                }
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.Space(4);
            DrawCommitArea();
        }

        private void DrawFileRow(FileEntry entry)
        {
            if (!_stageChecks.ContainsKey(entry.Path))
                _stageChecks[entry.Path] = false;

            Color rowColor = GetEntryColor(entry.XY);
            var origColor  = GUI.contentColor;
            GUI.contentColor = rowColor;

            using (new EditorGUILayout.HorizontalScope())
            {
                _stageChecks[entry.Path] = EditorGUILayout.Toggle(_stageChecks[entry.Path], GUILayout.Width(18));
                GUILayout.Label($"[{entry.XY}]", GUILayout.Width(34));
                GUILayout.Label(entry.Path);

                GUI.contentColor = Color.white;
                if (GUILayout.Button("Stage",   EditorStyles.miniButton, GUILayout.Width(48)))
                    RunAndLog($"add \"{entry.Path}\"");
                if (GUILayout.Button("Discard", EditorStyles.miniButton, GUILayout.Width(56)))
                {
                    if (EditorUtility.DisplayDialog("确认", $"丢弃对 {entry.Path} 的修改？", "确认", "取消"))
                        RunAndLog($"checkout -- \"{entry.Path}\"");
                }
            }

            GUI.contentColor = origColor;
        }

        private void DrawCommitArea()
        {
            EditorGUILayout.LabelField("Commit Message", EditorStyles.boldLabel);
            _commitMessage = EditorGUILayout.TextArea(_commitMessage, GUILayout.Height(54));

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(_isBusy || string.IsNullOrWhiteSpace(_commitMessage)))
                {
                    if (GUILayout.Button("✔ Commit", GUILayout.Height(26)))
                    {
                        string msg = _commitMessage.Replace("\"", "\\\"");
                        RunAndLog($"commit -m \"{msg}\"");
                        _commitMessage = "";
                    }
                    if (GUILayout.Button("Commit & Push", GUILayout.Height(26)))
                    {
                        string msg = _commitMessage.Replace("\"", "\\\"");
                        RunAndLog($"commit -m \"{msg}\"");
                        RunAndLog("push");
                        _commitMessage = "";
                    }
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Log Tab
        // ══════════════════════════════════════════════════════════════════════
        private void DrawLogTab()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("最近提交数量:", GUILayout.Width(90));
                _logCount = EditorGUILayout.IntSlider(_logCount, 5, 100, GUILayout.Width(180));
                if (GUILayout.Button("刷新 Log", EditorStyles.miniButton, GUILayout.Width(70)))
                    RefreshLog();
            }

            EditorGUILayout.Space(2);
            _logScroll = EditorGUILayout.BeginScrollView(_logScroll);
            EditorGUILayout.TextArea(_logText, EditorStyles.label);
            EditorGUILayout.EndScrollView();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Branches Tab
        // ══════════════════════════════════════════════════════════════════════
        private void DrawBranchesTab()
        {
            EditorGUILayout.LabelField("分支列表（* 为当前分支）", EditorStyles.boldLabel);
            _branchScroll = EditorGUILayout.BeginScrollView(_branchScroll, GUILayout.Height(150));
            DrawBranchLines();
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("新建 / 切换分支", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                _newBranchName = EditorGUILayout.TextField("Branch Name:", _newBranchName);
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(string.IsNullOrWhiteSpace(_newBranchName) || _isBusy))
                {
                    if (GUILayout.Button("Checkout -b (新建并切换)"))
                    {
                        string name = _newBranchName.Trim();
                        if (IsValidBranchName(name))
                            RunAndLog($"checkout -b \"{name}\"");
                        else
                            EditorUtility.DisplayDialog("非法分支名", "分支名含有非法字符，请重新输入。", "OK");
                    }
                    if (GUILayout.Button("Checkout (切换)"))
                    {
                        string name = _newBranchName.Trim();
                        if (IsValidBranchName(name))
                            RunAndLog($"checkout \"{name}\"");
                        else
                            EditorUtility.DisplayDialog("非法分支名", "分支名含有非法字符，请重新输入。", "OK");
                    }
                    if (GUILayout.Button("删除分支"))
                    {
                        string name = _newBranchName.Trim();
                        if (IsValidBranchName(name) &&
                            EditorUtility.DisplayDialog("确认", $"删除分支 {name}？", "确认", "取消"))
                            RunAndLog($"branch -d \"{name}\"");
                    }
                }
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Stash 操作", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Stash Push"))  RunAndLog("stash push");
                if (GUILayout.Button("Stash Pop"))   RunAndLog("stash pop");
                if (GUILayout.Button("Stash List"))  AppendConsole(GitRunner.Run("stash list").Output);
            }
        }

        private void DrawBranchLines()
        {
            if (string.IsNullOrEmpty(_branchesText)) return;
            foreach (var line in _branchesText.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                bool isCurrent = line.TrimStart().StartsWith("*");
                var orig = GUI.contentColor;
                if (isCurrent) GUI.contentColor = ColAdded;
                EditorGUILayout.LabelField(line);
                GUI.contentColor = orig;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Diff Tab
        // ══════════════════════════════════════════════════════════════════════
        private void DrawDiffTab()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                _diffTarget = EditorGUILayout.TextField("Diff 目标（留空=工作区）:", _diffTarget, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("刷新 Diff", EditorStyles.miniButton, GUILayout.Width(70)))
                    RefreshDiff();
            }
            EditorGUILayout.Space(2);
            _diffScroll = EditorGUILayout.BeginScrollView(_diffScroll);
            DrawColoredDiff(_diffText);
            EditorGUILayout.EndScrollView();
        }

        private void DrawColoredDiff(string diff)
        {
            if (string.IsNullOrEmpty(diff))
            {
                EditorGUILayout.LabelField("（无差异）");
                return;
            }

            foreach (var line in diff.Split('\n'))
            {
                var orig = GUI.contentColor;
                if (line.StartsWith("+") && !line.StartsWith("+++"))
                    GUI.contentColor = ColAdded;
                else if (line.StartsWith("-") && !line.StartsWith("---"))
                    GUI.contentColor = ColDeleted;
                else if (line.StartsWith("@@"))
                    GUI.contentColor = ColModified;

                EditorGUILayout.LabelField(line, EditorStyles.label);
                GUI.contentColor = orig;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  底部：Console 输出 & 自定义命令
        // ══════════════════════════════════════════════════════════════════════
        private void DrawConsoleArea()
        {
            EditorGUILayout.LabelField("Console Output", EditorStyles.boldLabel);
            _consoleScroll = EditorGUILayout.BeginScrollView(_consoleScroll, GUILayout.Height(90));
            EditorGUILayout.TextArea(_consoleOutput, EditorStyles.label, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("清空 Console", EditorStyles.miniButton, GUILayout.Width(90)))
                _consoleOutput = "";
        }

        private void DrawCustomCommandBar()
        {
            EditorGUILayout.Space(2);
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("git", GUILayout.Width(24));
                _customCommand = EditorGUILayout.TextField(_customCommand);
                using (new EditorGUI.DisabledScope(_isBusy || string.IsNullOrWhiteSpace(_customCommand)))
                {
                    if (GUILayout.Button("执行", EditorStyles.toolbarButton, GUILayout.Width(40)))
                    {
                        string cmd = _customCommand.Trim();
                        if (EditorUtility.DisplayDialog("确认执行", $"即将执行：\ngit {cmd}", "确认", "取消"))
                        {
                            RunAndLog(cmd);
                            _customCommand = "";
                        }
                    }
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  数据刷新
        // ══════════════════════════════════════════════════════════════════════
        private void Refresh()
        {
            RefreshBranch();
            RefreshStatus();
            RefreshLog();
            RefreshBranches();
            RefreshDiff();
        }

        private void RefreshBranch()
        {
            var r = GitRunner.Run("rev-parse --abbrev-ref HEAD");
            _currentBranch = r.Success ? r.Output.Trim() : "（未知）";
        }

        private void RefreshStatus()
        {
            var r = GitRunner.Run("status --porcelain");
            _statusText    = r.Output;
            _statusEntries = ParsePorcelain(r.Output);

            // 清理已不存在文件的勾选状态
            var existingPaths = new HashSet<string>();
            foreach (var e in _statusEntries) existingPaths.Add(e.Path);
            var toRemove = new List<string>();
            foreach (var key in _stageChecks.Keys)
                if (!existingPaths.Contains(key)) toRemove.Add(key);
            foreach (var key in toRemove) _stageChecks.Remove(key);
        }

        private void RefreshLog()
        {
            var r = GitRunner.Run($"log --oneline --decorate --graph -{_logCount}");
            _logText = r.Success ? r.Output : r.Error;
        }

        private void RefreshBranches()
        {
            var r = GitRunner.Run("branch -a");
            _branchesText = r.Success ? r.Output : r.Error;
        }

        private void RefreshDiff()
        {
            string target = (_diffTarget ?? "").Trim();
            // 拒绝含有 shell 危险字符的 diff target
            char[] forbidden = { '"', '\'', '`', '$', '\\', '|', '&', ';', '<', '>', '(', ')', '{', '}', '!', '\n', '\r' };
            if (!string.IsNullOrEmpty(target) && target.IndexOfAny(forbidden) >= 0)
            {
                _diffText = "[err] diff 目标含有非法字符，已拒绝执行。";
                return;
            }
            string args = string.IsNullOrEmpty(target) ? "diff" : $"diff {target}";
            var r = GitRunner.Run(args);
            _diffText = r.Success ? r.Output : r.Error;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  辅助方法
        // ══════════════════════════════════════════════════════════════════════
        private void RunAndLog(string gitArgs)
        {
            _isBusy = true;
            try
            {
                AppendConsole($"> git {gitArgs}");
                var r = GitRunner.Run(gitArgs);
                if (!string.IsNullOrEmpty(r.Output)) AppendConsole(r.Output);
                if (!string.IsNullOrEmpty(r.Error))  AppendConsole("[err] " + r.Error);
            }
            catch (Exception ex)
            {
                AppendConsole("[exception] " + ex.Message);
            }
            finally
            {
                _isBusy = false;
            }
            Refresh();
            Repaint();
        }

        private void AppendConsole(string text)
        {
            _consoleOutput = text + "\n" + _consoleOutput;
            // 最多保留 4000 个字符
            if (_consoleOutput.Length > 4000)
                _consoleOutput = _consoleOutput.Substring(0, 4000);
        }

        private void StageChecked()
        {
            var toStage = new List<string>();
            foreach (var kv in _stageChecks)
            {
                if (kv.Value) toStage.Add(kv.Key);
            }

            if (toStage.Count == 0) return;

            // 一次性 add 所有选中文件
            string allPaths = string.Join(" ", toStage.ConvertAll(p => $"\"{p}\""));
            RunAndLog($"add {allPaths}");
            SetAllChecks(false);
        }

        private void SetAllChecks(bool value)
        {
            foreach (var entry in _statusEntries)
                _stageChecks[entry.Path] = value;
        }

        private static bool IsValidBranchName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            // 拒绝包含 shell 危险字符
            char[] forbidden = { '"', '\'', '`', '$', '\\', '|', '&', ';', '<', '>', '(', ')', '{', '}', '!', '\n', '\r' };
            return name.IndexOfAny(forbidden) < 0;
        }

        // 修复：实现缺失的 GetEntryColor 方法（之前是一个意外的裸代码块）
        private static Color GetEntryColor(string xy)
        {
            if (string.IsNullOrEmpty(xy)) return Color.white;
            char x = xy[0];
            char y = xy.Length > 1 ? xy[1] : ' ';

            // 已暂存（X 字段非空且不是 ?）
            if (x != ' ' && x != '?') return ColStaged;
            // 工作区变更
            if (y == 'M') return ColModified;
            if (y == 'D') return ColDeleted;
            // 未追踪
            if (x == '?' || y == '?') return ColUntracked;
            // 其他视为新增
            return ColAdded;
        }

        private static List<FileEntry> ParsePorcelain(string output)
        {
            var list = new List<FileEntry>();
            if (string.IsNullOrEmpty(output)) return list;

            foreach (var line in output.Split('\n'))
            {
                if (line.Length < 4) continue;
                string xy   = line.Substring(0, 2);
                string path = line.Substring(3).Trim();
                // 处理重命名 "old -> new"
                int arrow = path.IndexOf(" -> ", StringComparison.Ordinal);
                if (arrow >= 0) path = path.Substring(arrow + 4);
                list.Add(new FileEntry { XY = xy, Path = path });
            }
            return list;
        }
    }
}
