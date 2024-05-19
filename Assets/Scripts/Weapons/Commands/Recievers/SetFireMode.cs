using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Player.Weapons;
using Game.Player.Weapons.WeaponClass;
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
        private List<MethodInfo> _methodInfos;
        private MethodInfo[] _allMethodUseAttribute;
        private ContextMenuAttribute[] _allAttributes;
        private int _index;

        public SetFireMode(ISetFireModes changeModeFire)
        {
            _changeModeFire = changeModeFire;
        }

        public void Initialize()
        {
            _allMethodUseAttribute = _typeFireMode
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.IsDefined(typeof(ContextMenuAttribute), false))
                .ToArray();
            _allAttributes = _allMethodUseAttribute
                .OfType<ContextMenuAttribute>()
                .ToArray();

        }

        public void Visit(Pistol pistol)
        {
            if (_allAttributes[_index].Label == "single fire" || _allAttributes[_index].Label == "burst fire")
            {
                _methodInfos.Add(_allMethodUseAttribute[_index]);
            }
        }

        public void Visit(Rifle rifle)
        {
            if (_allAttributes[_index].Label == "single fire" || _allAttributes[_index].Label == "burst fire" || _allAttributes[_index].Label == "automatic fire")
            {
                _methodInfos.Add(_allMethodUseAttribute[_index]);
            }
        }

        public void Visit(Shotgun shotgun)
        {
            if (_allAttributes[_index].Label != "single fire") return;
            _methodInfos.Add(_allMethodUseAttribute[_index]);
        }

        public void VisitWeapon(WeaponComponent component)
        {
            _methodInfos = new List<MethodInfo>();
            for (_index = 0; _index < _allAttributes.Length; _index++)
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
            return _methodInfos;
        }
    }
}