using System;
using Game.Player.Weapons.Decorator;
using Game.Player.Weapons.InterfaseWeapon;
using Game.Player.Weapons.ReloadStrategy;
using Game.Player.Weapons.WeaponConfigs;
using UI.Storage;
using UnityEngine;

namespace Game.Player.Weapons
{
    public class ReloadComponent : IReload
    {
        private IReloadStrategy _reloadStrategy = new ReloadImage();
        public readonly WeaponData WeaponData;
        public readonly ValueCountStorage<float> ImageReloadValue;
        public readonly BoolStorage BoolStorage;

        public ReloadComponent(WeaponData weaponData, ValueCountStorage<float> imageReloadValue, BoolStorage boolStorage)
        {
            WeaponData = weaponData;
            ImageReloadValue = imageReloadValue;
            BoolStorage = boolStorage;
        }
        
        public void Reload()
        {
            var fireAction = new ReloadAction(this, _reloadStrategy, BoolStorage);
            var handler = new HandlerDecoratorActions(() => !WeaponData.IsReloading, fireAction);
            handler.Execute();
        }

        public void ChangeReloadStrategy(IReloadStrategy reloadStrategy) //пока вопрос будет ли разные виды перезарядок
        {
            _reloadStrategy = reloadStrategy ?? throw new ArgumentNullException($"{(IReloadStrategy)null} is null");
            Debug.LogWarning($"сменил реализацию перезарядки на {_reloadStrategy.GetType()}");
        }
    }
}