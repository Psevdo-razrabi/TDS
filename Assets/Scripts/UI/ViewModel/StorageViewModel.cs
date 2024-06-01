using System;
using Game.Player.Weapons.WeaponClass;
using MVVM;
using UI.Storage;
using Zenject;

namespace UI.ViewModel
{
    public class StorageViewModel : IInitializable, IDisposable
    {
        private readonly StorageModel _storageModel;
        private readonly DiContainer _diContainer;

        public StorageViewModel(StorageModel storageModel, DiContainer diContainer)
        {
            _storageModel = storageModel;
            _diContainer = diContainer;
        }

        [Method("Pistol Equip")]
        public void PistolEquip()
        {
            _storageModel.ChangeWeapon(_diContainer.Resolve<Pistol>());
        }
        
        [Method("Rifle Equip")]
        public void RifleEquip()
        {
            _storageModel.ChangeWeapon(_diContainer.Resolve<Rifle>());
        }
        
        [Method("Shotgun Equip")]
        public void ShotgunEquip()
        {
            _storageModel.ChangeWeapon(_diContainer.Resolve<Shotgun>());
        }
        
        public void Dispose()
        {
            
        }

        public void Initialize()
        {
            
        }
    }
}