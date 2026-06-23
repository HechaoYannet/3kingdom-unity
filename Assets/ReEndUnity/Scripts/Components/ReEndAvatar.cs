using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndAvatar : ReEndBaseComponent
    {
        public Sprite Image { get; set; }
        public ReEndAvatarSize Size { get; set; } = ReEndAvatarSize.Md;
        public ReEndStatus Status { get; set; } = ReEndStatus.Offline;
        public bool ShowStatus { get; set; } = true;

        public Image AvatarImage { get; private set; }
        public Image StatusDot { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.08f);
            BackgroundImage.material = mat;
            BackgroundImage.raycastTarget = false;

            // Status dot
            var dotGo = CreateChild(transform, "StatusDot");
            StatusDot = dotGo.AddComponent<Image>();
            StatusDot.raycastTarget = false;
            var dRT = StatusDot.rectTransform;
            dRT.anchorMin = new Vector2(1, 0);
            dRT.anchorMax = new Vector2(1, 0);
            dRT.pivot = new Vector2(0.5f, 0.5f);
            dRT.sizeDelta = new Vector2(10, 10);
            dRT.anchoredPosition = new Vector2(0, 0);
        }

        public override void ApplyTheme()
        {
            float s = Size switch
            {
                ReEndAvatarSize.Xs => 24, ReEndAvatarSize.Sm => 32,
                ReEndAvatarSize.Md => 40, ReEndAvatarSize.Lg => 56,
                ReEndAvatarSize.Xl => 72, ReEndAvatarSize.Xxl => 96,
                _ => 40
            };
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, s);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, s);

            if (Image != null)
                BackgroundImage.sprite = Image;

            StatusDot.color = Status switch
            {
                ReEndStatus.Online => Theme.efGreen,
                ReEndStatus.Offline => Theme.efGrayMid,
                ReEndStatus.Warning => Theme.efOrange,
                ReEndStatus.Danger => Theme.efRed,
                _ => Theme.textMuted
            };
            StatusDot.gameObject.SetActive(ShowStatus);
        }

        public ReEndAvatar SetImage(Sprite s) { Image = s; if (_built) ApplyTheme(); return this; }
        public ReEndAvatar SetSize(ReEndAvatarSize s) { Size = s; if (_built) ApplyTheme(); return this; }
        public ReEndAvatar SetStatus(ReEndStatus s) { Status = s; if (_built) ApplyTheme(); return this; }
        public ReEndAvatar SetShowStatus(bool show) { ShowStatus = show; if (_built) ApplyTheme(); return this; }
    }
}
