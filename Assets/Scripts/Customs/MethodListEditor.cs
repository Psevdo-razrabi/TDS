using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Customs;
using Customs.Data;
using Cysharp.Threading.Tasks;
using Input;
using Newtonsoft.Json;
using SaveSystem;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Zenject;

[CustomEditor(typeof(ChangeModeFire))]
public class MethodListEditor : Editor
{
    private ReorderableList _list;
    private Type _typeClassForEditor;
    private List<string> _methodNames = new();
    private readonly List<int> _methodIndices = new();
    private List<string> _methodLabels = new();
    private UnitOfWorks _unitOfWorks;
    private IdGenerator _idGenerator;
    private const string PlayerPrefsKey = "SetFireMode";
    private const string PlayerPrefsKeyLabels = "MethodLabels";
    
    [Inject]
    public async UniTask Construct(UnitOfWorks unitOfWorks, IdGenerator generator)
    {
        _unitOfWorks = unitOfWorks;
        _idGenerator = generator;
        
         _unitOfWorks.RepositoryNameMethod
             .SetToCollectionItems(_methodNames
                 .Select(x => new MethodName(_idGenerator
                     .GenerateId(), x))
                 .ToList());
         _unitOfWorks.RepositoryLabelName
             .SetToCollectionItems(_methodLabels
                 .Select(x => new MethodLabelName(_idGenerator
                     .GenerateId(), x))
                 .ToList());
         
        await _unitOfWorks.Save();
    }
    
    private void OnEnable()
    {
        _typeClassForEditor = typeof(ChangeModeFire);
        
        AddElementsInList(FindAllMethods());
        
        Load();
        Save();
        
        InspectorDraw();
    }

    private List<MethodInfo> FindAllMethods()
    {
        return _typeClassForEditor
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.IsDefined(typeof(ContextMenuAttribute), false))
            .ToList();
    }
    
    private void AddElementsInList(List<MethodInfo> methods)
    {
        foreach (var method in methods)
        {
            _methodNames.Add(method.Name);
            _methodLabels.Add(method.GetCustomAttribute<ContextMenuAttribute>().Label);
            _methodIndices.Add(method.MetadataToken);
        }
    }

    private void InspectorDraw()
    {
        var property = serializedObject.FindProperty("fireMode");

        _list = new ReorderableList(_methodNames, typeof(string), true, true, true, true);
        _list.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            property.stringValue = _methodNames[index];
            EditorGUI.PropertyField(rect, property, new GUIContent(_methodLabels[index]));
        };
    }
    
    public override void OnInspectorGUI()
    {
        _list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        Save();
        
        //EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Swap Up"))
        {
            MoveUp(-1);
        }

        if (GUILayout.Button("Swap Down"))
        {
            MoveDown(1);
        }

        //EditorGUILayout.EndHorizontal();
    }

    private void MoveUp(int sign)
    {
        if (_list.index > 0)
        { 
            MovementWithHelpSign(sign);
        }
    }

    private void MoveDown(int sign)
    {
        if (_list.index < _methodNames.Count - 1)
        {
            MovementWithHelpSign(sign);
        }
    }
    
    private void Swap(int selectedIndexOne, int selectedIndexTwo)
    {
        (_methodNames[selectedIndexOne], _methodNames[selectedIndexTwo])
            = (_methodNames[selectedIndexTwo], _methodNames[selectedIndexOne]);
        
        (_methodLabels[selectedIndexOne], _methodLabels[selectedIndexTwo])
            = (_methodLabels[selectedIndexTwo], _methodLabels[selectedIndexOne]);

        (_methodIndices[selectedIndexOne], _methodIndices[selectedIndexTwo])
            = (_methodIndices[selectedIndexTwo], _methodIndices[selectedIndexOne]);
    }

    private void Save()
    {
        var jsonLabels = JsonConvert.SerializeObject(_methodLabels, Formatting.Indented);
        var jsonNames = JsonConvert.SerializeObject(_methodNames, Formatting.Indented);
    
        PlayerPrefs.SetString(PlayerPrefsKeyLabels, jsonLabels);
        PlayerPrefs.SetString(PlayerPrefsKey, jsonNames);
    }

    private void Load()
    {
        var jsonLabel = PlayerPrefs.GetString(PlayerPrefsKeyLabels);
        _methodLabels = JsonConvert.DeserializeObject<List<string>>(jsonLabel);
    
        var jsonName = PlayerPrefs.GetString(PlayerPrefsKey);
        _methodNames = JsonConvert.DeserializeObject<List<string>>(jsonName);
    }
    
    private void MovementWithHelpSign(int sign)
    {
        var selectIndex = _list.index;
        Swap(selectIndex, selectIndex + sign);
        _list.index = selectIndex + sign;
    }
}
