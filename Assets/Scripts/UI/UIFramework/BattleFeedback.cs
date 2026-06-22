using UnityEngine;

/// <summary>
/// 战斗反馈静态门面，供规则层（Sha/Tao 等）调用以显示伤害/治疗飘字。
/// 实际实现由 FeedbackTextPool（阶段 2）注入。
/// </summary>
public static class BattleFeedback
{
    private static IFeedbackProvider provider;

    public static void SetProvider(IFeedbackProvider p) => provider = p;

    public static void ShowDamage(int amount, Vector3 worldPos, bool isCritical = false)
        => provider?.ShowDamage(amount, worldPos, isCritical);

    public static void ShowHeal(int amount, Vector3 worldPos)
        => provider?.ShowHeal(amount, worldPos);
}

/// <summary>
/// 反馈提供者接口，由具体的池化系统实现。
/// </summary>
public interface IFeedbackProvider
{
    void ShowDamage(int amount, Vector3 worldPos, bool isCritical = false);
    void ShowHeal(int amount, Vector3 worldPos);
}
