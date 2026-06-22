using System.Collections.Generic;

/// <summary>
/// 基础规则 AI，只做最小可用的出牌和响应选择。
/// </summary>
public static class BasicBattleAI
{
    /// <summary>
    /// 从可出的主动牌中选择一张。
    /// </summary>
    /// <param name="playableIndices">当前可出的手牌索引。</param>
    /// <param name="hand">完整手牌。</param>
    /// <param name="currentHP">当前体力。</param>
    /// <param name="maxHP">最大体力。</param>
    /// <param name="selectedIndex">选中的手牌索引。</param>
    /// <returns>是否选中可行动作。</returns>
    public static bool TryChooseInitiativeCard(List<int> playableIndices, List<Card> hand, int currentHP, int maxHP, out int selectedIndex)
    {
        selectedIndex = -1;
        if (playableIndices == null || hand == null || playableIndices.Count == 0)
        {
            return false;
        }

        if (currentHP < maxHP)
        {
            foreach (int index in playableIndices)
            {
                if (hand[index] is Tao)
                {
                    selectedIndex = index;
                    return true;
                }
            }
        }

        foreach (int index in playableIndices)
        {
            if (hand[index] is Sha)
            {
                selectedIndex = index;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 从可响应的牌中选择一张。
    /// </summary>
    /// <param name="playableIndices">当前可响应的手牌索引。</param>
    /// <param name="hand">完整手牌。</param>
    /// <param name="responsingCard">待响应的牌。</param>
    /// <param name="selectedIndex">选中的手牌索引。</param>
    /// <returns>是否成功选择。</returns>
    public static bool TryChooseResponseCard(List<int> playableIndices, List<Card> hand, Card responsingCard, out int selectedIndex)
    {
        selectedIndex = -1;
        if (playableIndices == null || hand == null || responsingCard == null || playableIndices.Count == 0)
        {
            return false;
        }

        foreach (int index in playableIndices)
        {
            if (hand[index] is Shan && responsingCard.CardType == "Sha")
            {
                selectedIndex = index;
                return true;
            }
        }

        return false;
    }
}
