using System.Collections;
using UnityEngine;

// 专门管理3D卡牌特效的类
public class CardEffectsManager : MonoBehaviour
{
    [Header("特效预设")]
    [SerializeField] private GameObject cardDrawEffect;
    [SerializeField] private GameObject cardPlayEffect;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private GameObject healEffect;

    [Header("配置参数")]
    [SerializeField] private float cardFlyHeight = 2.0f;
    [SerializeField] private float cardFlyDuration = 0.5f;

    // 播放抽卡特效
    public void PlayDrawCardEffect(Vector3 from, Vector3 to, CardData cardData)
    {
        StartCoroutine(DrawCardEffectRoutine(from, to, cardData));
    }

    private IEnumerator DrawCardEffectRoutine(Vector3 from, Vector3 to, CardData cardData)
    {
        // 创建3D卡牌模型
        GameObject cardModel = InstantiateCardModel(cardData);
        cardModel.transform.position = from;

        // 飞行路径
        Vector3 controlPoint = (from + to) / 2 + Vector3.up * cardFlyHeight;

        float elapsed = 0;
        while (elapsed < cardFlyDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / cardFlyDuration;

            // 贝塞尔曲线移动
            cardModel.transform.position = CalculateBezierPoint(t, from, controlPoint, to);

            // 旋转卡牌使其朝向移动方向
            if (t < 0.9f)
            {
                Vector3 moveDirection = (CalculateBezierPoint(t + 0.01f, from, controlPoint, to) - cardModel.transform.position).normalized;
                if (moveDirection != Vector3.zero)
                {
                    cardModel.transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                }
            }

            yield return null;
        }

        cardModel.transform.position = to;

        // 落地后播放光效
        GameObject effect = Instantiate(cardDrawEffect, to, Quaternion.identity);
        Destroy(effect, 2.0f);

        // 销毁卡牌模型
        Destroy(cardModel, 0.5f);
    }

    // 计算贝塞尔曲线点
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return uu * p0 + 2 * u * t * p1 + tt * p2;
    }

    // 实例化3D卡牌模型
    private GameObject InstantiateCardModel(CardData cardData)
    {
        // 根据卡牌数据加载对应的3D模型
        GameObject cardPrefab = Resources.Load<GameObject>($"Cards3D/{cardData.templateID}");
        if (cardPrefab != null)
        {
            GameObject cardModel = Instantiate(cardPrefab);

            // 设置卡牌材质和纹理
            Renderer renderer = cardModel.GetComponent<Renderer>();
            if (renderer != null)
            {
                // 根据卡牌稀有度设置发光效果
                SetCardRarityEffect(renderer, cardData.rarity);
            }

            return cardModel;
        }

        return null;
    }

    // 根据卡牌稀有度设置特效
    private void SetCardRarityEffect(Renderer renderer, CardRarity rarity)
    {
        Material material = renderer.material;

        switch (rarity)
        {
            case CardRarity.Common:
                material.SetColor("_EmissionColor", Color.gray * 0.2f);
                break;
            case CardRarity.Uncommon:
                material.SetColor("_EmissionColor", Color.blue * 0.5f);
                break;
            case CardRarity.Rare:
                material.SetColor("_EmissionColor", Color.magenta * 0.8f);
                break;
            case CardRarity.Epic:
                material.SetColor("_EmissionColor", Color.yellow * 1.2f);
                break;
            case CardRarity.Legendary:
                // 传奇卡牌添加动态流光效果
                material.SetColor("_EmissionColor", Color.red * 1.5f);
                material.EnableKeyword("_EMISSION");
                break;
        }
    }

    // 播放使用卡牌特效
    public void PlayCardUseEffect(Vector3 position, Card card)
    {
        GameObject effect = Instantiate(cardPlayEffect, position, Quaternion.identity);

        // 根据卡牌类型调整特效
        ParticleSystem[] particles = effect.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
        {
            var main = ps.main;

            switch (card.CardType)
            {
                case "1":
                    main.startColor = new Color(1, 0.2f, 0.2f, 0.8f);
                    break;
                case "2":
                    main.startColor = new Color(0.2f, 0.8f, 1, 0.8f);
                    break;
                case "3":
                    main.startColor = new Color(0.8f, 0.2f, 1, 0.8f);
                    break;
            }
        }

        Destroy(effect, 3.0f);
    }
}