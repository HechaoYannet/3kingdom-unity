using System.Collections;
using TMPro;
using UnityEngine;

namespace ReEndUnity
{
    public class ReEndGlitchText : ReEndBaseComponent
    {
        public string Text { get; set; } = "GLITCH";
        public float GlitchInterval { get; set; } = 3f;
        public float GlitchIntensity { get; set; } = 2f;
        public bool Animate { get; set; } = true;

        public TMP_Text Label { get; private set; }
        private Coroutine _glitchRoutine;
        private string _originalText;

        protected override void BuildInternal()
        {
            Label = gameObject.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Left;
            Label.raycastTarget = false;
        }

        public override void ApplyTheme()
        {
            Label.text = Text;
            Label.fontSize = Theme.h3Size;
            Label.color = Theme.primary;
        }

        public ReEndGlitchText SetText(string t) { Text = t; if (_built) ApplyTheme(); return this; }
        public ReEndGlitchText SetFontSize(float s) { if (_built) Label.fontSize = s; return this; }

        private void OnEnable()
        {
            if (Animate && _built)
                _glitchRoutine = StartCoroutine(GlitchLoop());
        }

        private void OnDisable()
        {
            if (_glitchRoutine != null)
            {
                StopCoroutine(_glitchRoutine);
                _glitchRoutine = null;
            }
        }

        private IEnumerator GlitchLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(GlitchInterval);

                _originalText = Label.text;
                float dur = 0.4f;
                float t = 0;
                while (t < dur)
                {
                    t += Time.deltaTime;
                    // Random character replacements
                    char[] chars = _originalText.ToCharArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (Random.value < 0.3f * GlitchIntensity)
                            chars[i] = (char)Random.Range(33, 127);
                    }
                    Label.text = new string(chars);
                    Label.rectTransform.anchoredPosition = new Vector2(Random.Range(-GlitchIntensity, GlitchIntensity), Random.Range(-GlitchIntensity * 0.5f, GlitchIntensity * 0.5f));
                    yield return null;
                }
                Label.text = _originalText;
                Label.rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}
