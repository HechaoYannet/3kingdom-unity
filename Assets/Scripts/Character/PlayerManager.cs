using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 维护战斗中玩家实体，并在缺少对手时创建临时 AI。
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private Player runtimeEnemyPlayer;

    public List<Player> players = new List<Player>();
    public Player TEMP_PLAYER;

    /// <summary>
    /// 当前运行时敌方玩家。
    /// </summary>
    public Player RuntimeEnemyPlayer => runtimeEnemyPlayer;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 按当前场景对象初始化战斗参与者。
    /// </summary>
    public void InitializePlayersForBattle()
    {
        players.RemoveAll(player => player == null);

        if (TEMP_PLAYER == null && players.Count > 0)
        {
            TEMP_PLAYER = players[0];
        }

        if (TEMP_PLAYER != null && !players.Contains(TEMP_PLAYER))
        {
            players.Insert(0, TEMP_PLAYER);
        }

        runtimeEnemyPlayer = EnsureRuntimeEnemy();
        if (runtimeEnemyPlayer != null && !players.Contains(runtimeEnemyPlayer))
        {
            players.Add(runtimeEnemyPlayer);
        }

        int nextPlayerId = 2;
        foreach (Player player in players)
        {
            if (player == null)
            {
                continue;
            }

            Role role = EnsureRole(player);
            player.InitializeForBattle(nextPlayerId, role);
            nextPlayerId++;
        }
    }

    /// <summary>
    /// 根据 ID 获取玩家实例。
    /// </summary>
    /// <param name="id">玩家 ID。</param>
    /// <returns>匹配到的玩家；牌堆和弃牌堆返回 null。</returns>
    public Player GetPlayerInstanceByID(int id)
    {
        if (id == 0 || id == 1)
        {
            return null;
        }

        foreach (Player player in players)
        {
            if (player != null && player.PlayerID == id)
            {
                return player;
            }
        }

        return null;
    }

    /// <summary>
    /// 获取某玩家的第一个存活对手。
    /// </summary>
    /// <param name="requester">发起查询的玩家。</param>
    /// <returns>第一个存活对手。</returns>
    public Player GetFirstLivingOpponent(Player requester)
    {
        foreach (Player player in players)
        {
            if (player == null || player == requester || !player.IsAlive)
            {
                continue;
            }

            return player;
        }

        return null;
    }

    private Player EnsureRuntimeEnemy()
    {
        if (runtimeEnemyPlayer != null)
        {
            return runtimeEnemyPlayer;
        }

        foreach (Player player in players)
        {
            if (player is EnemyAI)
            {
                return player;
            }
        }

        GameObject enemyObject = new GameObject("RuntimeEnemyAI");
        enemyObject.transform.SetParent(transform);
        runtimeEnemyPlayer = enemyObject.AddComponent<EnemyAI>();
        return runtimeEnemyPlayer;
    }

    private Role EnsureRole(Player player)
    {
        Role role = player.CurrentRole;
        if (role == null)
        {
            role = player.GetComponent<Role>();
        }

        if (role == null)
        {
            role = player.gameObject.AddComponent<HuangGai>();
        }

        RoleManager.Instance?.RoleInitiation(role);
        return role;
    }
}
