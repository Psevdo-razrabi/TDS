using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.Commands.Factory
{
    public class FactoryWeapon
    {
        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer) => _diContainer = diContainer;

        public GameObject CreateWeapon(GameObject weapon)
        {
            return _diContainer.InstantiatePrefab(weapon);
        }
    }
}   