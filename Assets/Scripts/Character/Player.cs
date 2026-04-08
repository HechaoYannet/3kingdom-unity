using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using AssetBundleFormWork;

public class Player : MonoBehaviour
{
    //public static Player Instance;

    public int PlayerID { get; protected set; }

    protected int currHP;
    //多武将
    public List<Role> roles = new List<Role>();//no used
    public Role CurrentRole { get; protected set; }

    public int CurrentHP
    {
        get => currHP;
        set
        {
            currHP = value;
            // 更新UI
            //UI_MainPanel.Instance.UpdatePlayer_HP(CurrentHP);
        }
    }

    public bool IsSubRound { get; set; } = false;
    // 当前选择的卡-有可能会使用的卡
    public int currSelectedCardID = -1;

    // 当前玩家手里的卡
    //private Dictionary<int, CardDefines> currCardDic = new Dictionary<int, CardDefines>();
    public List<Card> currentCards = new List<Card>();
    public List<Card> checkingCards = new List<Card>();
    //private int currCardID;

    // 当前是否是我的主动回合
    protected bool isInitiativeRound = false;
    protected bool isResponsingRound = false;//回应
    // 对方当前出的卡
    public Card ResponsingCard { get; set; }
    //记录玩家UI操作
    public UIOperation PlayerUIOperation { get; set; }
    public int TargetPlayerEnemyID { get; set; }
    // 是否等待敌人
    protected bool isWaiting;


    public List<GameObject> PlayerHpList = new List<GameObject>();
    public GameObject HpParent;
    void Awake()
    {
        //Instance = this;
        //Player_Img.sprite = CharacterMesseges.sprite;
        //InitEnemyMessege(4);
    }

    public void SetPlayerID(int playerID)
    {
        PlayerID = playerID;
    }

    public void Initialize()
    {
        
    }

    //public void InitEnemyMessege(int num)
    //{
    //    for (int i = 0; i < num; i++)
    //    {
    //        //GameObject go = CardManager.InitPrefab("Prefabs/hp_gouyu", HpParent);
    //        //go.GetComponent<Image>().sprite= AssetBundleManager.GetInstance().LoadAsset<Sprite>("card", "game.ab", spriteName, false);
    //        //AssetBundleManager.GetInstance().LoadAsset<Sprite>("card", "game.ab", spriteName, false) as Sprite;
    //        PlayerHpList.Add(go);
    //    }
    //}
    //int indexHp = 0;
    //public IEnumerable RemovePlayerHp(int hp)
    //{
    //    if (PlayerHpList.Count >= 1)
    //    {
    //        Destroy(PlayerHpList[indexHp].gameObject);
    //        PlayerHpList.RemoveAt(indexHp);
    //        //indexHp += 1;
    //        CurrentHP -= hp;
    //    }

    //    return null;
    //}
    /// <summary>
    /// 获取卡
    /// </summary>
    public void GetCard(Card card)
    {
        card.SetOwnerByID(PlayerID);
        currentCards.Add(card);
        //Chorol_Follow.instance.Init(1);
        // 更新UI
        //CardManager.instance.AddCard(CharacterType.Player, currCardID, card);
        //currCardID += 1;
    }

    /// <summary>
    /// 移除全部卡
    /// </summary>
    public void RemoveAllCard()
    {
        //修改Owner
        currentCards.ForEach(card => card.SetOwnerByID(1));
        currentCards.Clear();
        // 更新UI
        //UI_MainPanel.Instance.RemoveAllCard();
    }

    /// <summary>
    /// 检查这张卡能不能出
    /// </summary>
    protected bool CheckCard(int cardID)
    {
        //return true;
        // 是主动回合
        //Debug.LogError("isInitiativeRound:" + isInitiativeRound);
        if (isInitiativeRound)
        {
            return currentCards[cardID].IsCardAvailable();
        }
        // 不是主动回合- 是响应别人，就是别人对我出杀了
        else if (isResponsingRound)
        {
            //// 当前选择的卡，是不是可以响应敌人出的卡
            return currentCards[currSelectedCardID].IsCardResponsible(ResponsingCard.CardType);
        }
        return false;
    }
    /// <summary>
    /// 检查响应卡
    /// </summary>
    /// <param name="cardKey">首个card,没有则为-1</param>
    /// <returns>是否有ResponsibleCard</returns>
    protected bool CheckAllCards(out int cardKey)
    {
        cardKey = -1;
        bool value = false;
        for (int i = 0; i < currentCards.Count; i++)
        {
            value = value || CheckCard(i);
            if (value && cardKey == -1)
            {
                cardKey = i;
            }
        }
        return value;
    }
    protected bool CheckAllCards(out List<int> cardKeyList)
    {
        cardKeyList = new List<int>();
        bool value = false;
        for (int i = 0; i < currentCards.Count; i++)
        {
            bool result = CheckCard(i);
            value = value || result;
            if (result)
            {
                cardKeyList.Add(i);
            }
        }
        return value;
    }


    public IEnumerator EnterChecking()
    {
        yield return null;
    }

    /// <summary>
    /// 进入主动回合
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator EnterRound()
    {
        if (IsSubRound)
        {
            isWaiting = false;
            // 等待玩家操作...
            yield return StartCoroutine(UpdateTime());
        }
    }

    /// <summary>
    /// 进入弃牌
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnterRefusing()
    {
        if (RoundManager.instance.RoundState == GRoundState.RefusingCard
            && IsSubRound
            && currentCards.Count > CurrentRole.GetMaxCardsNum())
        {

        }
        yield return null;
    }

    public IEnumerator EnterEnding()
    {
        CurrentRole.RefreshTags();
        yield return null;
    }
    /// <summary>
    /// 主回合更新逻辑
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator UpdateTime()
    {
        float currTime = 0;
        PlayerUIOperation = UIOperation.NONE;
        isInitiativeRound = true;
        isResponsingRound = false;
        // 玩家没有点确认、没点取消、操作时间也没到（假设一回合要在15秒内出一次牌）
        while (PlayerUIOperation == UIOperation.NONE && currTime < 30f)
        {
            // 等待玩家操作
            // 此处可能等待敌人的操作,比如玩家对敌人出杀后，等待敌人是否出闪
            while (isWaiting)
            {
                Debug.Log("????代码冗余?????????");
                yield return null;
            }
            yield return null;
            currTime += Time.deltaTime;
            // 刷新时间UI
        }
        // 玩家操作了 或者时间到

        // 出牌了
        if (PlayerUIOperation == UIOperation.OK)
        {
            // 出牌，检查敌人
            yield return StartCoroutine(DoUseCard());
            // 递归，让玩家继续可以出牌
            if (isWaiting == false)
                yield return StartCoroutine(UpdateTime());
        }
        // 取消了，主动放弃了
        else if (PlayerUIOperation == UIOperation.CANCEL || currTime >= 30f)
        {
            // 视为放弃此出牌阶段
            yield return null;
        }
    }

    /// <summary>
    /// 进入响应
    /// </summary>
    public virtual IEnumerator EnterResponse()
    {
        if (CheckAllCards(out int _))
        {
            float currTime = 0;
            PlayerUIOperation = UIOperation.NONE;
            isInitiativeRound = false;
            isResponsingRound = true;
            // 保存对方使用的卡
            // 等待玩家操作
            while (PlayerUIOperation == UIOperation.NONE && currTime < 30f)
            {
                yield return null;
                currTime += Time.deltaTime;
            }
            // 玩家操作了 或者时间到
            // 出牌了
            if (PlayerUIOperation == UIOperation.OK)
            {
                //IsEnterResponse = true;
                //Debug.LogError("operationIsOk:" + operationIsOk);
                // 响应成功 把卡使用掉
                CardManager.instance.RemoveCard(currentCards[currSelectedCardID]);
                UseCard(currSelectedCardID);
                //敌人继续出牌
            }
            else if (PlayerUIOperation == UIOperation.CANCEL || currTime >= 30f)
            {
                // 取消了，主动放弃了
                // 意味着对方的卡生效
                ResponsingCard.ResponseTrigger();
                yield return new WaitForSeconds(0.5f);

                // 敌人回合结束，进入自己回合
                //CardManager.instance.AudioSource.PlayOneShot(card.takeEffectAudioClip);
            }
        }
        else
        {
            //ui
            ResponsingCard.ResponseTrigger();
            yield return new WaitForSeconds(0.5f);
        }
        CardManager.instance.RemoveCard(ResponsingCard);
        ResponsingCard = null;
    }

    /// <summary>
    /// 执行使用卡片-主动回合的情况
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DoUseCard()
    {
        Card card = currentCards[currSelectedCardID];
        // 使用卡片,方法结束后 逻辑层面和显示层面都没有这张卡了
        UseCard(currSelectedCardID);
        // 使用卡有可能需要对方响应
        if (card.IsCardResponsible())
        {
            // 敌人进入响应，当敌人响应结束，应该恢复 isWaiting
            isWaiting = true;
            // 刷新UI
            // 等待敌人响应
            Player target = PlayerManager.Instance.GetPlayerInstanceByID(TargetPlayerEnemyID);
            target.ResponsingCard = card;
            target.TargetPlayerEnemyID = PlayerID;
            // 等待enemyResponse
            yield return target.StartCoroutine(target.EnterResponse());
            isWaiting = false;
        }
        else
        {
            CardManager.instance.RemoveCard(card);
        }
        // 运行到这里，意味着打出去一张卡，操作结束
        yield return null;
    }

    /// <summary>
    /// 使用卡片
    /// </summary>
    protected void UseCard(int cardId)
    {
        //Debug.LogError("PlayerUseCard");
        Card card = currentCards[cardId];
        // 逻辑层面删除这个卡
        card.SetOwnerByID(1);
        currentCards.Remove(card);
        card.DoCardsAction(this, PlayerManager.Instance.GetPlayerInstanceByID(TargetPlayerEnemyID));
        //弃用
        //Debug.LogError(card.cardName + " : " + card.Recover);
        //if (isResponsingRound)
        //{
        //    card.IsInitiative = true;
        //}
        //if (card.IsInitiative == false)
        //{
        //    Debug.LogError(card.IsInitiative);
        //    return;
        //}
        //if (card.cardID == 3 && card.FullHPCanInitiative == false)
        //{
        //    Debug.LogError(card.cardName + " : " + card.Recover);
        //    if (card.Recover > 0 && CurrentHP < 4)
        //    {
        //        CurrentHP += card.Recover;
        //        InitEnemyMessege(card.Recover);
        //    }
        //}
        // 如果有恢复能力
        //if (card.GetCardCount > 0)
        //{
        //    Debug.LogError("获取卡牌");
        //    RoundManager.instance.PlayerGetCard(card.GetCardCount);
        //    return;

        //}
        //if (card.cardName == "杀" || card.cardName == "闪")
        //    CardManager.instance.StartCoroutine(CardManager.instance.CardEffct(card.cardName + "_effect"));
        //// 恢复UI
        //CardManager.instance.CanUseCard = false;
        //CardManager.instance.UserCard_Effect(card);
        // 从手里移除这张卡
        //弃牌
        //FoldCard();
    }



    /////////////?????????????
    /// <summary>
    /// 玩家在UI那边点了确认
    /// </summary>
    public void Confirm_ButtonClick()
    {
        PlayerUIOperation = UIOperation.OK;
    }

    /// <summary>
    /// 玩家在UI那边点了取消
    /// </summary>
    public void Cancel_ButtonClick()
    {
        PlayerUIOperation = UIOperation.CANCEL;
    }
}

public enum UIOperation
{
    NONE, OK, CANCEL
}
