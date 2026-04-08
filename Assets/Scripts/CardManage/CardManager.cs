using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssetBundleFormWork;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card> cardsStack = new List<Card>();//ID:0
    public List<Card> usedCards = new List<Card>();//ID:1
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
    }
    /*
    //public IEnumerator CardEffct(string spriteName)
    //{
    //    if (spriteName != null)
    //    {
    //        CardEffect.gameObject.GetComponent<Image>().sprite = ResourcesLoad<Sprite>.LoadRes("UI/Card/" + spriteName); ;
    //        CardEffect.gameObject.SetActive(true);
    //        Tween t = CardEffect.transform.DOScale(3, 2f);

    //        t.OnComplete(() =>
    //        {
    //            CardEffect.transform.DOScale(1, 2f);
    //        });
    //        yield return new WaitForSeconds(4f);
    //        CardEffect.gameObject.SetActive(false);
    //        StopCoroutine(CardEffct(spriteName));
    //    }
    }*/
    /// <summary>
    /// 洗牌算法
    /// </summary>
    /// <returns></returns>
    public List<Card> Shuffle(int cardNum)     //(未完成)洗牌------------
    {
        for (int i = 0; i < cardNum; i++)
        {
            GameObject cardObject = Instantiate(Resources.Load<GameObject>("Prefabs/Cards/" + GetNextCardName()));
            Card card = cardObject.GetComponent<Card>();
            card.SetOwnerByID(0);
            cardsStack.Add(card);
        }
        CardsRearrangement();
        return cardsStack;
    }
    private string GetNextCardName()
    {
        if (cardsStack.Count <= 3)
        {
            return "Sha";
        }
        else if (cardsStack.Count <= 6)
        {
            return "Shan";
        }
        else if (cardsStack.Count <= 10)
        {
            return "Tao";
        }
        return "Sha";
    }
    public List<Card> CardsRearrangement()
    {
        // 洗牌
        for (int i = 0; i < cardsStack.Count; i++)
        {
            Card temp = cardsStack[i];
            // 在i的后面随机一张卡交换
            int index = Random.Range(0, cardsStack.Count);
            cardsStack[i] = cardsStack[index];
            cardsStack[index] = temp;
        }
        return cardsStack;
    }

    /// <summary>
    /// 抽牌
    /// </summary>
    /// <returns></returns>
    public Card GetCard()
    {
        // 如果卡抽完了
        if (cardsStack.Count <= 0)
        {
            //// 重新获取一副卡
            //cards = CardManager.instance.Shuffle();

            //旧牌堆洗牌并移入牌堆
            CardManager.instance.usedCards.ForEach(item =>
            {
                item.SetOwnerByID(0);
                CardManager.instance.cardsStack.Add(item);
            });
            CardManager.instance.usedCards.Clear();
            CardManager.instance.CardsRearrangement();
        }
        ////Debug.LogError(currCardIndex);
        Card card = cardsStack[0];
        cardsStack.RemoveAt(0);
        return card;
        ////Debug.Log(card.cardName);
        //if (card == null)
        //{
        //    Debug.Log("获取为空-1:" + currCardIndex);
        //    return GetCard();
        //}
        // 更新卡库的数字
        //UI_MainPanel.Instance.UpdateStandByCard_All(cards.Length - currCardIndex);
    }

    /// <summary>
    /// 放入旧牌堆
    /// </summary>
    /// <param name="card"></param>
    public void RemoveCard(Card card)
    {
        usedCards.Remove(card);
    }

    public IEnumerator DealCards()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            for (int i = 0; i < 4; i++)
            {
                p.GetCard(GetCard());
            }
        }
        yield return null;
    }

    public IEnumerator DealCards(Player subPlayer)
    {
        for (int i = 0; i < 4; i++)
        {
            subPlayer.GetCard(GetCard());
        }
        yield return null;
    }



    //public List<Card> InitCharacterCard()
    //{
    //    List<Card> cards = new List<Card>();
    //    for (int i = 0; i < 8; i++)
    //    {
    //        cards[i] = UICard.instance.cardsDic[7 + i];
    //    }

    //    return cards;
    //}

    //public void Init()
    //{
    //    //this.parent = Chorol_Follow.instance.m_target;
    //    //this.path = Chorol_Follow.instance.m_path;
    //}

    /*弃用
        public void InitEnemyMessege(int num,string spriteName="")
        {
            for (int i = 0; i < num; i++)
            {
                GameObject go = InitPrefab("Prefabs/hp_gouyu", EnemyHpParent);
                //go.GetComponent<Image>().sprite= AssetBundleManager.GetInstance().LoadAsset<Sprite>("card", "game.ab", spriteName, false);
                //AssetBundleManager.GetInstance().LoadAsset<Sprite>("card", "game.ab", spriteName, false) as Sprite;
                EnemyHpList.Add(go);
            }
        }
        int indexHp = 0;
        public void RemoveEnemyHp()
        {
            if(EnemyHpList.Count>0)
            {
                Destroy(EnemyHpList[indexHp].gameObject);
                EnemyHpList.RemoveAt(indexHp);
                //indexHp += 1;
            }
        }

        //public void AddEnemyHp()
        //{
        //    GameObject go = InitPrefab("Prefabs/hp_gouyu", EnemyHpParent);
        //    EnemyHpList.Add(go);
        //}
        public void AddCard(CharacterType characterType, int cardID,CardDefines cardDefines)
        {
            Init();
            UI_CardItem uI_CardItem = InitPrefab(path, parent).GetComponent<UI_CardItem>();
            uI_CardItem.Init(characterType, cardID, cardDefines);
            if(characterType==CharacterType.Player)
            {
                cardItemDic.Add(cardID, uI_CardItem);
            }
            if(characterType==CharacterType.EnemyAI)
            {
                //Debug.LogError(cardID);
                enemyCardDic.Add(cardID,uI_CardItem);
            }
        }
        public void ReMoveCard(int CardID)
        {
            Destroy(cardItemDic[CardID].gameObject);
            cardItemDic.Remove(CardID);
        }

        public void EnemyReMoveCard(int CardID)
        {
            Debug.Log("EnemyReMoveCard");
            Debug.Log(CardID);
            Destroy(enemyCardDic[CardID].gameObject);
            enemyCardDic.Remove(CardID);
        }
        public void ReMoveAllCard()
        {
            foreach (var item in cardItemDic.Values)
            {
                Destroy(item.gameObject);
            }
            cardItemDic.Clear();
        }


        public int currCardID = -1;
        public void SelectedCard(CharacterType charactertype,int cardID)
        {
            //如果上一次选择和本次是一样的，就无视
            if (currCardID == cardID)
            {
                return;
            }
            if(charactertype==CharacterType.EnemyAI)
            {
                return;
            }
            if(charactertype==CharacterType.Player)
            {
                // 找到老的ID对应的实际UIItem元素，设置他的Select为False
                if (cardItemDic.ContainsKey(currCardID))
                    cardItemDic[currCardID].IsSelect = false;
                currCardID = cardID;
                cardItemDic[cardID].IsSelect = true;
                // 让Player脚本知道当前选择的卡
                Player.Instance.currSelectedCardID = currCardID;
                // 根据玩家是否可以使用此卡，来决定按钮的显示状态
                CanUseCard = Player.Instance.CheckCard(cardID);
            }


        }

        // 能否使用当前卡
        private bool canUseCard;
        public bool CanUseCard
        {
            get => canUseCard;
            set
            {
                canUseCard = value;
                if (canUseCard)
                {
                    // 让确认按钮显示高亮状态
                    Confirm_Button.image.sprite = Confirm_Button_Normal;
                }
                else
                {
                    Confirm_Button.image.sprite = Confirm_Button_Disable;
                }
            }
        }
        public static GameObject InitPrefab(string path, GameObject parent)
        {
            if (path == null)
                return null;
            GameObject go = ResourcesLoad<GameObject>.LoadRes(path);
            go = Instantiate(go);

            if (parent == null)
                return go;
            go.transform.parent = parent.transform;
            go.transform.localScale = Vector3.one;
            return go;
        }


        /// <summary>
        /// 当前是否是玩家的回合
        /// </summary>
        private bool isOnPlayerRound;
        public bool IsOnPlayerRound
        {
            get => isOnPlayerRound;
            set
            {
                isOnPlayerRound = value;
                //// 关闭或隐藏按钮
                //Confirm_Button.gameObject.SetActive(isOnPlayerRound);
                //Cancel_Button.gameObject.SetActive(isOnPlayerRound);
                //// 关闭或隐藏剩余时间
                //TimeImage.gameObject.SetActive(isOnPlayerRound);

                // 如果是当前回合
                if (isOnPlayerRound)
                {

                }
                // 不是当前回合
                else
                {
                    // 取消之前可能选择的卡
                    if (cardItemDic.ContainsKey(currCardID))
                    {
                        cardItemDic[currCardID].IsSelect = false;
                    }
                }
            }
        }



        public Button Confirm_Button;
        public Button Cancel_Button;
        public Sprite Confirm_Button_Disable;
        public Sprite Confirm_Button_Normal;
        public Slider TimeImage;

        /// <summary>
        /// 确认出牌 按钮点击
        /// </summary>
        private void Confirm_ButtonClick()
        {
            // 每次玩家点击卡片后，都会检查canUseCard
            // 当前卡如果可以使用
            if (CanUseCard)
            {
                Debug.LogError("执行");
                Player.Instance.Confirm_ButtonClick();
            }
            else
            {

                if (!IsLoaded)
                {
                    UiManager.Instance().ShowUiForms(UiWind.LogErrorPopPanel.ToString());
                    IsLoaded = true;
                }
                else
                    LogErrorPopPanel.Instance.Display();
                LogErrorPopPanel.Instance.HidePanel();

            }
        }
       public bool IsLoaded = false;
        /// <summary>
        /// 取消出牌 按钮点击
        /// </summary>
        private void Cancel_ButtonClick()
        {
            Player.Instance.Cancel_ButtonClick();
        }

        public void UpdateTime(float value)
        {
            //Debug.LogError("UpdateTime"+ TimeImage.fillAmount);
            //TimeImage.fillAmount = value;
            TimeImage.value = value;
        }



        #region 出牌效果
        //public Image UserCardEF_Image;
        //public AudioSource AudioSource;
        private AudioClip audioClip;

        private string ClipName = "";
        private void OutClip(CardDefines card)
        {

            ClipName = card.useAudioClip;
            ClipName = ClipName + ".ogg";
            string abName = "ogg.ab";

            AudioClip clip = AssetBundleManager.GetInstance().LoadAsset<AudioClip>("audio", abName, ClipName, false) as AudioClip;

            audioClip = clip;

        }

        public void AudioEffect(string clipName)
        {
            string abName = "ogg.ab";
            AudioClip clip = AssetBundleManager.GetInstance().LoadAsset<AudioClip>("audio", abName, clipName, false) as AudioClip;

            audioClip = clip;
            AudioManage.Instance.PlayAudio(audioClip);
        }
        /// <summary>
        /// 使用卡 效果
        /// </summary>
        public void UserCard_Effect(CardDefines card)
        {
            //Debug.LogError("UserCard_Effect");
            OutClip(card);
            //Debug.LogError("执行音效");
            //Debug.LogError(audioClip);
            AudioManage.Instance.PlayAudio(audioClip);

            //card.useAudioClip = audioClip;
            //// 卡牌音效
            //AudioSource.PlayOneShot(card.useAudioClip);
            // 卡牌视觉效果
            //UserCardEF_Image.sprite = card.cardImage;
            //StopCoroutine(DoCardImageEffect());
            //StartCoroutine(DoCardImageEffect());
        }

        IEnumerator DoCardImageEffect()
        {
            //UserCardEF_Image.color = Color.white;
            Color color = Color.white;
            while (color.a > 0)
            {
                color.a -= Time.deltaTime;
                //UserCardEF_Image.color = color;
                yield return null;
            }
            //UserCardEF_Image.color = new Color(1, 1, 1, 0);
        }



        #endregion
    */
}

