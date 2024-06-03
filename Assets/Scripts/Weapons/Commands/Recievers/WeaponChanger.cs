﻿using System.Collections.Generic;
using Game.AsyncWorker.Interfaces;
using Game.Player.Weapons.Commands.Factory;
using Game.Player.Weapons.Prefabs;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Sirenix.Utilities;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

namespace Game.Player.Weapons.Commands.Recievers
{
    public class WeaponChanger : IInitializable, IVisitWeaponType
    {
        private WeaponPrefabs _weaponPrefabs;
        private readonly WeaponPivots _weaponPivots;
        private readonly FactoryWeapon _factoryWeapon;
        private readonly IAwaiter _awaiter;
        private readonly WeaponData _weaponData;

        public WeaponChanger(WeaponPrefabs weaponPrefabs, WeaponPivots weaponPivots, FactoryWeapon factoryWeapon, IAwaiter awaiter, WeaponData weaponData) 
        {
            _weaponPrefabs = weaponPrefabs;
            _weaponPivots = weaponPivots;
            _factoryWeapon = factoryWeapon;
            _awaiter = awaiter;
            _weaponData = weaponData;
        }

        public void WeaponChange(WeaponComponent weaponComponent)
        {
            _weaponPrefabs.InitPrefab.ForEach(x => x.Value.SetActive(false));
            VisitWeapon(weaponComponent);
        }

        public async void Initialize()
        {
            await _awaiter.AwaitLoadPrefabConfigs(_weaponPrefabs);

            _weaponPrefabs.InitPrefab = new Dictionary<string, GameObject>
            {
                { _weaponPrefabs.NameLoadPistolPrefab, _factoryWeapon.CreateWeapon(_weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadPistolPrefab].weapon) },
                { _weaponPrefabs.NameLoadRiflePrefab, _factoryWeapon.CreateWeapon(_weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadRiflePrefab].weapon) },
                { _weaponPrefabs.NameLoadShotgunPrefab, _factoryWeapon.CreateWeapon(_weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadShotgunPrefab].weapon) }
            };
            
            SetPrefabPositionOnPivot(_weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadPistolPrefab], _weaponPivots.PistolPivot);
            SetPrefabPositionOnPivot(_weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadRiflePrefab], _weaponPivots.RiflePivot);
            SetPrefabPositionOnPivot(_weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadShotgunPrefab], _weaponPivots.ShotgunPivot);
            
            _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadPistolPrefab].SetActive(false);
            _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadRiflePrefab].SetActive(false);
            _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadShotgunPrefab].SetActive(false);
        }

        private void SetPrefabPositionOnPivot(GameObject prefab, GameObject pivot)
        {
            prefab.transform.SetParent(pivot.transform);
            prefab.transform.position = pivot.transform.position;
            prefab.transform.rotation = Quaternion.LookRotation(pivot.transform.forward);
        }

        public void Visit(Pistol pistol)    
        {
            _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadPistolPrefab].SetActive(true);
            _weaponData.BulletPoint =  _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadPistolPrefab].GetComponentInChildren<BulletSpawnPoint>().gameObject.transform;
        }

        public void Visit(Rifle rifle)
        {
            _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadRiflePrefab].SetActive(true);
            _weaponData.BulletPoint =  _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadRiflePrefab].GetComponentInChildren<BulletSpawnPoint>().gameObject.transform;
        }

        public void Visit(Shotgun shotgun)
        {
            _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadShotgunPrefab].SetActive(true);
            _weaponData.BulletPoint =  _weaponPrefabs.InitPrefab[_weaponPrefabs.NameLoadShotgunPrefab].GetComponentInChildren<BulletSpawnPoint>().gameObject.transform;
        }

        public void VisitWeapon(WeaponComponent component)
        {
            Visit((dynamic)component);
        }
        
    }
}