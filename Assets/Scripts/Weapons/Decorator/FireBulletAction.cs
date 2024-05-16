using System;
using Game.Player.Weapons.InterfaceWeapon;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;

namespace Game.Player.Weapons.Decorator
{
    public class FireBulletAction : IAction
    {
        private WeaponData _weaponData;
        private FireStrategy _fireStrategy;
        private Action _fireAction;

        public FireBulletAction(WeaponData weaponData, FireStrategy fireStrategy, Action fireAction)
        {
            _weaponData = weaponData;
            _fireStrategy = fireStrategy;
            _fireAction = fireAction;
        }

        public void Execute()
        {
            Debug.LogWarning($"Shooooooooooooooooot {_fireStrategy.GetType()}");
            _fireAction();
        }
    }
}