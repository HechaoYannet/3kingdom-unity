using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndStepper : ReEndBaseComponent
    {
        public int Steps { get; set; } = 3;
        public int CurrentStep { get; set; }
        public List<string> Labels { get; private set; } = new();
        public ReEndDirection Direction { get; set; } = ReEndDirection.Horizontal;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            float total = Direction == ReEndDirection.Horizontal
                ? RectTransform.rect.width : RectTransform.rect.height;
            float stepSize = total / Mathf.Max(Steps, 1);

            for (int i = 0; i < Steps; i++)
            {
                var go = CreateChild(transform, $"Step_{i}");
                bool done = i < CurrentStep;
                bool current = i == CurrentStep;

                var iconGo = CreateChild(go.transform, "Icon");
                var iconTxt = iconGo.AddComponent<TextMeshProUGUI>();
                iconTxt.text = done ? "◆" : current ? "◇" : "○";
                iconTxt.fontSize = 14;
                iconTxt.color = done ? Theme.primary
                    : current ? Theme.textPrimary : Theme.textMuted;
                iconTxt.alignment = TextAlignmentOptions.Center;

                if (i < Labels.Count)
                {
                    var labGo = CreateChild(go.transform, "Label");
                    var labTxt = labGo.AddComponent<TextMeshProUGUI>();
                    labTxt.text = Labels[i];
                    labTxt.fontSize = Theme.captionSize;
                    labTxt.color = current ? Theme.textPrimary : Theme.textMuted;
                    labTxt.alignment = TextAlignmentOptions.Center;
                }

                float pos = i * stepSize + stepSize * 0.5f;
                var rt = go.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(stepSize, 40);
                rt.anchoredPosition = Direction == ReEndDirection.Horizontal
                    ? new Vector2(pos - stepSize * 0.5f, 0) : new Vector2(0, -pos);
            }
        }

        public ReEndStepper SetSteps(int s) { Steps = s; return this; }
        public ReEndStepper SetCurrent(int c) { CurrentStep = c; if (_built) ApplyTheme(); return this; }
        public ReEndStepper SetLabels(List<string> l) { Labels = l; return this; }
        public ReEndStepper SetDirection(ReEndDirection d) { Direction = d; return this; }
    }
}
