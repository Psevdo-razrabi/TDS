using System;
using Game.Player.Weapons.Decorator;
using Game.Player.Weapons.InterfaseWeapon;
using Game.Player.Weapons.ReloadStrategy;
using Game.Player.Weapons.WeaponConfigs;
using UI.Storage;
using UniRx;
using UnityEngine;

namespace Game.Player.Weapons
{
    public class ReloadComponent : IReload
    {
        private IReloadStrategy _reloadStrategy = new ReloadImage();
        private CompositeDisposable _compositeDisposable = new();
        private WeaponConfigs.WeaponConfigs _weaponConfigs;
        
        public readonly WeaponData WeaponData;
        public readonly ValueCountStorage<float> ImageReloadValue;
        public readonly BoolStorage BoolStorage;
        
        public ReloadComponent(WeaponData weaponData, ValueCountStorage<float> imageReloadValue, BoolStorage boolStorage,WeaponConfigs.WeaponConfigs weaponConfigs)
        {
            WeaponData = weaponData;
            ImageReloadValue = imageReloadValue;
            BoolStorage = boolStorage;
            _weaponConfigs = weaponConfigs;
            SubscribeToReloadEnd();
        }
        
        public void Reload()
        {
            var fireAction = new ReloadAction(this, _reloadStrategy);
            var handler = new HandlerDecoratorActions(() => !WeaponData.IsReloading, fireAction);
            handler.Execute();
        }
        
        public void ChangeReloadStrategy(IReloadStrategy reloadStrategy) //пока вопрос будет ли разные виды перезарядок
        {
            _reloadStrategy = reloadStrategy ?? throw new ArgumentNullException($"{(IReloadStrategy)null} is null");
            Debug.LogWarning($"сменил реализацию перезарядки на {_reloadStrategy.GetType()}");
        }
        
        private void SubscribeToReloadEnd()
        {
            if (_reloadStrategy is ReloadImage reloadImage)
            {
                reloadImage.ReloadCompleted
                    .Subscribe(_ => BulletRecovery())
                    .AddTo(_compositeDisposable);
            }
        }

        private void BulletRecovery()
        {
            WeaponData.AmmoInMagazine.Value = _weaponConfigs.RifleConfig.TotalAmmo;
        }
    }
}