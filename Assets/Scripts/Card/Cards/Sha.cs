using UnityEngine;

/// <summary>
/// 基础攻击牌，未被闪响应时对目标造成 1 点伤害。
/// </summary>
public class Sha : Card
{
    public override string cardName { get; protected set; } = "Sha";
    public override string description { get; protected set; } = "对一名目标角色发起攻击，若未被闪响应，则造成 1 点伤害。";
    public override Sprite cardImage { get; protected set; }
    public override string CardType { get; protected set; } = "Sha";

    public override void DoCardsAction(Player user, Player target)
    {
        base.DoCardsAction(user, target);
    }

    public override void ResponseTrigger()
    {
        Player target = GetLastTarget();
        if (target == null)
        {
            return;
        }

        target.TakeDamage(1, GetLastUser(), this);
    }

    public override bool IsCardResponsible(string cardType)
    {
        return cardType == CardType;
    }

    public override bool IsCardResponsible()
    {
        return true;
    }
}
