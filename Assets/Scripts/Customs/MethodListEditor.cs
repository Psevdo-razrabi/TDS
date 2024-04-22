using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Customs;
using Input;
using UnityEditor;
using UnityEditor.Profiling;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ChangeModeFire))]
public class MethodListEditor : Editor
{
    private ReorderableList List;
    private ReorderableList _list;
    private SerializedProperty _listProperty;
    private List<MethodInfo> _method;


    private void OnEnable()
    {
        _method = typeof(ChangeModeFire)
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.IsDefined(typeof(ContextMenuAttribute), false))
            .ToList();
        
        _listProperty = serializedObject.FindProperty("mode");
        
        List = new ReorderableList(_method, typeof(ChangeModeFire), true, true, true, true);
        _list = new ReorderableList(serializedObject, _listProperty, true, true, true, true);
        
        _list.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var method = (MethodInfo)List.list[index];
            var attribute = method.GetCustomAttribute<ContextMenuAttribute>();

            var element = _list.serializedProperty.GetArrayElementAtIndex(index);
            var fireMode = element.FindPropertyRelative("fireMode");
            fireMode.stringValue = method.Name;
            serializedObject.ApplyModifiedProperties();
            
            EditorGUI.PropertyField(rect, fireMode, new GUIContent(attribute.Label));
            
            var buttonRemove = new Rect(rect.x + rect.width - 20, rect.y, 20, 20);

            if (GUI.Button(buttonRemove, "Remove"))
            {
                RemoveElement(index);
            }
        };
    }
    
    private void RemoveElement(int index)
    {
        _listProperty.DeleteArrayElementAtIndex(index);
        _list.list.RemoveAt(index);
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        _list.DoLayoutList();
        _list.list = new List<MethodInfo>(_method);
        serializedObject.ApplyModifiedProperties();
    }
}
