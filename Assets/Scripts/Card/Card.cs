using System.Collections;
using UnityEngine;

/// <summary>
/// 战斗卡牌的规则基类。
/// </summary>
public abstract class Card : MonoBehaviour
{
    public abstract string cardName { get; protected set; }
    public abstract Sprite cardImage { get; protected set; }
    public abstract string description { get; protected set; }
    public abstract string CardType { get; protected set; }

    public int OwnerID { get; protected set; }
    protected Player LastUser { get; private set; }
    protected Player LastTarget { get; private set; }

    /// <summary>
    /// 设置当前卡牌的逻辑持有者。
    /// </summary>
    /// <param name="ownerID">玩家 ID。0 为牌堆，1 为弃牌堆，2+ 为实际角色。</param>
    public void SetOwnerByID(int ownerID)
    {
        OwnerID = ownerID;
    }

    /// <summary>
    /// 缓存本次结算上下文，供响应后续结算使用。
    /// </summary>
    /// <param name="user">出牌者。</param>
    /// <param name="target">目标。</param>
    protected void CacheResolutionContext(Player user, Player target)
    {
        LastUser = user;
        LastTarget = target;
    }

    /// <summary>
    /// 执行卡牌的主动效果。
    /// </summary>
    /// <param name="user">出牌者。</param>
    /// <param name="target">目标。</param>
    public virtual void DoCardsAction(Player user, Player target)
    {
        CacheResolutionContext(user, target);
    }

    /// <summary>
    /// 在未被有效响应时触发后续结算。
    /// </summary>
    public virtual void ResponseTrigger()
    {
    }

    /// <summary>
    /// 判断当前玩家主动阶段能否使用本牌。
    /// </summary>
    /// <param name="user">当前出牌者。</param>
    /// <returns>是否可用。</returns>
    public virtual bool IsCardAvailable(Player user)
    {
        return true;
    }

    /// <summary>
    /// 兼容旧接口的主动可用性判断。
    /// </summary>
    /// <returns>是否可用。</returns>
    public virtual bool IsCardAvailable()
    {
        return true;
    }

    /// <summary>
    /// 判断本牌能否响应给定牌型。
    /// </summary>
    /// <param name="cardType">待响应的牌型。</param>
    /// <returns>是否可响应。</returns>
    public virtual bool IsCardResponsible(string cardType)
    {
        return false;
    }

    /// <summary>
    /// 判断本牌是否会触发目标响应阶段。
    /// </summary>
    /// <returns>是否需要响应。</returns>
    public virtual bool IsCardResponsible()
    {
        return false;
    }

    protected Player GetLastUser()
    {
        return LastUser;
    }

    protected Player GetLastTarget()
    {
        return LastTarget;
    }
}
