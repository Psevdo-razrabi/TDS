using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Player.Weapons;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Input;
using Input.Interface;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

namespace Customs
{
    public class SetFireMode : IVisitWeaponType, IInitializable
    {
        private readonly ISetFireModes _changeModeFire;
        private readonly Type _typeFireMode = typeof(ChangeModeFire);
        private readonly WeaponConfigs _weaponConfigs;
        private List<MethodInfo> _methodInfos;
        private MethodInfo[] _allMethodUseAttribute;
        private int _index;

        public SetFireMode(ISetFireModes changeModeFire,WeaponConfigs weaponConfigs)
        {
            _changeModeFire = changeModeFire;
            _weaponConfigs = weaponConfigs;
        }

        public void Initialize()
        {
            _allMethodUseAttribute = _typeFireMode
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.IsDefined(typeof(ContextMenuAttribute), false))
                .ToArray();
        }

        public void Visit(Pistol pistol)
        {
            AddFireModes(_weaponConfigs.PistolConfig);
        }

        public void Visit(Rifle rifle)
        {
            AddFireModes(_weaponConfigs.RifleConfig);
        }

        public void Visit(Shotgun shotgun)
        {
            AddFireModes(_weaponConfigs.ShotgunConfig);
        }

        public void VisitWeapon(WeaponComponent component)
        {
            _methodInfos = new List<MethodInfo>();
            for (_index = 0; _index < _allMethodUseAttribute.Length; _index++)
            {
                Visit((dynamic)component);
            }
        }
        
        public void SetFireModes(WeaponComponent weaponComponent)
        {
            VisitWeapon(weaponComponent);
            _changeModeFire.SetFireModes(SetMethod());
        }
        
        private List<MethodInfo> SetMethod()
        {
            return new List<MethodInfo>(_methodInfos);
        }
        
        private void AddFireModes(BaseWeaponConfig config)
        {
            if (config.SingleFire && _allMethodUseAttribute[_index].Name == "AddSingleFire")
            {
                _methodInfos.Add(_allMethodUseAttribute[_index]);
                Debug.Log("СИНГЛЬ");
            }

            if (config.BurstFire && _allMethodUseAttribute[_index].Name == "AddBurstFire")
            {
                _methodInfos.Add(_allMethodUseAttribute[_index]);
                Debug.Log("БИРСТ");
            }

            if (config.AutomaticFire && _allMethodUseAttribute[_index].Name == "AddAutomaticFire")
            {
                _methodInfos.Add(_allMethodUseAttribute[_index]);
                Debug.Log("АВТАМАТИК");
            }
        }
    }
}