using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndSessionTimeoutModal : ReEndBaseComponent
    {
        public int CountdownSeconds { get; set; } = 60;
        public System.Action OnContinue { get; set; }
        public System.Action OnLogout { get; set; }
        public System.Action OnTimeout { get; set; }

        public TMP_Text MessageText { get; private set; }
        public TMP_Text CountdownText { get; private set; }
        private float _remaining;
        private bool _active;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = new Color(0, 0, 0, 0.85f);
            BackgroundImage.raycastTarget = true;
            Stretch(BackgroundImage.rectTransform);

            var panelGo = CreateChild(transform, "Panel");
            var panelImg = panelGo.AddComponent<Image>();
            panelImg.color = Theme.card;
            var pRT = panelImg.rectTransform;
            pRT.anchorMin = new Vector2(0.5f, 0.5f);
            pRT.anchorMax = new Vector2(0.5f, 0.5f);
            pRT.sizeDelta = new Vector2(360, 200);

            MessageText = CreateChild<TextMeshProUGUI>(panelGo.transform, "Message");
            MessageText.alignment = TextAlignmentOptions.Center;
            var mRT = MessageText.rectTransform;
            mRT.anchorMin = new Vector2(0, 1);
            mRT.sizeDelta = new Vector2(-32, 24);
            mRT.anchoredPosition = new Vector2(16, -24);

            CountdownText = CreateChild<TextMeshProUGUI>(panelGo.transform, "Countdown");
            CountdownText.alignment = TextAlignmentOptions.Center;
            CountdownText.fontSize = 48;
            var cRT = CountdownText.rectTransform;
            cRT.anchorMin = new Vector2(0.5f, 0.5f);
            cRT.sizeDelta = new Vector2(100, 56);

            var continueBtn = ReEndUI.Button(transform, "Continue Session").SetVariant(ReEndVariant.Primary).SetSize(ReEndSize.Sm);
            continueBtn.SetOnClick(() => { _active = false; gameObject.SetActive(false); OnContinue?.Invoke(); });
            continueBtn.RectTransform.anchorMin = new Vector2(0.5f, 0);
            continueBtn.RectTransform.anchorMax = new Vector2(0.5f, 0);
            continueBtn.RectTransform.anchoredPosition = new Vector2(0, 20);

            gameObject.SetActive(false);
        }

        public void StartCountdown()
        {
            _remaining = CountdownSeconds;
            _active = true;
            gameObject.SetActive(true);
            EnsureBuilt();
            ApplyTheme();
        }

        public override void ApplyTheme()
        {
            MessageText.text = "Your session is about to expire.";
            MessageText.fontSize = Theme.bodySize;
            MessageText.color = Theme.textPrimary;
            CountdownText.color = _remaining < 15 ? Theme.efRed : Theme.textPrimary;
        }

        private void Update()
        {
            if (!_active) return;
            _remaining -= Time.deltaTime;
            CountdownText.text = Mathf.CeilToInt(_remaining).ToString();
            if (_remaining <= 0)
            {
                _active = false;
                gameObject.SetActive(false);
                OnTimeout?.Invoke();
            }
        }
    }
}
