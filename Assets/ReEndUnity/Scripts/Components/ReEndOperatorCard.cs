using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndOperatorCard : ReEndBaseComponent
    {
        public string OperatorName { get; set; } = "OPERATOR";
        public string Role { get; set; }
        public string OperatorClass { get; set; }
        public int Level { get; set; } = 1;
        public Sprite Portrait { get; set; }
        public float Atk { get; set; }
        public float Def { get; set; }
        public float Tech { get; set; }

        public Image PortraitImage { get; private set; }
        public TMP_Text NameText { get; private set; }
        public TMP_Text RoleText { get; private set; }
        public TMP_Text ClassText { get; private set; }
        public TMP_Text StatsText { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.material = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            BackgroundImage.material.SetFloat("_CornerSize", 0.08f);
            BackgroundImage.material.SetFloat("_RTOnly", 1);

            // Portrait
            PortraitImage = CreateChild<Image>(transform, "Portrait");
            PortraitImage.raycastTarget = false;
            var pRT = PortraitImage.rectTransform;
            pRT.anchorMin = new Vector2(0, 1);
            pRT.pivot = new Vector2(0, 1);
            pRT.sizeDelta = new Vector2(80, 80);
            pRT.anchoredPosition = new Vector2(8, -8);

            // Name
            NameText = CreateChild<TextMeshProUGUI>(transform, "Name");
            NameText.alignment = TextAlignmentOptions.Left;
            NameText.raycastTarget = false;
            var nRT = NameText.rectTransform;
            nRT.anchorMin = new Vector2(0, 1);
            nRT.sizeDelta = new Vector2(140, 24);
            nRT.anchoredPosition = new Vector2(96, -12);

            // Role badge
            RoleText = CreateChild<TextMeshProUGUI>(transform, "Role");
            RoleText.alignment = TextAlignmentOptions.Left;
            RoleText.raycastTarget = false;
            var rRT = RoleText.rectTransform;
            rRT.anchorMin = new Vector2(0, 1);
            rRT.sizeDelta = new Vector2(140, 16);
            rRT.anchoredPosition = new Vector2(96, -38);

            ClassText = CreateChild<TextMeshProUGUI>(transform, "Class");
            ClassText.alignment = TextAlignmentOptions.Left;
            ClassText.raycastTarget = false;
            var cRT = ClassText.rectTransform;
            cRT.anchorMin = new Vector2(0, 0);
            cRT.sizeDelta = new Vector2(60, 20);
            cRT.anchoredPosition = new Vector2(96, 20);

            // Stats (ATK / DEF / TECH)
            StatsText = CreateChild<TextMeshProUGUI>(transform, "Stats");
            StatsText.alignment = TextAlignmentOptions.Left;
            StatsText.raycastTarget = false;
            var sRT = StatsText.rectTransform;
            sRT.anchorMin = new Vector2(0, 0);
            sRT.sizeDelta = new Vector2(160, 18);
            sRT.anchoredPosition = new Vector2(96, 4);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 240);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 96);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.card;

            if (Portrait != null) PortraitImage.sprite = Portrait;
            PortraitImage.color = Theme.surface2;

            NameText.text = OperatorName;
            NameText.fontSize = Theme.bodyLgSize;
            NameText.color = Theme.textPrimary;

            RoleText.text = $"{Role}  ◆  Lv.{Level}";
            RoleText.fontSize = Theme.bodySmSize;
            RoleText.color = Theme.textMuted;

            ClassText.text = OperatorClass?.ToUpper();
            ClassText.fontSize = Theme.overlineSize;
            ClassText.color = Theme.primary;

            StatsText.text = $"ATK {Atk:F0}  DEF {Def:F0}  TECH {Tech:F0}";
            StatsText.fontSize = Theme.captionSize;
            StatsText.color = Theme.textSecondary;
        }

        public ReEndOperatorCard SetOperatorName(string n) { OperatorName = n; if (_built) ApplyTheme(); return this; }
        public ReEndOperatorCard SetRole(string r) { Role = r; if (_built) ApplyTheme(); return this; }
        public ReEndOperatorCard SetClass(string c) { OperatorClass = c; if (_built) ApplyTheme(); return this; }
        public ReEndOperatorCard SetPortrait(Sprite p) { Portrait = p; if (_built) ApplyTheme(); return this; }
        public ReEndOperatorCard SetStats(float atk, float def, float tech) { Atk = atk; Def = def; Tech = tech; if (_built) ApplyTheme(); return this; }
    }
}
