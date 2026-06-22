# 三国卡牌游戏 — "能玩"计划

> 目标：从当前状态（框架可用、UI 未接线、核心逻辑有缺口）推进到 **能在编辑器中完整打一局牌** 的状态。

---

## 现状诊断

| 维度 | 现状 | 能玩所需 |
|------|------|----------|
| 回合流程 | `RoundManager` 6 状态循环已实现 ✅ | 无需改动 |
| 牌堆/发牌 | `CardManager` 洗牌+发牌+弃牌堆回收 ✅ | 无需改动 |
| 出牌/响应 | `Player` 主动+响应窗口已实现 ✅ | 需修复 Shan |
| AI | `EnemyAI` + `BasicBattleAI` 规则AI ✅ | 可用但需验证 |
| 角色/血量 | `Player` HP/伤害/治疗 ✅ | 需确保 UI 显示 |
| 卡牌实现 | Sha ✅, Tao ✅, **Shan ❌ 未完成** | 必须修复 |
| 战斗场景 | 存在但不完整（无 Canvas HUD） | 必须搭建 |
| 手牌 UI | 代码已写，**prefab 未创建** | 必须创建 |
| 目标选择 | `BattleTargetButtonView` 代码已有 | 需 prefab + 接线 |
| 卡牌贴图 | `BattleCardSpriteLibrary` 指向 Resources 路径 | **贴图不存在** |
| 胜负判定 | `HandlePlayerDefeated` 已实现 ✅ | 无需改动 |
| 结果反馈 | 伤害/治疗文字 Controller 代码已有 | 需场景接线 |

---

## Phase 0 — 修复核心规则缺口（纯代码，不需要进 Unity 编辑器）

### Step 0.1：补全 Shan（闪）卡逻辑

**文件**：`Assets/Scripts/Card/Cards/Shan.cs`

**问题**：当前 Shan 只重写了 `IsCardResponsible("Sha")` 返回 `true`，但 `DoCardsAction()` 和 `ResponseTrigger()` 没有重写。作为响应牌打出时，它被选中了但不执行任何效果——Sha 的伤害仍然会结算。

**修复方案**：
- Shan 作为响应牌打出时，需要**阻止**触发牌的 `ResponseTrigger()` 结算
- 当前架构中，`Player.EnterResponse()` 的逻辑是：如果玩家选了有效响应牌，则调用 `UseCard()` 打出这张响应牌；如果没选或超时，才调用 `ResponsingCard.ResponseTrigger()`
- 所以 Shan 只需要存在为合法响应即可——打出 Shan 本身就阻止了 `ResponseTrigger()` 被调用
- **验证**：阅读 `Player.DoUseCard()` 确认流程：如果 `card.IsCardResponsible()` 返回 true（表示需要响应），才进入目标的 `EnterResponse()`；目标如果选了合法牌（Shan），则调用 `UseCard()` 打出 Shan，**不会**调用原牌的 `ResponseTrigger()`

**结论**：Shan 的逻辑实际上是隐式正确的（它的存在就阻止了 ResponseTrigger），但缺少 `DoCardsAction()` 的重写，无法播放闪避动画/音效。补全它：

```csharp
// Shan.cs — 补全
public override void DoCardsAction(Player user, Player target)
{
    CacheResolutionContext(user, target);
    // 闪的结算在打出时完成——阻止了杀的 ResponseTrigger
    // 可在此触发闪避音效/特效
}
```

### Step 0.2：验证 BasicBattleAI 响应选择

**文件**：`Assets/Scripts/Character/BasicBattleAI.cs`

**检查**：`TryChooseResponseCard()` 目前只匹配 `Shan` 响应 `Sha`。需确认 `responsingCard.CardType` 返回的是否与 `Sha.CardType` 一致（需读取 `Sha.cs` 的 `CardType` 值）。

### Step 0.3：确保 PlayerManager 正确初始化玩家

**文件**：`Assets/Scripts/Character/PlayerManager.cs`

**检查**：`InitializePlayersForBattle()` 是否创建了一个人类 `Player` + 一个 `EnemyAI`，各自分配了 `Role`。当前只有 `HuangGai` 一个角色——确保两个玩家可以共用或添加第二个角色。

---

## Phase 1 — 创建最小可玩 UI Prefab（需进 Unity 编辑器）

### Step 1.1：创建卡牌贴图资源

**路径**：`Assets/Resources/UI/Card/`

需要 3 张 Sprite（或先用占位纯色）：
- `杀.png` — 杀卡牌面
- `闪.png` — 闪卡牌面  
- `桃.png` — 桃卡牌面
- `CardBack.png` — 卡牌背面

**临时方案**：用纯色方块 + 文字标注作为占位贴图，后续替换正式美术。

### Step 1.2：创建 BattleHandCardView Prefab

**路径**：`Assets/Resources/Prefabs/BattleHandCardView.prefab`

结构：
```
BattleHandCardView (RectTransform, BattleHandCardView 组件)
├── Back (Image — 卡背)
├── Art (Image — 卡面主图)
├── Front (Image — 卡面底板)
│   ├── NameText (TextMeshPro — 卡名)
│   └── DescText (TextMeshPro — 描述)
```

需要挂载的组件：
- `BattleHandCardView` 脚本
- `CanvasGroup`（用于透明度控制）
- `Image`（用于 raycast target）
- 实现 `IPointerEnter/Exit/Down`, `IBeginDrag/IDrag/IEndDrag`

### Step 1.3：创建 BattleTargetButtonView Prefab

**路径**：`Assets/Resources/Prefabs/BattleTargetButtonView.prefab`

结构：
```
BattleTargetButtonView (RectTransform, BattleTargetButtonView 组件)
├── Background (Image — 按钮底图)
├── NameText (TextMeshPro — 角色名)
├── HPText (TextMeshPro — 血量显示)
└── HPBar (Image — 血条填充)
```

### Step 1.4：创建回合/状态提示 UI

最小需要的 HUD 元素：
- 当前回合玩家指示（"你的回合" / "敌方回合"）
- 回合阶段指示（出牌阶段 / 弃牌阶段）
- 确认/取消按钮

---

## Phase 2 — 搭建 BattleScene（Unity 编辑器内操作）

### Step 2.1：Scene 层级搭建

在 `BattleScene.scene` 中建立完整层级：

```
BattleScene
├── Main Camera (CinemachineBrain 或标准 Camera)
├── Directional Light
├── EventSystem
│
├── --- 管理器组 ---
├── GameStart (GamePlaying 脚本 — 入口)
├── RoundSystem (RoundManager)
├── CardsManager (CardManager)
├── PlayerManager (PlayerManager)
├── RoleManager (RoleManager)
├── EventManager (EventManager)
├── HandManager (HandManager3D — 可选，如保留 3D 手牌)
│
├── --- 玩家实体 ---
├── Player_Human (Player 脚本 + Role 子对象)
├── Player_AI (EnemyAI 脚本 + Role 子对象)
│
├── --- 桌面 ---
├── Table (3D 桌面模型，已有)
│
├── --- UI 层 ---
├── UICanvas (Canvas — Screen Space Overlay)
│   ├── BattleUIBootstrap (BattleUIBootstrap 脚本)
│   ├── HandArea (RectTransform — 手牌区容器)
│   │   └── [运行时生成 BattleHandCardView 实例]
│   ├── TargetArea (RectTransform — 目标选择区)
│   │   └── [运行时生成 BattleTargetButtonView 实例]
│   ├── TurnIndicator (TextMeshPro — 回合提示)
│   ├── ActionButtons
│   │   ├── ConfirmButton (Button — 确认出牌)
│   │   └── CancelButton (Button — 取消/跳过)
│   └── PhaseLabel (TextMeshPro — 当前阶段)
│
├── WorldSpaceCanvas (Canvas — World Space，用于伤害/治疗数字)
│   └── [运行时生成 DamageText/HealText]
│
└── NetworkMgr (可选，暂 inactive)
```

### Step 2.2：接线 Inspector 引用

关键接线清单：

| 脚本 | 字段 | 拖入目标 |
|------|------|----------|
| `GamePlaying` | (无特殊引用) | — |
| `CardManager` | `runtimeCardRoot` | 新建空 GameObject 作为卡牌容器 |
| `CardManager` | `discardCardRoot` | 新建空 GameObject 作为弃牌堆 |
| `CardManager` | `deckTemplate` | 配置 6 Sha, 4 Shan, 2 Tao |
| `BattleUIBootstrap` | `presenterParent` | UICanvas 下的 HandArea RectTransform |
| `BattleUIBootstrap` | `handPresenterPrefab` | Step 1.2 创建的 prefab |
| `BattleHandPresenter` | `cardViewPrefab` | BattleHandCardView prefab |
| `BattleHandPresenter` | `targetButtonPrefab` | BattleTargetButtonView prefab |
| `Player` (人类) | `cardContainer` | 卡牌容器 Transform |
| `PlayerManager` | `players[]` | Player_Human + Player_AI |

### Step 2.3：创建玩家实体

**人类玩家**：
```
Player_Human
├── Player 组件
│   ├── initiativeTurnTimeLimit = 30
│   ├── responseTimeLimit = 10
│   └── maxInitiativeActionsPerTurn = 1
└── Role 子对象 (HuangGai)
```

**AI 玩家**：
```
Player_AI
├── EnemyAI 组件
│   ├── initiativeDecisionDelay = 0.75
│   ├── responseDecisionDelay = 0.35
│   └── maxInitiativeActionsPerTurn = 1
└── Role 子对象 (HuangGai 或新建角色)
```

---

## Phase 3 — 修复 UI 与游戏逻辑的对接

### Step 3.1：BattleUIBootstrap.BindBattle() 接线

确保 `BindBattle()` 正确：
1. 查找 `PlayerManager` 中的人类玩家
2. 订阅 `Player.HandChanged` 事件
3. 创建 `BattleHandPresenter` 实例
4. 禁用旧的 3D `HandManager3D`（如果存在）

### Step 3.2：手牌 → UI 同步

**流程**：
1. `Player.GetCard()` / `UseCard()` → 触发 `HandChanged` 事件
2. `BattleHandPresenter` 监听事件 → 重建手牌视图
3. 每张 `BattleHandCardView` 显示卡名、卡面贴图

**需验证**：`BattleHandPresenter` 是否正确订阅了 `HandChanged`，如果没有需添加。

### Step 3.3：目标选择流程

出杀时的流程：
1. 玩家选中一张杀 → 高亮
2. 显示目标按钮（`BattleTargetButtonView`）
3. 玩家点击目标 → 设置 `Player.TargetPlayerEnemyID`
4. 玩家点击确认 → `Player.Confirm_ButtonClick()`
5. 触发 `DoUseCard()` → 目标进入 `EnterResponse()`

**需验证**：`BattleHandPresenter` 的目标选择逻辑是否完整。

### Step 3.4：弃牌阶段 UI

当前 `Player.EnterRefusing()` 自动从末尾丢弃，没有让玩家选择。

**最小可玩方案**（先自动弃牌，后续迭代加 UI 选择）：
- 保持当前自动弃牌逻辑
- 在 UI 上显示"弃牌阶段"文字提示

**后续迭代**：添加手动选牌弃牌功能。

---

## Phase 4 — 端到端验证

### Step 4.1：手动测试用例

在编辑器中按 Play，验证以下流程：

| # | 测试步骤 | 预期结果 |
|---|----------|----------|
| 1 | 进入 BattleScene，自动开始 | 显示双方玩家，各发 4 张牌 |
| 2 | 人类玩家回合 | 显示手牌，提示"出牌阶段" |
| 3 | 点击杀 → 选择目标 → 确认 | 打出杀，目标进入响应 |
| 4 | AI 有闪则出闪 | 伤害被闪避 |
| 5 | AI 无闪则受伤害 | HP 减少，显示伤害数字 |
| 6 | AI 回合 | 自动出牌，有延迟动画 |
| 7 | 桃回血 | HP 恢复，显示治疗数字 |
| 8 | 弃牌阶段 | 手牌超过 HP 上限时自动弃牌 |
| 9 | 一方 HP 归零 | 游戏结束，显示胜负结果 |
| 10 | 牌堆耗尽 | 弃牌堆回收再洗牌 |

### Step 4.2：修复测试中发现的问题

根据 4.1 的测试结果逐项修复。

---

## Phase 5 — 打磨体验（"能玩"之后）

### Step 5.1：音效反馈
- 出杀音效、出闪音效、使用桃音效
- 受伤音效、回血音效
- 回合开始/结束提示音

### Step 5.2：卡牌动画
- 摸牌飞入动画
- 出牌飞出动画
- 弃牌消失动画

### Step 5.3：血条 UI
- 双方 HP 条显示
- HP 变化动画（缓动）

### Step 5.4：更多角色
- 添加 2-3 个新角色（不同 HP、不同技能）
- 角色选择界面

### Step 5.5：更多卡牌
- 实现已定义但未编码的卡牌：无中生有、南蛮入侵、万箭齐发
- 扩充牌堆数量

---

## 执行优先级总结

```
Phase 0 (代码修复)    ████████  必须最先完成，无依赖
  ├─ Step 0.1 补全 Shan       ~30 min
  ├─ Step 0.2 验证 AI 响应     ~15 min
  └─ Step 0.3 验证 PlayerManager ~15 min

Phase 1 (UI Prefab)   ████████  依赖 Phase 0
  ├─ Step 1.1 卡牌贴图         ~30 min
  ├─ Step 1.2 HandCardView Prefab ~1 hr
  ├─ Step 1.3 TargetButtonView Prefab ~45 min
  └─ Step 1.4 回合提示 UI       ~30 min

Phase 2 (场景搭建)    ████████  依赖 Phase 1
  ├─ Step 2.1 Scene 层级        ~1 hr
  ├─ Step 2.2 Inspector 接线    ~1 hr
  └─ Step 2.3 玩家实体          ~30 min

Phase 3 (UI↔逻辑对接) ████████  依赖 Phase 2
  ├─ Step 3.1 Bootstrap 接线   ~1 hr
  ├─ Step 3.2 手牌同步          ~1 hr
  ├─ Step 3.3 目标选择          ~1 hr
  └─ Step 3.4 弃牌阶段          ~30 min

Phase 4 (端到端验证)  ████████  依赖 Phase 3
  └─ Step 4.1 + 4.2            ~2 hr

Phase 5 (打磨)        ████░░░░  "能玩"之后
  └─ 按需迭代

总估算：Phase 0-4 ≈ 10-12 小时有效工作时间
```

---

## 关键风险

| 风险 | 影响 | 缓解 |
|------|------|------|
| `BattleHandPresenter` 代码与实际需求不匹配 | UI 无法正确显示 | Step 3.2 先读代码再对接 |
| `BattleUIBootstrap.EnsureRuntimeUI()` 假设了场景中已有 Canvas | 运行时崩溃 | Step 2.1 确保场景层级正确 |
| 卡牌贴图资源缺失导致 null sprite | 手牌显示空白 | Step 1.1 先用占位贴图 |
| `PlayerManager` 硬编码了玩家数量或角色 | 无法正确初始化 | Step 0.3 验证并修改 |
| 弃牌阶段无 UI 交互 | 玩家无法选择弃哪张 | Phase 3 先自动弃牌，Phase 5 加交互 |

