using UnityEngine;

/// <summary>
/// 基础治疗牌，恢复 1 点体力。
/// </summary>
public class Tao : Card
{
    public override string cardName { get; protected set; } = "Tao";
    public override string description { get; protected set; } = "恢复 1 点体力。";
    public override Sprite cardImage { get; protected set; }
    public override string CardType { get; protected set; } = "Tao";

    public override void DoCardsAction(Player user, Player target)
    {
        base.DoCardsAction(user, target);
        user?.Heal(1);
    }

    public override bool IsCardAvailable(Player user)
    {
        return user != null && user.CurrentHP < user.GetMaxHP();
    }
}
