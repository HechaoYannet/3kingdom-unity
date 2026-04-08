using UnityEngine;
using TMPro;

/// <summary>
/// 3D 卡牌视图 —— 更新卡图、文字、属性显示
/// 假设卡牌预制体结构如下：
/// - 根物体：带 Collider、Card3D 脚本
///   - 子物体：CardFront（Quad 或带材质的 Mesh，用于显示卡图）
///   - 子物体：Canvas_WorldSpace（内含 TextMeshPro 显示费用、攻击、生命、名称等）
/// </summary>
public class CardView3D : MonoBehaviour
{
    [Header("卡牌模型组件")]
    public MeshRenderer frontRenderer;      // 正面 Quad
    public MeshRenderer backRenderer;       // 背面 Quad

    [Header("材质设置")]
    public Texture2D defaultCardBack;       // 默认卡背纹理

    [Header("文字显示")]
    public TextMeshPro nameText;
    public TextMeshPro manaText;
    public TextMeshPro attackText;
    public TextMeshPro healthText;

    private Card3D card3D;
    private MaterialPropertyBlock propBlock; // 用于高效修改材质属性


    private void Awake()
    {
        card3D = GetComponent<Card3D>();
        if (card3D != null)
            card3D.OnCardDataChanged += RefreshUI;

        propBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (card3D == null || card3D.cardData == null) return;

        Card data = card3D.cardData;

        // 设置正面卡图
        if (frontRenderer != null)
        {
            // 使用 MaterialPropertyBlock 避免创建新材质
            frontRenderer.GetPropertyBlock(propBlock);
            //if (data.cardImage != null)
            //{
            //    propBlock.SetTexture("_MainTex", data.cardImage.texture);
            //}
            //else
            //{
            //    propBlock.SetTexture("_MainTex", null);
            //}
            frontRenderer.SetPropertyBlock(propBlock);
        }

        // 设置背面卡背（始终不变）
        if (backRenderer != null && defaultCardBack != null)
        {
            backRenderer.GetPropertyBlock(propBlock);
            //propBlock.SetTexture("_MainTex", defaultCardBack);
            backRenderer.SetPropertyBlock(propBlock);
        }

        // 更新文字
        if (nameText != null) nameText.text = data.cardName;
    }

    private void OnDestroy()
    {
        if (card3D != null)
            card3D.OnCardDataChanged -= RefreshUI;
    }
}