using System.Collections;
using UnityEngine;

/// <summary>
/// 驱动完整战斗回合状态机。
/// </summary>
public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public GRoundState RoundState { get; private set; }
    public Player CurrentTurnPlayer { get; private set; }
    public bool IsBattleRunning { get; private set; }

    private Coroutine roundLoop;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 进入战斗并启动主回合循环。
    /// </summary>
    public void EnterGamer()
    {
        StopAllCoroutines();
        PlayerManager.Instance.InitializePlayersForBattle();
        CardManager.instance.ResetForBattle();
        IsBattleRunning = true;
        roundLoop = StartCoroutine(UpdateRound());
        StartCoroutine(CheckGameOver());
    }

    /// <summary>
    /// 处理玩家被击败事件并判定胜负。
    /// </summary>
    /// <param name="defeatedPlayer">被击败的玩家。</param>
    /// <param name="sourcePlayer">造成击败的玩家。</param>
    public void HandlePlayerDefeated(Player defeatedPlayer, Player sourcePlayer)
    {
        if (!IsBattleRunning || GamePlaying.instance == null || GamePlaying.instance.gameState == GameState.Over)
        {
            return;
        }

        bool hasHumanSideAlive = false;
        bool hasAiSideAlive = false;

        foreach (Player player in PlayerManager.Instance.players)
        {
            if (player == null || !player.IsAlive)
            {
                continue;
            }

            if (player is EnemyAI)
            {
                hasAiSideAlive = true;
            }
            else
            {
                hasHumanSideAlive = true;
            }
        }

        if (!hasAiSideAlive)
        {
            GamePlaying.instance.GameOver(GameResult.Win);
        }
        else if (!hasHumanSideAlive)
        {
            GamePlaying.instance.GameOver(GameResult.Defeat);
        }
    }

    private IEnumerator UpdateRound()
    {
        yield return StartCoroutine(CardManager.instance.DealCards());

        while (GamePlaying.instance != null && GamePlaying.instance.gameState != GameState.Over)
        {
            foreach (Player player in PlayerManager.Instance.players)
            {
                if (player == null || !player.IsAlive)
                {
                    continue;
                }

                CurrentTurnPlayer = player;
                player.TargetPlayerEnemyID = PlayerManager.Instance.GetFirstLivingOpponent(player)?.PlayerID ?? 0;
                player.IsSubRound = true;

                RoundState = GRoundState.Preparing;
                if (RoleManager.Instance != null)
                {
                    yield return StartCoroutine(RoleManager.Instance.EnterPreparing());
                }
                else
                {
                    yield return null;
                }

                RoundState = GRoundState.Checking;
                yield return StartCoroutine(player.EnterChecking());

                RoundState = GRoundState.GettingCard;
                yield return StartCoroutine(CardManager.instance.DealCards(player));

                RoundState = GRoundState.Battling;
                yield return StartCoroutine(player.EnterRound());

                RoundState = GRoundState.RefusingCard;
                yield return StartCoroutine(player.EnterRefusing());

                RoundState = GRoundState.Ending;
                yield return StartCoroutine(player.EnterEnding());

                player.IsSubRound = false;

                if (GamePlaying.instance == null || GamePlaying.instance.gameState == GameState.Over)
                {
                    yield break;
                }
            }
        }
    }

    private IEnumerator CheckGameOver()
    {
        while (GamePlaying.instance != null && GamePlaying.instance.gameState != GameState.Over)
        {
            yield return null;
        }

        IsBattleRunning = false;
        if (roundLoop != null)
        {
            StopCoroutine(roundLoop);
            roundLoop = null;
        }
    }
}

public enum GRoundState
{
    Preparing,
    Checking,
    GettingCard,
    Battling,
    RefusingCard,
    Ending
}
