# Git Console — Unity Editor Git 操作面板

> 文件路径：`Assets/Editor/GitConsole/`  
> 打开方式：Unity 菜单 **Tools › Git Console**

---

## 功能概览

| 功能区 | 说明 |
|--------|------|
| 顶部工具栏 | 显示当前分支；一键 Refresh / Pull / Push / Fetch |
| **Status Tab** | 查看工作区与暂存区状态，逐文件 Stage / Discard，批量 Stage，填写提交信息并 Commit / Commit & Push |
| **Log Tab** | 可视化 `git log --graph` 提交历史，支持自定义显示数量 |
| **Branches Tab** | 列出所有本地与远端分支，新建 / 切换 / 删除分支，Stash Push / Pop / List |
| **Diff Tab** | 查看工作区或任意两个 ref 之间的差异，带颜色高亮 |
| Console 输出区 | 实时显示所有已执行命令及其输出 |
| 自定义命令栏 | 输入任意 git 子命令（不含 `git` 前缀）直接执行 |

---

## 安装要求

- Unity 2020.3 或更高版本
- 本机已安装 `git` 并位于系统 `PATH`
- 脚本放置于 `Assets/Editor/` 路径下（编辑器脚本，不会打包进运行时）

---

## 快速上手

1. 在 Unity 菜单点击 **Tools › Git Console**，窗口会自动弹出并刷新当前仓库状态。
2. **查看改动**：切换到 *Status* Tab，所有已修改/新增/删除的文件会按颜色分类列出：
   - 🔵 蓝色 — 已暂存（Staged）
   - 🟡 黄色 — 已修改（Modified）
   - 🟢 绿色 — 新增（Added/Untracked added）
   - 🔴 红色 — 已删除（Deleted）
   - ⬜ 灰色 — 未追踪（Untracked）
3. **暂存文件**：勾选文件旁的复选框 → 点击 **Stage 已勾选**；或直接点击每行的 **Stage** 按钮。
4. **提交**：在 *Commit Message* 文本框输入提交信息 → 点击 **✔ Commit**，或一步完成 **Commit & Push**。
5. **查看历史**：切换到 *Log* Tab，拖动滑块调整条数，点击 **刷新 Log**。
6. **切换分支**：切换到 *Branches* Tab，在输入框填写分支名，选择 *Checkout -b*（新建并切换）或 *Checkout*（切换已有分支）。
7. **自定义命令**：在底部输入框输入任意 git 参数（例如 `remote -v`）并点击 **执行**，结果显示在 Console 区。

---

## 文件结构

```
Assets/
└── Editor/
    └── GitConsole/
        ├── GitConsoleWindow.cs   # EditorWindow 主体 UI 逻辑
        └── GitRunner.cs          # 封装 System.Diagnostics.Process 调用 git
```

### GitRunner.cs

提供静态方法 `GitRunner.Run(string arguments) → Result`：

```csharp
var r = GitRunner.Run("status --porcelain");
if (r.Success)
    Debug.Log(r.Output);
else
    Debug.LogError(r.Error);
```

- 工作目录自动设为 `Application.dataPath` 的上级（即项目根目录）。
- 超时时间为 30 秒。
- 编码设置为 UTF-8，中文路径不乱码。

### GitConsoleWindow.cs

继承自 `EditorWindow`，通过 `[MenuItem("Tools/Git Console")]` 注册菜单入口。  
内部通过调用 `GitRunner.Run()` 执行所有 git 命令，结果统一输出到 Console 区，并在每次操作后自动调用 `Refresh()` 刷新界面数据。

---

## 支持的 Git 操作一览

| 操作 | 对应 git 命令 |
|------|---------------|
| 刷新状态 | `git status --porcelain` |
| 拉取 | `git pull` |
| 推送 | `git push` |
| 抓取所有远端 | `git fetch --all` |
| 暂存单文件 | `git add "<path>"` |
| 批量暂存 | 对勾选文件依次执行 `git add` |
| 取消全部暂存 | `git restore --staged .` |
| 丢弃修改 | `git checkout -- "<path>"` |
| 提交 | `git commit -m "<message>"` |
| 查看日志 | `git log --oneline --decorate --graph -N` |
| 列出分支 | `git branch -a` |
| 新建并切换分支 | `git checkout -b "<branch>"` |
| 切换分支 | `git checkout "<branch>"` |
| 删除分支 | `git branch -d "<branch>"` |
| Stash 推入 | `git stash push` |
| Stash 弹出 | `git stash pop` |
| 列出 Stash | `git stash list` |
| 差异查看 | `git diff [target]` |
| 自定义命令 | `git <自定义参数>` |

---

## 已知限制

- 不支持 SSH 密钥弹窗或需要 GUI 交互的鉴权方式（建议使用 SSH Agent 或凭据管理器）。
- Diff 使用纯文本显示，适合快速查阅；大文件差异可能导致滚动卡顿。
- 每次操作为同步阻塞（最长 30 秒），网络操作（Pull/Push）建议在网络稳定时使用。

---

## 扩展建议

- 可在 `GitRunner.Run` 中增加 `async/await` 支持，避免编辑器卡顿。
- 可增加 `git rebase`、`git merge`、`git cherry-pick` 等高级操作按钮。
- 可集成 `git blame` 功能，在 Inspector 中显示文件的逐行作者信息。
