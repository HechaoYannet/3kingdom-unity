using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : Player
{
    public EnemyAI(bool isInit = false)
    {
        Debug.LogError("Enemy" + isInit);
        if (isInit)
        {
            string name = "lord-magatama-r.png";
        }
    }
    // HP
    private int currHP;
    public int CurrHP
    {
        get => currHP;
        set
        {
            currHP = value;

            // 更新UI
            //UI_MainPanel.Instance.UpdateEnemy_HP(CurrentHP);
        }
    }

    // 手中的卡
    //private List<Card> currentCards = new List<Card>();
    //private Dictionary<int, Card> currentCards = new Dictionary<int, Card>();
    //private int currCardID;

    /// <summary>
    /// 移除全部卡
    /// </summary>
    //public void RemoveAllCard()
    //{
    //    currentCards.Clear();
    //    // 更新UI
    //    //UI_MainPanel.Instance.UpdateStandByCard_Enemy(currentCards.Count);
    //}

    /// <summary>
    /// 获取卡
    /// </summary>
    //public void GetCard(Card card)
    //{
    //    currentCards.Add(currCardID, card);

    //    Chorol_Follow.instance.Init(2);
    //    // 更新UI
    //    CardManager.instance.AddCard(CharacterType.Enemy, currCardID, card);
    //    currCardID += 1;
    //}

    /// <summary>
    /// 进入响应
    /// </summary>
    public override IEnumerator EnterResponse()
    {
        //Debug.Log("执行EnterResponse");
        // 延迟一点时间，仿真操作
        yield return new WaitForSeconds(Random.Range(1, 2));
        // 能不能阻挡杀
        if (CheckAllCards(out int cardKey))
        {
            //RoundManager.instance.isEnterPlayerRounder = true;
            CardManager.instance.RemoveCard(currentCards[cardKey]);
            UseCard(cardKey);
        }
        else
        {
            // 意味着对方的卡生效
            ResponsingCard.ResponseTrigger();
            yield return new WaitForSeconds(0.5f);

            //if (card.Attack > 0)
            //{
            //    // 更新血量
            //    CardManager.instance.AudioEffect("shaTX");
            //    CardManager.instance.sha_Img.gameObject.SetActive(true);
            //    CurrHP -= card.Attack;
            //    CardManager.instance.RemoveEnemyHp();
            //    yield return new WaitForSeconds(0.5f);
            //    CardManager.instance.sha_Img.gameObject.SetActive(false);
            //    RoundManager.instance.isEnterPlayerRounder = false;
            //}
        }
        CardManager.instance.RemoveCard(ResponsingCard);
        ResponsingCard = null;

        //// 让对方的卡生效
        //if (!canResponse)
        //{

        //    ////UI_MainPanel.Instance.AudioSource.PlayOneShot(card.takeEffectAudioClip);
        //}
    }

    /// <summary>
    /// 使用卡牌
    /// </summary>
    //public void UseCard(int index)
    //{
    //    Card card = currentCards[index];
    //    // 出卡特效
    //    //UI_MainPanel.Instance.UserCard_Effect(card);
    //    // 逻辑层面移除这张卡
    //    //currentCards.RemoveAt(index);
    //    card.SetOwnerByID(1);
    //    currentCards.Remove(card);
    //    card.DoCardsAction(this, PlayerManager.Instance.GetPlayerInstanceByID(TargetPlayerEnemyID));
    //    // 更新卡牌数量
    //    //UI_MainPanel.Instance.UpdateStandByCard_Enemy(currentCards.Count);
    //}

    /// <summary>
    /// 进入主动回合
    /// </summary>
    /// <returns></returns>
    public override IEnumerator EnterRound()
    {
        if (IsSubRound)
        {
            isWaiting = false;
            yield return StartCoroutine(UpdateTime());


            isInitiativeRound = true;
            isResponsingRound = false;
            //Debug.Log("AI的主动回合操作");
        }
    }
    protected override IEnumerator UpdateTime()
    {
        // AI的主动回合操作
        yield return new WaitForSeconds(Random.Range(2, 3f));
        isInitiativeRound = true;
        isResponsingRound = false;
        PlayerUIOperation = UIOperation.NONE;


        /*AI实现
         * 
         * 1.选牌
         * 
         * 2.选敌人
         * 
         * 3.模拟操作
         * PlayerUIOperation == UIOperation.OK......
         * 
         */




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
        else if (PlayerUIOperation == UIOperation.CANCEL)
        {
            // 视为放弃此出牌阶段
            yield return null;
        }
    }
}
