using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Player.Weapons;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Input;
using Input.Interface;
using Weapons.InterfaceWeapon;
using Zenject;

namespace Customs
{
    public class SetFireMode : IVisitWeaponType, IInitializable
    {
        private readonly ISetFireModes _changeModeFire;
        private readonly Type _typeFireMode = typeof(ChangeModeFire);
        private readonly Weapon _weapon;
        private List<MethodInfo> _methodInfos;
        private MethodInfo[] _allMethodUseAttribute;
        private int _index;

        public SetFireMode(ISetFireModes changeModeFire, Weapon weapon)
        {
            _changeModeFire = changeModeFire;
            _weapon = weapon;
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
            AddFireModes(_weapon.PistolConfig);
        }

        public void Visit(Rifle rifle)
        {
            AddFireModes(_weapon.RifleConfig);
        }

        public void Visit(Shotgun shotgun)
        {
            AddFireModes(_weapon.ShotgunConfig);
        }
        
        
        public void SetFireModes(WeaponComponent weaponComponent)
        {
            _methodInfos = new List<MethodInfo>();
            for (_index = 0; _index < _allMethodUseAttribute.Length; _index++)
            {
                weaponComponent.Accept(this);
            }
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
            }

            if (config.BurstFire && _allMethodUseAttribute[_index].Name == "AddBurstFire")
            {
                _methodInfos.Add(_allMethodUseAttribute[_index]);
            }

            if (config.AutomaticFire && _allMethodUseAttribute[_index].Name == "AddAutomaticFire")
            {
                _methodInfos.Add(_allMethodUseAttribute[_index]);
            }
        }
    }
}