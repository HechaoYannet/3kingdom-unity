using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using ReEndUnity;

/// <summary>
/// 主界面 UI 构建器 — 使用 ReEnd 组件库搭建非对称仪表盘式布局（对标明日方舟）。
/// 左侧视觉区 + 右侧垂直卡栈 + 顶部资源条 + 底部角落入口。
/// </summary>
public class HomePresenter
{
    private readonly HomeUIConfig cfg;
    private Transform root;

    // 动画缓存
    private RectTransform _bgGroup, _leftGroup, _stackGroup, _topBar, _bottomLeft, _bottomRight, _footer;
    private readonly List<RectTransform> _cards = new();

    public HomePresenter(HomeUIConfig config)
    {
        cfg = config;
    }

    /// <summary>入口：在 Canvas 下构建完整主界面。</summary>
    public void Build(Transform canvasRoot)
    {
        root = canvasRoot;
        BuildBackground();
        BuildTopBar();
        BuildLeftArtZone();
        BuildRightCardStack();
        BuildBottomCorners();
        BuildFooter();
        PlayEntryAnimation();
    }

    // ═══════════════════════════════════════════════════════════
    //  Layer 0-1: 背景层
    // ═══════════════════════════════════════════════════════════

    private void BuildBackground()
    {
        _bgGroup = CreateGroup("Background", root);
        StretchRT(_bgGroup);

        // 纯色底
        var fillGo = new GameObject("BgFill", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        fillGo.transform.SetParent(_bgGroup, false);
        StretchRT(fillGo.GetComponent<RectTransform>());
        fillGo.GetComponent<Image>().color = cfg.backgroundColor;

        // GridBackground shader 叠加层
        var gridGo = new GameObject("GridOverlay", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        gridGo.transform.SetParent(_bgGroup, false);
        StretchRT(gridGo.GetComponent<RectTransform>());
        var gridImg = gridGo.GetComponent<Image>();
        var mat = Shader.Find("ReEnd/UI/GridBackground");
        if (mat != null) gridImg.material = new Material(mat);
        gridImg.color = cfg.gridAlpha;
    }

    // ═══════════════════════════════════════════════════════════
    //  TopBar: 左侧菜单 + 日期, 右侧资源条
    // ═══════════════════════════════════════════════════════════

    private void BuildTopBar()
    {
        _topBar = CreateGroup("TopBar", root);
        _topBar.anchorMin = new Vector2(0, 1);
        _topBar.anchorMax = new Vector2(1, 1);
        _topBar.pivot = new Vector2(0.5f, 1);
        _topBar.sizeDelta = new Vector2(0, cfg.topBarHeight);
        _topBar.anchoredPosition = Vector2.zero;

        // 左侧: 菜单按钮
        var menuBtn = ReEndUI.Button(_topBar, "≡");
        menuBtn.SetVariant(ReEndVariant.Ghost);
        menuBtn.SetSize(ReEndSize.Sm);
        var menuRt = menuBtn.RectTransform;
        menuRt.anchorMin = new Vector2(0, 0.5f);
        menuRt.anchorMax = new Vector2(0, 0.5f);
        menuRt.pivot = new Vector2(0, 0.5f);
        menuRt.anchoredPosition = new Vector2(12, 0);

        // 日期
        var dateLabel = ReEndUI.Label(_topBar, DateTime.Now.ToString("yyyy/MM/dd"));
        dateLabel.SetMuted(true);
        dateLabel.SetSize(ReEndSize.Xs);
        var dateRt = dateLabel.RectTransform;
        dateRt.anchorMin = new Vector2(0, 0.5f);
        dateRt.anchorMax = new Vector2(0, 0.5f);
        dateRt.pivot = new Vector2(0, 0.5f);
        dateRt.anchoredPosition = new Vector2(56, 0);

        // 右侧: 资源条 (理智/铜钱/元宝)
        BuildResourceItem(_topBar, "理智", "180", cfg.accentCyan, 0);
        BuildResourceItem(_topBar, "铜钱", "8,420", cfg.accentGold, 1);
        BuildResourceItem(_topBar, "元宝", "3", cfg.accentGold, 2);
    }

    private void BuildResourceItem(Transform parent, string label, string value, Color color, int index)
    {
        var container = new GameObject($"Res_{label}", typeof(RectTransform));
        container.transform.SetParent(parent, false);
        var rt = container.GetComponent<RectTransform>();
        var x = -(index + 1) * (cfg.resourceItemWidth + cfg.resourceItemSpacing);
        rt.anchorMin = new Vector2(1, 0.5f);
        rt.anchorMax = new Vector2(1, 0.5f);
        rt.pivot = new Vector2(1, 0.5f);
        rt.anchoredPosition = new Vector2(x, 0);
        rt.sizeDelta = new Vector2(cfg.resourceItemWidth, 32);

        var stat = ReEndUI.Stat(container.transform, label, value);
        stat.RectTransform.anchorMin = new Vector2(0, 0);
        stat.RectTransform.anchorMax = new Vector2(1, 1);
        stat.RectTransform.offsetMin = Vector2.zero;
        stat.RectTransform.offsetMax = Vector2.zero;
    }

    // ═══════════════════════════════════════════════════════════
    //  Left Art Zone: 大标题 + 副标题
    // ═══════════════════════════════════════════════════════════

    private void BuildLeftArtZone()
    {
        _leftGroup = CreateGroup("LeftArtZone", root);
        _leftGroup.anchorMin = new Vector2(0, 0);
        _leftGroup.anchorMax = new Vector2(cfg.artZoneWidthRatio, 1);
        _leftGroup.offsetMin = Vector2.zero;
        _leftGroup.offsetMax = Vector2.zero;

        // 大标题 "三国·杀"
        var title = ReEndUI.Label(_leftGroup, "三国·杀");
        title.SetColor(cfg.accentGold);
        var titleTmp = title.Label;
        if (titleTmp != null)
        {
            titleTmp.fontSize = cfg.titleFontSize;
            titleTmp.fontStyle = FontStyles.Bold;
            titleTmp.alignment = TextAlignmentOptions.Center;
        }
        CenterInRect(title.RectTransform, _leftGroup, new Vector2(0, 60));

        // 装饰分隔线
        var sep = ReEndUI.Separator(_leftGroup, ReEndDirection.Horizontal);
        sep.SetStyle(ReEndSeparatorStyle.Glow);
        sep.SetLength(180);
        CenterInRect(sep.RectTransform, _leftGroup, new Vector2(0, 10));

        // 副标题
        var sub = ReEndUI.Label(_leftGroup, "策略卡牌对决");
        sub.SetMuted(true);
        var subTmp = sub.Label;
        if (subTmp != null)
        {
            subTmp.fontSize = cfg.subtitleFontSize;
            subTmp.alignment = TextAlignmentOptions.Center;
        }
        CenterInRect(sub.RectTransform, _leftGroup, new Vector2(0, -20));

        // TODO: 后续替换为武将立绘 + 粒子特效
    }

    // ═══════════════════════════════════════════════════════════
    //  Right Card Stack: 6 张垂直排列的 TacticalPanel
    // ═══════════════════════════════════════════════════════════

    private void BuildRightCardStack()
    {
        _stackGroup = CreateGroup("RightStack", root);
        _stackGroup.anchorMin = new Vector2(1, 0.5f);
        _stackGroup.anchorMax = new Vector2(1, 0.5f);
        _stackGroup.pivot = new Vector2(1, 0.5f);
        _stackGroup.anchoredPosition = new Vector2(-cfg.stackRightMargin, 0);

        // 卡片定义 (从上到下)
        var cardDefs = new CardDef[]
        {
            new("作  战", "Lv.15 · 理智 180/180", cfg.primaryCardHeight, true, OnBattleClick),
            new("剧本模式", "亲历三国传奇故事", cfg.standardCardHeight, false, OnScriptModeClick),
            new("编  队", "排兵布阵, 选择武将", cfg.standardCardHeight, false, OnSquadClick),
            new("图  鉴", "武将与卡牌收藏", cfg.standardCardHeight, false, OnAlbumClick),
            new("商  店", "道具与武将招募", cfg.standardCardHeight, false, OnShopClick),
            new("设  置", "", cfg.smallCardHeight, false, OnSettingsClick),
        };

        // 计算总高度
        float totalHeight = 0;
        foreach (var d in cardDefs) totalHeight += d.height;
        totalHeight += (cardDefs.Length - 1) * cfg.cardSpacing;
        _stackGroup.sizeDelta = new Vector2(cfg.stackWidth, totalHeight);

        // 从上往下排
        float cursorY = totalHeight / 2f;
        for (int i = 0; i < cardDefs.Length; i++)
        {
            var def = cardDefs[i];
            cursorY -= def.height / 2f;

            var card = BuildStackCard(_stackGroup, def, i);
            card.anchoredPosition = new Vector2(0, cursorY);

            cursorY -= def.height / 2f + cfg.cardSpacing;
            _cards.Add(card);
        }
    }

    private RectTransform BuildStackCard(Transform parent, CardDef def, int index)
    {
        var panel = ReEndUI.TacticalPanel(parent, def.isPrimary ? "" : def.title);
        var rt = panel.RectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        panel.SetWidth(cfg.stackWidth);
        panel.SetHeight(def.height);

        // 覆盖卡片背景色 — 使其比默认 theme.card (#141414) 更亮以确保可见性
        if (panel.BackgroundImage != null)
            panel.BackgroundImage.color = def.isPrimary
                ? new Color(0.10f, 0.10f, 0.16f, 1f)   // 主卡片: 带蓝调的暗色
                : new Color(0.08f, 0.08f, 0.12f, 1f);  // 普通卡片: 略暗

        if (def.isPrimary)
        {
            panel.SetStatus(ReEndStatus.Online);

            // 大标题居中
            var titleText = ReEndUI.Label(panel.ContentSlot, def.title);
            titleText.SetColor(cfg.accentGold);
            var tmp = titleText.Label;
            if (tmp != null)
            {
                tmp.fontSize = 28;
                tmp.fontStyle = FontStyles.Bold;
                tmp.alignment = TextAlignmentOptions.Center;
            }
            titleText.RectTransform.anchorMin = new Vector2(0, 0.5f);
            titleText.RectTransform.anchorMax = new Vector2(1, 0.5f);
            titleText.RectTransform.offsetMin = Vector2.zero;
            titleText.RectTransform.offsetMax = Vector2.zero;
            titleText.RectTransform.anchoredPosition = new Vector2(0, 20);

            // 等级/理智信息
            var info = ReEndUI.Label(panel.ContentSlot, def.desc);
            info.SetMuted(true);
            info.SetSize(ReEndSize.Sm);
            var infoTmp = info.Label;
            if (infoTmp != null) infoTmp.alignment = TextAlignmentOptions.Center;
            info.RectTransform.anchorMin = new Vector2(0, 0.5f);
            info.RectTransform.anchorMax = new Vector2(1, 0.5f);
            info.RectTransform.offsetMin = Vector2.zero;
            info.RectTransform.offsetMax = Vector2.zero;
            info.RectTransform.anchoredPosition = new Vector2(0, -20);
        }
        else
        {
            panel.SetStatus(ReEndStatus.Default);

            if (!string.IsNullOrEmpty(def.desc))
            {
                var descLabel = ReEndUI.Label(panel.ContentSlot, def.desc);
                descLabel.SetMuted(true);
                descLabel.SetSize(ReEndSize.Xs);
                var descTmp = descLabel.Label;
                if (descTmp != null) descTmp.alignment = TextAlignmentOptions.Left;
                descLabel.RectTransform.anchorMin = new Vector2(0, 0);
                descLabel.RectTransform.anchorMax = new Vector2(1, 0.3f);
                descLabel.RectTransform.offsetMin = new Vector2(4, 0);
                descLabel.RectTransform.offsetMax = Vector2.zero;
            }
        }

        // 添加 Button 组件使整张卡片可点击
        var btn = panel.gameObject.AddComponent<Button>();
        btn.onClick.AddListener(() => def.onClick());

        return rt;
    }

    // ═══════════════════════════════════════════════════════════
    //  Bottom Corners: 左下充值, 右下通知
    // ═══════════════════════════════════════════════════════════

    private void BuildBottomCorners()
    {
        // 左下: 充值
        _bottomLeft = CreateGroup("BottomLeft", root);
        _bottomLeft.anchorMin = new Vector2(0, 0);
        _bottomLeft.anchorMax = new Vector2(0, 0);
        _bottomLeft.pivot = new Vector2(0, 0);
        _bottomLeft.anchoredPosition = new Vector2(cfg.cornerMargin, cfg.cornerMargin);
        _bottomLeft.sizeDelta = cfg.bottomLeftSize;

        var shopBtn = ReEndUI.Button(_bottomLeft, "充值");
        shopBtn.SetVariant(ReEndVariant.Outline);
        shopBtn.SetSize(ReEndSize.Sm);
        shopBtn.SetOnClick(() => Debug.Log("[Home] 充值"));
        StretchRT(shopBtn.RectTransform);

        // 右下: 通知
        _bottomRight = CreateGroup("BottomRight", root);
        _bottomRight.anchorMin = new Vector2(1, 0);
        _bottomRight.anchorMax = new Vector2(1, 0);
        _bottomRight.pivot = new Vector2(1, 0);
        _bottomRight.anchoredPosition = new Vector2(-cfg.cornerMargin, cfg.cornerMargin);
        _bottomRight.sizeDelta = cfg.bottomRightSize;

        var notifyBtn = ReEndUI.Button(_bottomRight, "通知 (6)");
        notifyBtn.SetVariant(ReEndVariant.Outline);
        notifyBtn.SetSize(ReEndSize.Sm);
        notifyBtn.SetOnClick(() => Debug.Log("[Home] 通知"));
        StretchRT(notifyBtn.RectTransform);
    }

    // ═══════════════════════════════════════════════════════════
    //  Footer: 版本号
    // ═══════════════════════════════════════════════════════════

    private void BuildFooter()
    {
        _footer = CreateGroup("Footer", root);
        _footer.anchorMin = new Vector2(0.5f, 0);
        _footer.anchorMax = new Vector2(0.5f, 0);
        _footer.pivot = new Vector2(0.5f, 0);
        _footer.anchoredPosition = new Vector2(0, cfg.cornerMargin);
        _footer.sizeDelta = new Vector2(400, 20);

        var label = ReEndUI.Label(_footer, "v0.1.0 · Three Kingdoms · 三国·杀");
        label.SetMuted(true);
        label.SetSize(ReEndSize.Xs);
        var tmp = label.Label;
        if (tmp != null) tmp.alignment = TextAlignmentOptions.Center;
        StretchRT(label.RectTransform);
    }

    // ═══════════════════════════════════════════════════════════
    //  入场动画
    // ═══════════════════════════════════════════════════════════

    private void PlayEntryAnimation()
    {
        // 背景已就位

        // 左侧标题从左滑入
        var leftTarget = _leftGroup.anchoredPosition;
        _leftGroup.anchoredPosition = leftTarget + new Vector2(-200, 0);
        _leftGroup.DOAnchorPosX(leftTarget.x, cfg.titleSlideDuration)
            .SetEase(cfg.entrySlide);

        // 右侧卡片从右依次滑入
        for (int i = 0; i < _cards.Count; i++)
        {
            var card = _cards[i];
            var targetX = card.anchoredPosition.x;
            card.anchoredPosition += new Vector2(400, 0);
            card.DOAnchorPosX(targetX, cfg.cardSlideDuration)
                .SetEase(cfg.entrySlide)
                .SetDelay(i * cfg.cardStaggerDelay);
        }

        // TopBar 从上滑入
        var topTarget = _topBar.anchoredPosition;
        _topBar.anchoredPosition = topTarget + new Vector2(0, 48);
        _topBar.DOAnchorPosY(topTarget.y, cfg.topBarDuration)
            .SetEase(cfg.entrySlide)
            .SetDelay(0.9f);

        // 底部角落 FadeIn
        SetAlpha(_bottomLeft, 0);
        SetAlpha(_bottomRight, 0);
        DOVirtual.DelayedCall(1.0f, () =>
        {
            FadeGroup(_bottomLeft, 1f, cfg.cornerFadeDuration);
            FadeGroup(_bottomRight, 1f, cfg.cornerFadeDuration);
        });

        // Footer FadeIn
        SetAlpha(_footer, 0);
        DOVirtual.DelayedCall(1.15f, () =>
            FadeGroup(_footer, 1f, cfg.footerFadeDuration));
    }

    // ═══════════════════════════════════════════════════════════
    //  点击回调
    // ═══════════════════════════════════════════════════════════

    private void OnBattleClick()
    {
        Debug.Log("[Home] 作战 — 进入 BattleScene");
        SceneManager.LoadScene("BattleScene");
    }

    private void OnScriptModeClick()  => Debug.Log("[Home] 剧本模式 — 进入 BattleScene (script mode)");
    private void OnSquadClick()       => Debug.Log("[Home] 编队");
    private void OnAlbumClick()       => Debug.Log("[Home] 图鉴");
    private void OnShopClick()        => Debug.Log("[Home] 商店");
    private void OnSettingsClick()    => Debug.Log("[Home] 设置");

    // ═══════════════════════════════════════════════════════════
    //  Utility
    // ═══════════════════════════════════════════════════════════

    private static RectTransform CreateGroup(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go.GetComponent<RectTransform>();
    }

    private static void StretchRT(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private static void CenterInRect(RectTransform target, RectTransform container, Vector2 offset)
    {
        target.anchorMin = new Vector2(0.5f, 0.5f);
        target.anchorMax = new Vector2(0.5f, 0.5f);
        target.pivot = new Vector2(0.5f, 0.5f);
        target.anchoredPosition = offset;
    }

    private static void SetAlpha(RectTransform group, float alpha)
    {
        var cg = group.GetComponent<CanvasGroup>();
        if (cg == null) cg = group.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = alpha;
    }

    private static void FadeGroup(RectTransform group, float target, float duration)
    {
        var cg = group.GetComponent<CanvasGroup>();
        if (cg == null) cg = group.gameObject.AddComponent<CanvasGroup>();
        cg.DOFade(target, duration);
    }

    // ── 数据结构 ──

    private readonly struct CardDef
    {
        public readonly string title;
        public readonly string desc;
        public readonly float height;
        public readonly bool isPrimary;
        public readonly Action onClick;

        public CardDef(string title, string desc, float height, bool isPrimary, Action onClick)
        {
            this.title = title;
            this.desc = desc;
            this.height = height;
            this.isPrimary = isPrimary;
            this.onClick = onClick;
        }
    }
}
