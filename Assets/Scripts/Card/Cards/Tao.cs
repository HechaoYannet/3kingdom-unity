using UnityEngine;

/// <summary>
/// 基础治疗牌，恢复 1 点体力。
/// </summary>
public class Tao : Card
{
    public override string cardName { get; protected set; } = "Tao";
    public override string description { get; protected set; } = "回 1 点伤。";
    public override Sprite cardImage { get; protected set; }
    public override string CardType { get; protected set; } = "Tao";

    private void Awake()
    {
        if (cardImage == null)
            cardImage = Resources.Load<Sprite>("UI/Card/桃");
    }

    public override void DoCardsAction(Player user, Player target)
    {
        base.DoCardsAction(user, target);
        user?.Heal(1);

        // 治疗飘字
        if (user != null && user.transform != null)
            BattleFeedback.ShowHeal(1, user.transform.position + Vector3.up * 1.5f);
    }

    public override bool IsCardAvailable(Player user)
    {
        return user != null && user.CurrentHP < user.GetMaxHP();
    }
}
