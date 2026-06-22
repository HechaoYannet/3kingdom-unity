using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndRadioGroup : ReEndBaseComponent
    {
        public List<string> Options { get; private set; } = new();
        public int SelectedIndex { get; set; } = -1;
        public System.Action<int> OnValueChanged { get; set; }

        private List<ReEndCheckbox> _items = new();

        protected override void BuildInternal()
        {
            // radio items are added dynamically via AddOption
        }

        public override void ApplyTheme()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].SetChecked(i == SelectedIndex);
                _items[i].RefreshTheme();
            }
        }

        public ReEndRadioGroup AddOption(string label)
        {
            Options.Add(label);
            var item = ReEndUI.Checkbox(transform, label);
            item.SetChecked(Options.Count - 1 == SelectedIndex);
            int idx = Options.Count - 1;
            item.SetOnValueChanged(v =>
            {
                if (v)
                {
                    SelectedIndex = idx;
                    RefreshTheme();
                    OnValueChanged?.Invoke(idx);
                }
            });
            item.RectTransform.anchoredPosition = new Vector2(0, -(Options.Count - 1) * 32);
            _items.Add(item);
            return this;
        }

        public ReEndRadioGroup SetOptions(List<string> options)
        {
            Options = options;
            for (int i = 0; i < options.Count; i++)
                AddOption(options[i]);
            return this;
        }

        public ReEndRadioGroup SetSelected(int idx) { SelectedIndex = idx; if (_built) ApplyTheme(); return this; }
        public ReEndRadioGroup SetOnValueChanged(System.Action<int> cb) { OnValueChanged = cb; return this; }
    }
}
