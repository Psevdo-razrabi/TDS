using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class ObjectPool : MonoBehaviour, IFactory<GameObject>
    {
        [SerializeField] private GameObject _prefab;
        private List<GameObject> _pool = new List<GameObject>();
        [Inject] private DiContainer _container;
        public GameObject Create()
        {
            GameObject newObject = _container.InstantiatePrefab(_prefab);
            newObject.SetActive(false);
            _pool.Add(newObject);
            return newObject;
        }

        public GameObject GetFromPool()
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                if(_pool[i].activeInHierarchy) continue;
                _pool[i].SetActive(true);
                return _pool[i];
            }
            GameObject newObject = Create();
            newObject.SetActive(true);
            return newObject;
        }
    }
}