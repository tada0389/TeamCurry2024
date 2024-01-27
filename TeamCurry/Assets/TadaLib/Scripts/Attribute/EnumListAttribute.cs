using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TadaLib.Attribute
{
    /// <summary>
    /// Inspectorでリストの要素名をEnumの種類名にする
    /// </summary>
    public class EnumListAttribute : PropertyAttribute
{
#region コンストラクタ
    public EnumListAttribute(System.Type enumType)
    {
        _names = System.Enum.GetNames(enumType);
    }
#endregion

#region 定義
#if UNITY_EDITOR
    /// <summary>
    /// Show inspector
    /// </summary>
    [CustomPropertyDrawer(typeof(EnumListAttribute))]
    private class EnumListDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var names = ((EnumListAttribute)attribute)._names;
            // propertyPath returns something like hogehoge.Array.data[0]
            // so get the index from there.
            var index = int.Parse(property.propertyPath.Split('[', ']').Where(c => !string.IsNullOrEmpty(c)).Last());
            if (index < names.Length) label.text = names[index];
            EditorGUI.PropertyField(rect, property, label, includeChildren: true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }
    }
#endif
#endregion

#region privateフィールド
    string[] _names;
#endregion
}
}