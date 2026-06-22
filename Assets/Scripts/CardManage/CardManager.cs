using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理牌堆、弃牌堆和运行时卡牌实例。
/// </summary>
public class CardManager : MonoBehaviour
{
    [Serializable]
    public class DeckEntry
    {
        public string cardResource;
        public int count;
    }

    public static CardManager instance;

    [SerializeField] private int openingHandSize = 4;
    [SerializeField] private int turnDrawCount = 2;
    [SerializeField] private Transform runtimeCardRoot;
    [SerializeField] private Transform discardCardRoot;
    [SerializeField] private List<DeckEntry> deckTemplate = new List<DeckEntry>
    {
        new DeckEntry { cardResource = "Sha", count = 6 },
        new DeckEntry { cardResource = "Shan", count = 4 },
        new DeckEntry { cardResource = "Tao", count = 2 }
    };

    public List<Card> cardsStack = new List<Card>();
    public List<Card> usedCards = new List<Card>();

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 重置整场战斗的牌局状态并重新生成牌堆。
    /// </summary>
    public void ResetForBattle()
    {
        foreach (Player player in PlayerManager.Instance.players)
        {
            player?.ClearHandForBattleReset();
        }

        DestroyTrackedCards(cardsStack);
        DestroyTrackedCards(usedCards);
        cardsStack.Clear();
        usedCards.Clear();

        BuildDeck();
        CardsRearrangement();
    }

    /// <summary>
    /// 洗牌并返回当前牌堆。
    /// </summary>
    /// <returns>洗牌后的牌堆列表。</returns>
    public List<Card> Shuffle(int cardNum)
    {
        cardsStack.Clear();
        for (int i = 0; i < cardNum; i++)
        {
            Card card = CreateCardInstance(GetFallbackCardName(i));
            card.SetOwnerByID(0);
            card.gameObject.SetActive(false);
            cardsStack.Add(card);
        }

        CardsRearrangement();
        return cardsStack;
    }

    /// <summary>
    /// 原地随机重排牌堆。
    /// </summary>
    /// <returns>重排后的牌堆。</returns>
    public List<Card> CardsRearrangement()
    {
        for (int i = 0; i < cardsStack.Count; i++)
        {
            int index = UnityEngine.Random.Range(i, cardsStack.Count);
            Card temp = cardsStack[i];
            cardsStack[i] = cardsStack[index];
            cardsStack[index] = temp;
        }

        return cardsStack;
    }

    /// <summary>
    /// 从牌堆摸一张牌；若牌堆为空则回收弃牌堆。
    /// </summary>
    /// <returns>摸到的卡牌；若没有可用牌则返回 null。</returns>
    public Card GetCard()
    {
        if (cardsStack.Count <= 0)
        {
            RecycleDiscardPile();
        }

        if (cardsStack.Count <= 0)
        {
            return null;
        }

        Card card = cardsStack[0];
        cardsStack.RemoveAt(0);
        card.gameObject.SetActive(true);
        return card;
    }

    /// <summary>
    /// 将卡牌放入弃牌堆。
    /// </summary>
    /// <param name="card">待弃置的卡牌。</param>
    public void DiscardCard(Card card)
    {
        if (card == null)
        {
            return;
        }

        card.SetOwnerByID(1);
        card.gameObject.SetActive(false);
        Transform discardParent = discardCardRoot != null ? discardCardRoot : runtimeCardRoot;
        if (discardParent != null)
        {
            card.transform.SetParent(discardParent, false);
        }

        if (!usedCards.Contains(card))
        {
            usedCards.Add(card);
        }
    }

    /// <summary>
    /// 兼容旧接口的弃牌入口。
    /// </summary>
    /// <param name="card">待弃置的卡牌。</param>
    public void RemoveCard(Card card)
    {
        DiscardCard(card);
    }

    /// <summary>
    /// 给所有玩家发起始手牌。
    /// </summary>
    /// <returns>协程。</returns>
    public IEnumerator DealCards()
    {
        foreach (Player player in PlayerManager.Instance.players)
        {
            if (player == null || !player.IsAlive)
            {
                continue;
            }

            for (int i = 0; i < openingHandSize; i++)
            {
                player.GetCard(GetCard());
            }
        }

        yield return null;
    }

    /// <summary>
    /// 给指定玩家发本回合摸牌。
    /// </summary>
    /// <param name="subPlayer">要摸牌的玩家。</param>
    /// <returns>协程。</returns>
    public IEnumerator DealCards(Player subPlayer)
    {
        if (subPlayer == null || !subPlayer.IsAlive)
        {
            yield break;
        }

        for (int i = 0; i < turnDrawCount; i++)
        {
            subPlayer.GetCard(GetCard());
        }

        yield return null;
    }

    private void BuildDeck()
    {
        if (deckTemplate == null || deckTemplate.Count == 0)
        {
            Shuffle(12);
            return;
        }

        foreach (DeckEntry entry in deckTemplate)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.cardResource) || entry.count <= 0)
            {
                continue;
            }

            for (int i = 0; i < entry.count; i++)
            {
                Card card = CreateCardInstance(entry.cardResource);
                card.SetOwnerByID(0);
                card.gameObject.SetActive(false);
                cardsStack.Add(card);
            }
        }
    }

    private void RecycleDiscardPile()
    {
        if (usedCards.Count <= 0)
        {
            return;
        }

        foreach (Card card in usedCards)
        {
            if (card == null)
            {
                continue;
            }

            card.SetOwnerByID(0);
            cardsStack.Add(card);
        }

        usedCards.Clear();
        CardsRearrangement();
    }

    private Card CreateCardInstance(string cardResource)
    {
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Cards/{cardResource}");
        Transform parent = runtimeCardRoot != null ? runtimeCardRoot : transform;
        GameObject cardObject = prefab != null ? Instantiate(prefab, parent) : new GameObject(cardResource);
        if (prefab == null)
        {
            cardObject.transform.SetParent(parent, false);
        }

        cardObject.name = cardResource;

        Card card = cardObject.GetComponent<Card>();

        if (card == null)

        {

            card = AddMissingCardLogic(cardObject, cardResource);

        }


        return card;
    }

    private Card AddMissingCardLogic(GameObject cardObject, string cardResource)
    {
        switch (cardResource)
        {
            case "Sha":
                return cardObject.AddComponent<Sha>();
            case "Shan":
                return cardObject.AddComponent<Shan>();
            case "Tao":
                return cardObject.AddComponent<Tao>();
            default:
                return cardObject.AddComponent<Sha>();
        }
    }

    private string GetFallbackCardName(int index)
    {
        if (index < 6)
        {
            return "Sha";
        }

        if (index < 10)
        {
            return "Shan";
        }

        return "Tao";
    }

    private void DestroyTrackedCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }
        }
    }
}
