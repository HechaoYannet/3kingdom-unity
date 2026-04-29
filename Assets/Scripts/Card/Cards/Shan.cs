using UnityEngine;

/// <summary>
/// 基础防御牌，仅用于响应杀。
/// </summary>
public class Shan : Card
{
    public override string cardName { get; protected set; } = "Shan";
    public override string description { get; protected set; } = "用于响应杀，使本次攻击无效。";
    public override Sprite cardImage { get; protected set; }
    public override string CardType { get; protected set; } = "Shan";

    public override bool IsCardResponsible(string cardType)
    {
        return cardType == "Sha";
    }
}
