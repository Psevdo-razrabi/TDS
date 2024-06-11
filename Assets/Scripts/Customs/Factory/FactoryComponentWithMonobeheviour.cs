﻿using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CharacterOrEnemyEffect.Factory
{
    public class FactoryComponentWithMonoBehaviour : Factory
    {
        private readonly PoolObject _poolObject;
        private readonly string _key;
        private readonly bool _usePool;
        private readonly int _countElementInPool;

        public FactoryComponentWithMonoBehaviour(PoolObject poolObject, DiContainer diContainer, bool usePool, string key, int countElementInPool) : base(diContainer)
        {
            _poolObject = poolObject;
            _usePool = usePool;
            _key = key;
            _countElementInPool = countElementInPool;
        }
        
        /// <summary>+
        /// Получение компонентов из пула
        /// </summary>
        /// <param name="gameObject">gameObject из пула.</param>
        /// <param name="T1">компонент на обьекте.</param>
        public (GameObject, T1) CreateWithPoolObject<T1>() where T1 : Object
        {
            var gameObject = GetElementInPool();
            return (gameObject, gameObject.GetComponent<T1>());
        }
        
        /// <summary>+
        /// Получение компонентов из пула
        /// </summary>
        /// <param name="gameObject">gameObject из пула.</param>
        /// <param name="T1">компонент на обьекте.</param>
        /// <param name="T2">компонент на обьекте.</param>
        public (GameObject, T1, T2) CreateWithPoolObject<T1, T2>() where T1 : Object where T2: Object
        {
            var gameObject = GetElementInPool();
            return (gameObject, gameObject.GetComponent<T1>(), gameObject.GetComponent<T2>());
        }
        
        
        /// <summary>+
        /// Получение компонентов из пула
        /// </summary>
        /// <param name="gameObject">gameObject из пула.</param>
        /// <param name="T1">компонент на обьекте.</param>
        /// <param name="T2">компонент на обьекте.</param>
        /// <param name="T3">компонент на обьекте.</param>
        public (GameObject, T1, T2, T3) CreateWithPoolObject<T1, T2, T3>() where T1 : Object where T2: Object where T3 : Object
        {
            var gameObject = GetElementInPool();
            return (gameObject, gameObject.GetComponent<T1>(), gameObject.GetComponent<T2>(), gameObject.GetComponent<T3>());
        }
        
        /// <summary>+
        /// Создание пула обьектов. Каждый тип T, компонент который будет на gameobject
        /// </summary>
        /// <param name="gameObject">Если не указать явно обьект, пулл создаст пулл новых gameObject.</param>
        /// <param name="T1">компонент на обьекте.</param>
        public void CreatePool<T1>(GameObject gameObject = null) where T1: Object
        {
            AddElementInPool(gameObject, typeof(T1));
        }
        
        /// <summary>+
        /// Создание пула обьектов. Каждый тип T, компонент который будет на gameobject
        /// </summary>
        /// <param name="gameObject">Если не указать явно обьект, пулл создаст пулл новых gameObject.</param>
        /// <param name="T1">компонент на обьекте.</param>
        /// /// <param name="T2">компонент на обьекте.</param>
        public void CreatePool<T1,T2>(GameObject gameObject = null) where T1 : Object where T2: Object
        {
            AddElementInPool(gameObject, typeof(T1), typeof(T2));
        }
        
        /// <summary>+
        /// Создание пула обьектов. Каждый тип T, компонент который будет на gameobject
        /// </summary>
        /// <param name="gameObject">Если не указать явно обьект, пулл создаст пулл новых gameObject.</param>
        /// <param name="T1">компонент на обьекте.</param>
        /// <param name="T2">компонент на обьекте.</param>
        /// <param name="T3">компонент на обьекте.</param>
        public void CreatePool<T1,T2,T3>(GameObject gameObject = null) where T1 : Object where T2: Object where T3 : Object
        {
            AddElementInPool(gameObject, typeof(T1), typeof(T2), typeof(T3));
        }

        private void AddElementInPool(GameObject gameObject = null, params Type[] componentTypes)
        {
            if(_usePool == false) return;
            _poolObject.AddElementsInPool(_key, gameObject == null ? new GameObject() : gameObject,
                _countElementInPool, componentTypes);
        }

        private GameObject GetElementInPool()
        {
            var objectPool = _poolObject.GetElementInPool(_key);
            if (objectPool == null) throw new Exception("Pool not create, create please");
            return objectPool;
        }
    }
}