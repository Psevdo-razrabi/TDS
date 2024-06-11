using CharacterOrEnemyEffect.Factory;
using UnityEngine;
using Zenject;

namespace CharacterOrEnemyEffect
{
    public class CreateVFXTrail : IInitializable
    {
        private FactoryComponent _factoryComponent;
        private FactoryComponentWithMonoBehaviour _factoryMeshFilter;
    
        public CreateVFXTrail(FactoryComponent factoryComponent, FactoryComponentWithMonoBehaviour factoryMeshFilter)
        {
            _factoryComponent = factoryComponent;
            _factoryMeshFilter = factoryMeshFilter;
        }

        public (GameObject gameObject, MeshFilter meshFilter, MeshRenderer meshRenderer, Mesh mesh) Create()
        {
            var meshFilter = _factoryMeshFilter.CreateWithPoolObject<MeshFilter, MeshRenderer>();
            var mesh = _factoryComponent.CreateComponentFromNew<Mesh>();

            return (meshFilter.Item1, meshFilter.Item2, meshFilter.Item3, mesh);
        }

        public void Initialize()
        {
            _factoryMeshFilter.CreatePool<MeshFilter, MeshRenderer>();
        }
    }
}