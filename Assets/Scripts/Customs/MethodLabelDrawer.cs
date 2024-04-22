using UnityEditor;
using UnityEngine;

namespace Customs
{
    [CustomPropertyDrawer(typeof(ContextMenuAttribute), true)]
    public class MethodLabelDrawer : PropertyDrawer
    {
        private SerializedProperty _labelProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _labelProperty = property.FindPropertyRelative("Label");
            
            EditorGUI.PropertyField(position, _labelProperty, GUIContent.none);
        }
    }
}