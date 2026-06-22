using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基于规则的最小可用战斗 AI。
/// </summary>
public class EnemyAI : Player
{
    [SerializeField] private float initiativeDecisionDelay = 0.75f;
    [SerializeField] private float responseDecisionDelay = 0.35f;
    [SerializeField] private float betweenActionsDelay = 0.3f;

    public override bool UsesHumanInput => false;

    public override IEnumerator EnterRound()
    {
        if (!IsSubRound || !IsAlive)
        {
            yield break;
        }

        yield return StartCoroutine(UpdateTime());
    }

    public override IEnumerator EnterResponse()
    {
        if (!IsAlive)
        {
            yield break;
        }

        yield return new WaitForSeconds(responseDecisionDelay);
        isInitiativeRound = false;
        isResponsingRound = true;

        if (CheckAllCards(out List<int> playableIndices) && BasicBattleAI.TryChooseResponseCard(playableIndices, currentCards, ResponsingCard, out int selectedIndex))
        {
            currSelectedCardID = selectedIndex;
            UseCard(selectedIndex, ResolveTargetForCard(currentCards[selectedIndex]));
        }
        else
        {
            ResponsingCard?.ResponseTrigger();
        }

        ResponsingCard = null;
        currSelectedCardID = -1;
        isResponsingRound = false;
    }

    protected override IEnumerator UpdateTime()
    {
        isInitiativeRound = true;
        isResponsingRound = false;
        yield return new WaitForSeconds(initiativeDecisionDelay);

        for (int actionIndex = 0; actionIndex < maxInitiativeActionsPerTurn; actionIndex++)
        {
            List<int> playableIndices = GetPlayableInitiativeCardIndices();
            if (!BasicBattleAI.TryChooseInitiativeCard(playableIndices, currentCards, CurrentHP, GetMaxHP(), out int selectedIndex))
            {
                break;
            }

            Player target = GetResolvedTarget();
            if (target == null)
            {
                break;
            }

            currSelectedCardID = selectedIndex;
            TargetPlayerEnemyID = target.PlayerID;
            yield return StartCoroutine(DoUseCard());
            yield return new WaitForSeconds(betweenActionsDelay);
        }

        currSelectedCardID = -1;
        isInitiativeRound = false;
        isResponsingRound = false;
    }
}
