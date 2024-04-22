using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class PoolObject<TComponent> where TComponent : Object
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

    public void AddElementsInPool(string keyObjectInPool, GameObject objectInPool, float countElementsWillBeInPool = 1f)
    {
        Debug.Log(1);
        if (_poolObject.ContainsKey(keyObjectInPool))
        {
            AddElement(countElementsWillBeInPool, keyObjectInPool, objectInPool);
        }
        else 
        {
            _poolObject.Add(keyObjectInPool, new Queue<ObjectInPool>());
            AddElement(countElementsWillBeInPool, keyObjectInPool, objectInPool); 
        }
    }

    private void AddElement(float countElementsWillBeInPool, string keyObjectInPool, GameObject objectInPool)
    {
        for (var i = 0; i < countElementsWillBeInPool; i++)
        {
            AddObjectInPool(keyObjectInPool, objectInPool, false);
        }
    }

    private TComponent AddObjectInPool(string keyObjectInPool, GameObject prefabObjectObject, bool isActive)
    {
        var objectInPool = CreateNewObjectWithComponent(prefabObjectObject);
        objectInPool.GameObject().SetActive(isActive);
        _poolObject[keyObjectInPool].Enqueue(new ObjectInPool(objectInPool));
        return objectInPool;
    }
        
    private TComponent CreateNewObjectWithComponent(GameObject prefabObjectObject)
    {
        return _container.InstantiatePrefabForComponent<TComponent>(prefabObjectObject);
    }

    public TComponent GetElementInPool(string keyObjectInPool)
    {
        if (HasFreeElementInPool(out var objectInPool, keyObjectInPool))
        {
            return objectInPool;
        }

        if (AutoExpandPool)
            return _poolObject[keyObjectInPool]
                .Where(objectPool => objectPool.PrefabObjectObject.GameObject().activeInHierarchy)
                .Select(objectPool => AddObjectInPool(keyObjectInPool, objectPool.PrefabObjectObject.GameObject(), false))
                .FirstOrDefault();
        Debug.LogWarning($"parameter {nameof(AutoExpandPool)} false and object dont create automatically create manually");
        return null;

    }

    private bool HasFreeElementInPool(out TComponent objectInPool, string keyObjectInPool)
    {
        foreach (var objectPool in _poolObject[keyObjectInPool].Where(objectPool => !objectPool.PrefabObjectObject.GameObject().activeInHierarchy))
        {
            objectInPool = objectPool.PrefabObjectObject;
            objectInPool.GameObject().SetActive(true);
            return true;
        }

        objectInPool = null;
        return false;
    }
        
    private class ObjectInPool
    {
        public readonly TComponent PrefabObjectObject;
            
        public ObjectInPool(TComponent prefabObjectObject)
        {
            if (prefabObjectObject == null)
                throw new ArgumentNullException($"One of the arguments construct is null {prefabObjectObject} or {prefabObjectObject}");
                
            PrefabObjectObject = prefabObjectObject;
        }
    }
}
