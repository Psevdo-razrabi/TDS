using UnityEngine;
using Zenject;

namespace CharacterOrEnemyEffect.Factory
{
    public class FactoryGameObject
    {
        private DiContainer _diContainer;

        public FactoryGameObject(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public GameObject CreateGameObject(GameObject gameObject)
        {
            return _diContainer.InstantiatePrefab(gameObject);
        }
    }
}