using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PoolObject
{
    private Dictionary<string, Queue<ObjectInPool>> _poolObject;
    private DiContainer _container;

    public bool AutoExpandPool { get; private set; } = true;
        
    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container ?? throw new ArgumentNullException($"{nameof(container)} is null");
            
        _poolObject = new Dictionary<string, Queue<ObjectInPool>>();
    }

    public void AddElementsInPool(string keyObjectInPool, GameObject objectInPool, int countElementsWillBeInPool = 1, params Type[] typesComponent)
    {
        CanAddInPool(keyObjectInPool, objectInPool, countElementsWillBeInPool, typesComponent);
    }

    private void CanAddInPool(string keyObjectInPool, GameObject objectInPool, int countElementsWillBeInPool = 1, params Type[] typesComponent)
    {
        if (_poolObject.ContainsKey(keyObjectInPool))
        {
            AddElement(countElementsWillBeInPool, keyObjectInPool, objectInPool, typesComponent);
        }
        else 
        {
            _poolObject.Add(keyObjectInPool, new Queue<ObjectInPool>());
            AddElement(countElementsWillBeInPool, keyObjectInPool, objectInPool, typesComponent); 
        }
    }

    private void AddElement(int countElementsWillBeInPool, string keyObjectInPool, GameObject objectInPool, params Type[] typesComponent)
    {
        for (var i = 0; i < countElementsWillBeInPool; i++)
        {
            AddObjectInPool(keyObjectInPool, objectInPool, false, typesComponent);
        }
    }

    private GameObject AddObjectInPool(string keyObjectInPool, GameObject prefabObjectObject, bool isActive, params Type[] typesComponent)
    {
        var objectInPool = CreateNewObjectWithComponent(prefabObjectObject, typesComponent);
        objectInPool.GameObject().SetActive(isActive);
        _poolObject[keyObjectInPool].Enqueue(new ObjectInPool(objectInPool));
        return objectInPool;
    }
        
    private GameObject CreateNewObjectWithComponent(GameObject prefabObjectObject, params Type[] typesComponent)
    {
        var prefab = _container.InstantiatePrefab(prefabObjectObject);
        
        typesComponent.ForEach(x => prefab.AddComponent(x));
        return prefab;
    }

    public GameObject GetElementInPool(string keyObjectInPool)
    {
        if (HasFreeElementInPool(out var objectInPool, keyObjectInPool))
        {
            return objectInPool;
        }

        if (AutoExpandPool)
            return _poolObject[keyObjectInPool]
                .Where(objectPool => objectPool.PrefabObject.GameObject().activeInHierarchy)
                .Select(objectPool => AddObjectInPool(keyObjectInPool, objectPool.PrefabObject.GameObject(), false))
                .FirstOrDefault();
        Debug.LogWarning($"parameter {nameof(AutoExpandPool)} false and object dont create automatically create manually");
        return null;
    }

    private bool HasFreeElementInPool(out GameObject objectInPool, string keyObjectInPool)
    {
        foreach (var objectPool in _poolObject[keyObjectInPool].Where(objectPool => !objectPool.PrefabObject.GameObject().activeInHierarchy))
        {
            objectInPool = objectPool.PrefabObject;
            objectInPool.GameObject().SetActive(true);
            return true;
        }

        objectInPool = null;
        return false;
    }
        
    private class ObjectInPool
    {
        public readonly GameObject PrefabObject;
            
        public ObjectInPool(GameObject prefabObject)
        {
            if (prefabObject == null)
                throw new ArgumentNullException($"One of the arguments construct is null {prefabObject} or {prefabObject}");
                
            PrefabObject = prefabObject;
        }
    }
}
