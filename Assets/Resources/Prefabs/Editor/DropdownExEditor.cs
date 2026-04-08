using UnityEditor; //引用编辑器的脚本，需要放在 Editor 目录中
using UnityEditor.UI;


/// <summary>
/// 此编辑器脚本：用于在层次面板中添加一个可控属性AlwaysCallback/arrow
/// </summary>
[CustomEditor(typeof(DropdownPro), true)]
[CanEditMultipleObjects]
public class DropdownExEditor : DropdownEditor
{
    SerializedProperty AlwaysCallback;
    SerializedProperty arrow;

    protected override void OnEnable()
    {
        base.OnEnable();
        AlwaysCallback = serializedObject.FindProperty("AlwaysCallback");
        arrow = serializedObject.FindProperty("arrow");
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(AlwaysCallback);
        EditorGUILayout.PropertyField(arrow);
        serializedObject.ApplyModifiedProperties();
    }
}