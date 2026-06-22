using UnityEngine;

/// <summary>
/// 基础防御牌，仅用于响应杀；打出即视为闪避成功，阻止杀的 ResponseTrigger 结算。
/// </summary>
public class Shan : Card
{
    public override string cardName { get; protected set; } = "Shan";
    public override string description { get; protected set; } = "用于响应杀，使攻击不造成伤害。";
    public override Sprite cardImage { get; protected set; }
    public override string CardType { get; protected set; } = "Shan";

    private void Awake()
    {
        if (cardImage == null)
            cardImage = Resources.Load<Sprite>("UI/Card/闪");
    }

    public override void DoCardsAction(Player user, Player target)
    {
        base.DoCardsAction(user, target);
    }

    public override bool IsCardResponsible(string cardType)
    {
        return cardType == "Sha";
    }
}
