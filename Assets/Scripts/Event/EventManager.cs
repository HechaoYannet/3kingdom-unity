using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // 卡牌打出时触发，参数为打出的 Card 对象
    public static event Action<Card, int> OnCardPlayed;

    public static void TriggerCardPlayed(Card card, int target)
    {
        OnCardPlayed?.Invoke(card, target);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnCardPlayed += CardCallBack; // 订阅事件
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CardCallBack(Card card, int target)
    {
        card.DoCardsAction(PlayerManager.Instance.GetPlayerInstanceByID(card.OwnerID), PlayerManager.Instance.GetPlayerInstanceByID(target));
    }
}
