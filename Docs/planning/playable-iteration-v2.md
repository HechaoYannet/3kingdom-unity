# 三国卡牌 — 完整"能玩"迭代计划 v2

> 当前状态：手牌显示正常，但玩家看不到 AI、回合提示、血量、目标选择无指引、无胜负画面。
> 目标：让游戏**一目了然** — 始终知道谁在出牌、什么阶段、能否行动、结果如何。

---

## 第一部分：已有问题诊断

### 当前玩家看到的画面

| 元素 | 当前状态 | 问题 |
|------|---------|------|
| 背景 | 古战场静态图片 | ✅ 有 |
| 手牌 | 4 张红牌扇形排列 | ✅ 有，但全是红色方块（缺美术） |
| 目标按钮 | **不显示**（B1） | ❌ 只在选杀时出现，且无提示 |
| AI 敌人 | **完全不可见**（B2） | ❌ 无角色形象、无名字、无血量 |
| 回合提示 | **无**（B3） | ❌ 不知道谁的回合 |
| 阶段提示 | **无**（B4） | ❌ 不知道该出牌还是该弃牌 |
| 血量显示 | **无**（B5） | ❌ 不知道自己和敌人剩多少血 |
| 响应提示 | **无**（B6） | ❌ AI 出杀时玩家不知道要出闪 |
| 弃牌交互 | 自动从末尾弃（B7） | ⚠️ 玩家不能选择弃哪张 |
| 胜负画面 | **无**（B8） | ❌ HP 归零后只打印 Log |
| 卡牌美术 | 纯色占位块 | ⚠️ Tuanjie 内置 sprite 不可用 |

---

## 第二部分：已知限制解决方案

### B1 — 目标按钮不显示

**根因**：
1. `BattleHandPresenter.ShouldShowTargetSelection()` 只在**选中了杀（Sha）且不处于响应模式**时返回 true
2. 玩家不知道"先点杀、再选目标"这个流程
3. 即使显示，目标按钮在 TargetArea 中，但没有文字提示说明

**解决方案**：
- [A] 在手牌区上方加一个永久显示的提示文字："选择一张手牌出牌或向上拖拽"
- [B] 目标区始终显示"选择目标"标题（即使无目标可选也显示灰色提示）
- [C] 给可出牌（杀/桃）加视觉高亮，不可出的牌加灰色压暗

### B2 — AI 不可见

**根因**：`PlayerManager.EnsureRuntimeEnemy()` 创建的是一个空 GameObject（只有 EnemyAI 脚本），没有任何 Mesh/Sprite。

**解决方案**：
- [A] 在 BattleScene 中创建一个 EnemySpot 3D 占位对象（Cube/Plane + 材质 + 文字标签）
- [B] 在 Canvas(Screen) 上方创建一个 EnemyInfoPanel（名字、HP 条、血条）
- [C] EnemyInfoPanel 始终可见

### B3/B4 — 回合/阶段提示缺失

**根因**：场景中没有 TurnIndicator 实例；BattleHandPresenter 的 guideText 只在拖拽时显示。

**解决方案**：
- [A] 创建一个简单的 `BattlePhaseHUD` MonoBehaviour
  - 监听 `RoundManager.RoundState` 和 `RoundManager.CurrentTurnPlayer`
  - 显示"你的回合 — 出牌阶段" / "敌方回合 — 等待中"
  - 显示当前阶段名（Preparing/Checking/GettingCard/Battling/RefusingCard/Ending）
  - 放在 Canvas(Screen) 顶部居中

### B5 — 血量显示缺失

**根因**：没有 HP 条 UI 组件被创建或更新。

**解决方案**：
- [A] 在 EnemyInfoPanel 和 PlayerInfoPanel 中各加一个 HP Bar
  - 监听 `Player` 的 HP 变化事件
  - 显示 "HP: 3/4" + 血条
- [B] 创建 `BattlePlayerHUD` 组件挂到玩家对象上

### B6 — 响应提示缺失

**根因**：当 `EnterResponse()` 被调用时，没有 UI 提示。

**解决方案**：
- [A] PhaseHUD 检测到 `IsResponseInputActive` 时显示"请出闪响应"红色大字
- [B] 可响应的手牌（闪）显示绿色闪烁高亮

### B7 — 弃牌无交互

**根因**：`Player.EnterRefusing()` 自动 `DiscardCardFromHand(currentCards.Count - 1)`。

**解决方案**：
- [A] 修改 `EnterRefusing()`，当 `UsesHumanInput == true` 时暂停等待玩家选择
- [B] 弃牌模式下，PhaseHUD 显示"请弃置 X 张牌"
- [C] 玩家点击手牌 → 弃掉该牌 → 重复直到手牌 ≤ HP

### B8 — 无胜负画面

**根因**：`GamePlaying.GameOver()` 只设置 state 和打印 Log。

**解决方案**：
- [A] 创建 `BattleResultPanel` — 半透明遮罩 + 胜利/失败大字 + 重新开始按钮
- [B] 监听 `GameState.Over` → 显示面板

---

## 第三部分：完整迭代分步计划

### Sprint A：让战斗"看得见"

#### A1 — 创建 BattlePhaseHUD（~45min）
**新文件**：`Assets/Scripts/UI/UICompnent/BattlePhaseHUD.cs`

**功能**：
- 两个 TMP 文字：`phaseLabel`（当前阶段名）、`turnLabel`（谁的回合）
- 响应提示：`responseLabel`（"出闪" 红色闪烁）
- 每帧轮询 `RoundManager.instance.RoundState` 和 `player.IsResponseInputActive`

**场景接线**：
- 在 Canvas(Screen) 顶部添加 `PhaseHUD` GameObject
- TMP：`PhaseLabel`、`TurnLabel`、`ResponsePrompt`

#### A2 — 创建 BattlePlayerHUD（~1h）
**新文件**：`Assets/Scripts/UI/UICompnent/BattlePlayerHUD.cs`

**功能**：
- HP 条（Slider + TMP "3/4"）
- 角色名显示
- 订阅 `Player.HandChanged` 事件刷新 HP

**场景接线**：
- 在 TargetArea 上方添加 `EnemyHUD`（敌方信息区）
- 在 HandArea 下方添加 `PlayerHUD`（己方信息区）
- BattleUIBootstrap.BindBattle() 注入 Player 引用

#### A3 — 创建 AI 3D 占位体（~30min）
- 在 BattleScene 中创建一个 EnemySpot：
  - `EnemyCharacter(TEMP)` — Cube（桌面敌方位置）+ 材质（红色）
  - 上挂 EnemyAI 组件（替换自动创建的 RuntimeEnemyAI）
  - 添加 `EnemyHUDAnchor` — 空 Transform，标记血条世界位置

#### A4 — 修复PlayerManager使用场景AI而非自动创建（~15min）
- 在 `PlayerManager` 的 Inspector 中拖入 `EnemyCharacter(TEMP)` 到 `runtimeEnemyPlayer`
- 同样挂在 `players` 列表中
- 确保 `EnsureRuntimeEnemy()` 返回已有对象

---

### Sprint B：让交互"有反馈"

#### B5 — 创建 BattleResultPanel（~45min）
**新文件**：`Assets/Scripts/UI/UICompnent/BattleResultPanel.cs`

**功能**：
- 半透明黑色遮罩（Image + CanvasGroup）
- 居中大字：胜利/失败
- 重新开始按钮 → `UnityEngine.SceneManagement.SceneManager.LoadScene`
- `Show(GameResult)` 方法

**场景接线**：
- 在 Canvas(Screen) 底部添加（高 z-order）
- BattleUIBootstrap 中注入引用

#### B6 — 修复弃牌交互（~1h）
**修改文件**：`Assets/Scripts/Character/Player.cs`

**改动**：
- `EnterRefusing()`:
  ```csharp
  if (UsesHumanInput) {
      isInRefusingPhase = true;
      PhaseHUD.SetRefusingPrompt(需要弃牌数);
      yield return new WaitWhile(() => currentCards.Count > maxCards);
      isInRefusingPhase = false;
  }
  ```
- 在 `NotifyCardPressed`（BattleHandPresenter）中：
  ```csharp
  if (player.isInRefusingPhase) { player.DiscardCardFromHand(cardIndex); return; }
  ```

#### B7 — 添加出牌指引文字（~30min）
**修改文件**：`Assets/Scripts/UI/UIFramework/BattleHandPresenter.cs`

- 在手牌区上方始终显示 `permanentHintText`："选择一张牌出牌 或 向上拖拽"
- 目标区域始终显示 `targetTitleText`
- 无目标选择时显示灰色"请先选择可攻击的牌"

---

### Sprint C：让画面"有血有肉"

#### C8 — 替换卡牌贴图为已有资源（~30min）
- 项目已有杀/闪/桃贴图在 `Assets/Resources/UI/Card/`
- 验证 `BattleCardSpriteLibrary` 映射正确加载
- 确保每张手牌视图加载到正版贴图

#### C9 — 修复Tuanjie内置Sprite不可用（~30min）
**问题**：`Resources.GetBuiltinResource("UI/Skin/UISprite.psd")` 在 Tuanjie 中返回 null

**解决方案**：
- 移除所有 `GetBuiltinResource` 调用
- 替换为 `CreateProceduralSprite()` — 运行时用代码生成纯色 Sprite
- 修改 `BattleHandCardView.BuildVisualTree()` 和 `CreateLayer()`
- 修改 `BattleTargetButtonView.EnsureRuntimeReferences()`

#### C10 — 添加世界空间伤害/治疗数字（~45min）
**已有代码**：`WorldSpaceUIManager.CreateDamageText()` / `CreateHealText()`

**需要**：
- 将 Canvas(Space) 改为 World Space 模式
- 在 Sha.ResponseTrigger() 中调用：`UIManager.Instance.GetWorldSpaceManager().CreateDamageText(1, target.transform.position)`
- 在 Tao.DoCardsAction() 中调用治疗文字
- 确保 UIManager 在场景中存在

---

### Sprint D：验证 & 打磨

#### D11 — 端到端测试（~30min）
逐项验证：
1. ✅ 启动 → 看到双方 HP 条 + 回合提示
2. ✅ 人类回合 → 看到"你的回合·出牌阶段"
3. ✅ 点杀 → 目标按钮出现 → 点目标 → 杀打出
4. ✅ AI 被攻击 → 看到红方块 AI + HP 减少
5. ✅ AI 出杀 → 看到"请出闪"提示
6. ✅ 点闪 → 闪避成功 → 伤害取消
7. ✅ 点桃（受伤时） → HP 恢复
8. ✅ 弃牌阶段 → 看到提示 → 点牌弃掉
9. ✅ HP 归零 → 胜负画面弹出
10. ✅ 牌堆耗尽 → 自动回收洗牌

#### D12 — 修复 D11 中发现的问题（~1h）

---

## 执行优先级矩阵

```
高影响 / 低成本（优先做）
  ├─ A1 BattlePhaseHUD         — 立刻知道在干什么
  ├─ A3 AI 3D占位体            — 能看到敌人
  ├─ B5 BattleResultPanel      — 有胜负感
  ├─ C8 卡牌贴图替换           — 不再是纯色方块
  └─ C9 修复Tuanjie sprite     — 消除所有 GetBuiltinResource 错误

高影响 / 中等成本
  ├─ A2 BattlePlayerHUD        — 看到血量
  ├─ B6 弃牌交互               — 可控制弃哪张
  └─ B7 出牌指引               — 知道怎么操作

中等影响 / 中等成本
  ├─ A4 PlayerManager接线      — 场景AI而非自动创建
  ├─ C10 伤害/治疗数字         — 视觉反馈
  └─ D11/D12 验证修复          — 质量保障
```

---

## 需要创建的新文件

| 文件 | 用途 |
|------|------|
| `Assets/Scripts/UI/UICompnent/BattlePhaseHUD.cs` | 回合/阶段/响应提示 |
| `Assets/Scripts/UI/UICompnent/BattlePlayerHUD.cs` | 玩家/AI HP 条 + 角色名 |
| `Assets/Scripts/UI/UICompnent/BattleResultPanel.cs` | 胜负画面 |

## 需要修改的现有文件

| 文件 | 改动 |
|------|------|
| `Player.cs` | EnterRefusing — 人类玩家暂停等待手动弃牌 |
| `BattleHandPresenter.cs` | 永久提示文字 + 弃牌模式支持 |
| `BattleUIBootstrap.cs` | 绑定 PhaseHUD、PlayerHUD、ResultPanel |
| `BattleHandCardView.cs` | 替换 GetBuiltinResource → 程序化生成 sprite |
| `BattleTargetButtonView.cs` | 同上 |
| `Sha.cs` | 触发伤害数字 |
| `Tao.cs` | 触发治疗数字 |
| `PlayerManager.cs` | 支持场景 AI |
| `BattleSceneSetup.cs` | 创建 PhaseHUD/PlayerHUD/ResultPanel/EnemySpot |

---

## 总估算

| Sprint | 内容 | 时间 |
|--------|------|------|
| A | 创建 HUD + AI 占位 | ~2.5h |
| B | 反馈交互 | ~2.25h |
| C | 美术资源 | ~1.75h |
| D | 验证修复 | ~1.5h |
| **合计** | | **~8h** |
