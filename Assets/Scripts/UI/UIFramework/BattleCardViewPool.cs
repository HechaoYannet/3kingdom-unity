using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// BattleHandCardView 对象池，避免手牌变化时频繁 Instantiate/Destroy。
/// </summary>
public class BattleCardViewPool : MonoBehaviour
{
    [SerializeField] private BattleHandCardView cardViewPrefab;
    [SerializeField] private int initialPoolSize = 12;

    private readonly Queue<BattleHandCardView> pool = new Queue<BattleHandCardView>();

    /// <summary>
    /// 初始化对象池，预实例化一定数量的卡牌视图。
    /// </summary>
    /// <param name="prefab">卡牌视图预制体。</param>
    /// <param name="prepoolCount">预池化数量。</param>
    public void Initialize(BattleHandCardView prefab, int prepoolCount)
    {
        cardViewPrefab = prefab;
        initialPoolSize = prepoolCount;
        for (int i = 0; i < initialPoolSize; i++)
        {
            BattleHandCardView view = CreatePooledInstance();
            pool.Enqueue(view);
        }
    }

    /// <summary>
    /// 从池中获取一个卡牌视图，池空时自动新建。
    /// </summary>
    /// <param name="parent">父节点。</param>
    /// <returns>可用的卡牌视图。</returns>
    public BattleHandCardView Get(Transform parent)
    {
        BattleHandCardView view = pool.Count > 0 ? pool.Dequeue() : CreatePooledInstance();
        // 确保残留 Tween 和状态已清除
        view.ResetForPool();
        view.transform.SetParent(parent, false);
        view.gameObject.SetActive(true);
        return view;
    }

    /// <summary>
    /// 将卡牌视图归还池中，重置状态并杀死残留动画。
    /// </summary>
    /// <param name="view">待归还的卡牌视图。</param>
    public void Return(BattleHandCardView view)
    {
        if (view == null) return;

        // 杀死所有残留 Tween，防止跨池化生命周期继续运行
        view.RectTransform.DOKill();
        CanvasGroup cg = view.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.DOKill();
            cg.blocksRaycasts = true;
        }

        view.ResetForPool();
        view.gameObject.SetActive(false);
        view.transform.SetParent(transform, false);
        pool.Enqueue(view);
    }

    private BattleHandCardView CreatePooledInstance()
    {
        BattleHandCardView view = Instantiate(cardViewPrefab, transform);
        view.gameObject.SetActive(false);
        return view;
    }
}
