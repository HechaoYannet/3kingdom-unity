using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndCommandPalette : ReEndBaseComponent
    {
        public string Placeholder { get; set; } = "Type a command...";
        public List<CommandItem> Commands { get; private set; } = new();

        [System.Serializable]
        public class CommandItem
        {
            public string Id;
            public string Label;
            public string Category;
            public System.Action OnExecute;
        }

        public TMP_InputField SearchInput { get; private set; }
        private GameObject _resultsList;
        private List<CommandItem> _filtered = new();

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.popover;
            var mat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
            mat.SetFloat("_CornerSize", 8);
            BackgroundImage.material = mat;

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 480);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 320);

            // Search
            var searchGo = CreateChild(transform, "Search");
            var searchBg = searchGo.AddComponent<Image>();
            searchBg.color = Theme.surface1;
            var sRT = searchBg.rectTransform;
            sRT.anchorMin = new Vector2(0, 1);
            sRT.anchorMax = new Vector2(1, 1);
            sRT.pivot = new Vector2(0, 1);
            sRT.sizeDelta = new Vector2(0, 44);

            SearchInput = searchGo.AddComponent<TMP_InputField>();
            var textGo = CreateChild(searchGo.transform, "Text");
            var text = textGo.AddComponent<TextMeshProUGUI>();
            text.alignment = TextAlignmentOptions.Left;
            text.fontSize = 16;
            text.color = Theme.textPrimary;
            var tRT = text.rectTransform;
            tRT.offsetMin = new Vector2(16, 0);
            tRT.offsetMax = Vector2.zero;
            Stretch(tRT);
            SearchInput.textComponent = text;
            SearchInput.onValueChanged.AddListener(Filter);

            // Results
            _resultsList = CreateChild(transform, "Results");
            var rRT = _resultsList.GetComponent<RectTransform>();
            rRT.anchorMin = new Vector2(0, 0);
            rRT.anchorMax = new Vector2(1, 1);
            rRT.offsetMin = new Vector2(0, 4);
            rRT.offsetMax = new Vector2(0, -52);

            gameObject.SetActive(false);
        }

        private void Filter(string query)
        {
            _filtered.Clear();
            if (string.IsNullOrWhiteSpace(query))
            {
                _filtered.AddRange(Commands);
            }
            else
            {
                var q = query.ToLower();
                foreach (var c in Commands)
                    if (c.Label.ToLower().Contains(q) || c.Category.ToLower().Contains(q))
                        _filtered.Add(c);
            }
            RenderResults();
        }

        private void RenderResults()
        {
            foreach (Transform t in _resultsList.transform) Destroy(t.gameObject);
            float y = 0;
            for (int i = 0; i < Mathf.Min(_filtered.Count, 10); i++)
            {
                var cmd = _filtered[i];
                var btn = ReEndUI.Button(_resultsList.transform, cmd.Label).SetVariant(ReEndVariant.Ghost).SetSize(ReEndSize.Sm);
                btn.SetOnClick(() => { cmd.OnExecute?.Invoke(); Close(); });
                btn.RectTransform.anchoredPosition = new Vector2(0, -y);
                y += 36;
            }
        }

        public override void ApplyTheme()
        {
            SearchInput.text = "";
            Filter("");
        }

        public ReEndCommandPalette AddCommand(CommandItem cmd) { Commands.Add(cmd); return this; }
        public void Open() { gameObject.SetActive(true); EnsureBuilt(); ApplyTheme(); SearchInput.Select(); SearchInput.ActivateInputField(); }
        public void Close() { gameObject.SetActive(false); }
    }
}
