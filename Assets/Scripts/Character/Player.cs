using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗中的基础玩家实体，负责手牌、体力和回合内规则动作。
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private float initiativeTurnTimeLimit = 30f;
    [SerializeField] private float responseTimeLimit = 10f;
    [SerializeField] protected int maxInitiativeActionsPerTurn = 1;

    public int PlayerID { get; protected set; }
    public Role CurrentRole { get; protected set; }
    public int CurrentHP { get; protected set; }
    public bool IsSubRound { get; set; }
    public int currSelectedCardID = -1;
    public List<Card> currentCards = new List<Card>();
    public List<Card> checkingCards = new List<Card>();
    public Card ResponsingCard { get; set; }
    public UIOperation PlayerUIOperation { get; protected set; }
    public int TargetPlayerEnemyID { get; set; }
    public bool IsAlive => CurrentHP > 0;
    public virtual bool UsesHumanInput => true;

    protected bool isInitiativeRound;
    protected bool isResponsingRound;
    protected bool isWaiting;

    /// <summary>
    /// 设置玩家运行时 ID。
    /// </summary>
    /// <param name="playerID">玩家 ID。</param>
    public void SetPlayerID(int playerID)
    {
        PlayerID = playerID;
    }

    /// <summary>
    /// 绑定角色并同步初始血量。
    /// </summary>
    /// <param name="role">角色组件。</param>
    public void AssignRole(Role role)
    {
        CurrentRole = role;
        if (CurrentRole == null)
        {
            CurrentHP = 0;
            return;
        }

        CurrentRole.IsRoleEnabled = true;
        CurrentRole.HP = CurrentRole.HPmax;
        CurrentHP = CurrentRole.HP;
    }

    /// <summary>
    /// 初始化战斗前的玩家状态。
    /// </summary>
    /// <param name="playerID">玩家 ID。</param>
    /// <param name="role">角色组件。</param>
    public virtual void InitializeForBattle(int playerID, Role role)
    {
        SetPlayerID(playerID);
        AssignRole(role);
        PrepareForBattle();
    }

    /// <summary>
    /// 重置战斗运行态。
    /// </summary>
    public virtual void PrepareForBattle()
    {
        currSelectedCardID = -1;
        PlayerUIOperation = UIOperation.NONE;
        ResponsingCard = null;
        isInitiativeRound = false;
        isResponsingRound = false;
        isWaiting = false;
        IsSubRound = false;
        currentCards.Clear();
        checkingCards.Clear();

        if (CurrentRole != null)
        {
            CurrentRole.HP = CurrentRole.HPmax;
            CurrentHP = CurrentRole.HP;
            CurrentRole.RefreshTags();
        }
    }

    /// <summary>
    /// 清理手牌引用并销毁残留卡牌对象。
    /// </summary>
    public void ClearHandForBattleReset()
    {
        foreach (Card card in currentCards)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }
        }

        currentCards.Clear();
        checkingCards.Clear();
        currSelectedCardID = -1;
        ResponsingCard = null;
    }

    /// <summary>
    /// 获取最大体力。
    /// </summary>
    /// <returns>角色最大体力；若无角色则返回当前体力下限 1。</returns>
    public int GetMaxHP()
    {
        return CurrentRole != null ? CurrentRole.HPmax : Mathf.Max(1, CurrentHP);
    }

    /// <summary>
    /// 摸到一张牌并加入手牌。
    /// </summary>
    /// <param name="card">卡牌实例。</param>
    public void GetCard(Card card)
    {
        if (card == null)
        {
            return;
        }

        card.gameObject.SetActive(true);
        card.SetOwnerByID(PlayerID);
        currentCards.Add(card);
    }

    /// <summary>
    /// 受到伤害。
    /// </summary>
    /// <param name="amount">伤害值。</param>
    /// <param name="source">伤害来源玩家。</param>
    /// <param name="sourceCard">伤害来源卡牌。</param>
    public void TakeDamage(int amount, Player source = null, Card sourceCard = null)
    {
        if (!IsAlive || amount <= 0)
        {
            return;
        }

        CurrentHP = Mathf.Max(0, CurrentHP - amount);
        if (CurrentRole != null)
        {
            CurrentRole.HP = CurrentHP;
        }

        if (CurrentHP <= 0)
        {
            RoundManager.instance?.HandlePlayerDefeated(this, source);
        }
    }

    /// <summary>
    /// 恢复体力。
    /// </summary>
    /// <param name="amount">恢复值。</param>
    public void Heal(int amount)
    {
        if (!IsAlive || amount <= 0)
        {
            return;
        }

        CurrentHP = Mathf.Min(GetMaxHP(), CurrentHP + amount);
        if (CurrentRole != null)
        {
            CurrentRole.HP = CurrentHP;
        }
    }

    /// <summary>
    /// 进入判定阶段。
    /// </summary>
    /// <returns>协程。</returns>
    public virtual IEnumerator EnterChecking()
    {
        yield return null;
    }

    /// <summary>
    /// 进入主动出牌阶段。
    /// </summary>
    /// <returns>协程。</returns>
    public virtual IEnumerator EnterRound()
    {
        if (!IsSubRound || !IsAlive)
        {
            yield break;
        }

        isWaiting = false;
        yield return StartCoroutine(UpdateTime());
    }

    /// <summary>
    /// 进入弃牌阶段。
    /// </summary>
    /// <returns>协程。</returns>
    public virtual IEnumerator EnterRefusing()
    {
        int maxCards = GetMaxCardsLimit();
        while (currentCards.Count > maxCards)
        {
            DiscardCardFromHand(currentCards.Count - 1);
            yield return null;
        }
    }

    /// <summary>
    /// 进入结束阶段。
    /// </summary>
    /// <returns>协程。</returns>
    public virtual IEnumerator EnterEnding()
    {
        CurrentRole?.RefreshTags();
        yield return null;
    }

    /// <summary>
    /// 进入响应阶段。
    /// </summary>
    /// <returns>协程。</returns>
    public virtual IEnumerator EnterResponse()
    {
        if (!IsAlive)
        {
            yield break;
        }

        if (!CheckAllCards(out int _))
        {
            ResponsingCard?.ResponseTrigger();
            ResponsingCard = null;
            yield break;
        }

        float elapsed = 0f;
        PlayerUIOperation = UIOperation.NONE;
        isInitiativeRound = false;
        isResponsingRound = true;
        currSelectedCardID = -1;

        while (PlayerUIOperation == UIOperation.NONE && elapsed < responseTimeLimit)
        {
            yield return null;
            elapsed += Time.deltaTime;
        }

        if (PlayerUIOperation == UIOperation.OK && IsCurrentSelectionPlayable())
        {
            UseCard(currSelectedCardID, GetResolvedTarget());
        }
        else
        {
            ResponsingCard?.ResponseTrigger();
        }

        ResponsingCard = null;
        PlayerUIOperation = UIOperation.NONE;
        isInitiativeRound = false;
        isResponsingRound = false;
        currSelectedCardID = -1;
    }

    /// <summary>
    /// 玩家点击确认。
    /// </summary>
    public void Confirm_ButtonClick()
    {
        PlayerUIOperation = UIOperation.OK;
    }

    /// <summary>
    /// 玩家点击取消。
    /// </summary>
    public void Cancel_ButtonClick()
    {
        PlayerUIOperation = UIOperation.CANCEL;
    }

    protected virtual IEnumerator UpdateTime()
    {
        isInitiativeRound = true;
        isResponsingRound = false;

        for (int actionIndex = 0; actionIndex < maxInitiativeActionsPerTurn; actionIndex++)
        {
            if (!HasPlayableInitiativeCard())
            {
                break;
            }

            float elapsed = 0f;
            PlayerUIOperation = UIOperation.NONE;
            currSelectedCardID = -1;

            while (PlayerUIOperation == UIOperation.NONE && elapsed < initiativeTurnTimeLimit)
            {
                while (isWaiting)
                {
                    yield return null;
                }

                yield return null;
                elapsed += Time.deltaTime;
            }

            if (PlayerUIOperation == UIOperation.OK && IsCurrentSelectionPlayable())
            {
                yield return StartCoroutine(DoUseCard());
                continue;
            }

            break;
        }

        PlayerUIOperation = UIOperation.NONE;
        isInitiativeRound = false;
        isResponsingRound = false;
        currSelectedCardID = -1;
    }

    protected bool CheckCard(int cardID)
    {
        if (cardID < 0 || cardID >= currentCards.Count)
        {
            return false;
        }

        if (isInitiativeRound)
        {
            return currentCards[cardID].IsCardAvailable(this);
        }

        if (isResponsingRound && ResponsingCard != null)
        {
            return currentCards[cardID].IsCardResponsible(ResponsingCard.CardType);
        }

        return false;
    }

    protected bool CheckAllCards(out int cardKey)
    {
        cardKey = -1;
        for (int i = 0; i < currentCards.Count; i++)
        {
            if (!CheckCard(i))
            {
                continue;
            }

            cardKey = i;
            return true;
        }

        return false;
    }

    protected bool CheckAllCards(out List<int> cardKeyList)
    {
        cardKeyList = new List<int>();
        for (int i = 0; i < currentCards.Count; i++)
        {
            if (CheckCard(i))
            {
                cardKeyList.Add(i);
            }
        }

        return cardKeyList.Count > 0;
    }

    protected List<int> GetPlayableInitiativeCardIndices()
    {
        List<int> result = new List<int>();
        bool oldInitiative = isInitiativeRound;
        bool oldResponse = isResponsingRound;
        isInitiativeRound = true;
        isResponsingRound = false;

        for (int i = 0; i < currentCards.Count; i++)
        {
            if (CheckCard(i))
            {
                result.Add(i);
            }
        }

        isInitiativeRound = oldInitiative;
        isResponsingRound = oldResponse;
        return result;
    }

    protected bool HasPlayableInitiativeCard()
    {
        return GetPlayableInitiativeCardIndices().Count > 0;
    }

    protected bool IsCurrentSelectionPlayable()
    {
        return CheckCard(currSelectedCardID);
    }

    protected IEnumerator DoUseCard()
    {
        if (!IsCurrentSelectionPlayable())
        {
            yield break;
        }

        Card card = currentCards[currSelectedCardID];
        Player target = GetResolvedTarget();
        UseCard(currSelectedCardID, target);

        if (card.IsCardResponsible() && target != null && target.IsAlive)
        {
            isWaiting = true;
            target.ResponsingCard = card;
            target.TargetPlayerEnemyID = PlayerID;
            yield return target.StartCoroutine(target.EnterResponse());
            isWaiting = false;
        }
    }

    protected void UseCard(int cardId, Player target)
    {
        if (cardId < 0 || cardId >= currentCards.Count)
        {
            return;
        }

        Card card = currentCards[cardId];
        currentCards.RemoveAt(cardId);
        currSelectedCardID = -1;
        card.SetOwnerByID(1);
        card.DoCardsAction(this, target);
        CardManager.instance?.DiscardCard(card);
    }

    protected void DiscardCardFromHand(int cardId)
    {
        if (cardId < 0 || cardId >= currentCards.Count)
        {
            return;
        }

        Card card = currentCards[cardId];
        currentCards.RemoveAt(cardId);
        CardManager.instance?.DiscardCard(card);
    }

    protected Player GetResolvedTarget()
    {
        Player explicitTarget = PlayerManager.Instance != null ? PlayerManager.Instance.GetPlayerInstanceByID(TargetPlayerEnemyID) : null;
        if (explicitTarget != null && explicitTarget.IsAlive && explicitTarget != this)
        {
            return explicitTarget;
        }

        return PlayerManager.Instance != null ? PlayerManager.Instance.GetFirstLivingOpponent(this) : null;
    }

    protected int GetMaxCardsLimit()
    {
        return CurrentRole != null ? CurrentRole.GetMaxCardsNum() : currentCards.Count;
    }
}

public enum UIOperation
{
    NONE,
    OK,
    CANCEL
}
