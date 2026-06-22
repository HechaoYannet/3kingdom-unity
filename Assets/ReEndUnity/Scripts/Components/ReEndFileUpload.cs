using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndFileUpload : ReEndBaseComponent
    {
        public string Label { get; set; } = "Drop files here or click to upload";
        public List<string> AcceptedTypes { get; set; } = new() { ".png", ".jpg", ".pdf" };
        public bool MultiFile { get; set; }
        public int MaxFiles { get; set; } = 5;
        public long MaxSizeBytes { get; set; } = 10 * 1024 * 1024;
        public System.Action<string[]> OnFilesSelected { get; set; }

        public TMP_Text LabelText { get; private set; }
        public Image DropZone { get; private set; }
        private Button _btn;
        private List<string> _files = new();

        protected override void BuildInternal()
        {
            DropZone = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            DropZone.raycastTarget = true;
            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(OpenFileDialog);

            var textGo = CreateChild(transform, "Label");
            LabelText = textGo.AddComponent<TextMeshProUGUI>();
            LabelText.alignment = TextAlignmentOptions.Center;
            LabelText.raycastTarget = false;
            Stretch(LabelText.rectTransform);
        }

        private void OpenFileDialog()
        {
            // Note: Unity standalone uses System.Windows.Forms or Unity's file picker via NativeGallery
            // This is a simplified version that shows a status message
#if UNITY_STANDALONE_WIN
            var exts = string.Join(";", AcceptedTypes);
            var paths = System.Windows.Forms.OpenFileDialog(); // pseudo
            if (paths != null)
            {
                _files.AddRange(paths);
                ApplyTheme();
                OnFilesSelected?.Invoke(_files.ToArray());
            }
#endif
        }

        public override void ApplyTheme()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120);

            DropZone.color = Theme.surface2;
            LabelText.text = _files.Count > 0
                ? $"{_files.Count} file(s) selected"
                : Label;
            LabelText.fontSize = Theme.bodySize;
            LabelText.color = _files.Count > 0 ? Theme.efGreen : Theme.textMuted;
        }

        public ReEndFileUpload SetLabel(string l) { Label = l; if (_built) ApplyTheme(); return this; }
        public ReEndFileUpload SetAcceptedTypes(List<string> types) { AcceptedTypes = types; return this; }
        public ReEndFileUpload SetMultiFile(bool m) { MultiFile = m; return this; }
        public ReEndFileUpload SetOnFilesSelected(System.Action<string[]> cb) { OnFilesSelected = cb; return this; }
    }
}
