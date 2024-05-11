using Cysharp.Threading.Tasks;
using Game.Player.Weapons.InterfaceWeapon;
using Game.Player.Weapons.InterfaseWeapon;
using Game.Player.Weapons.WeaponConfigs;
using UI.Storage;
using UnityEngine;

namespace Game.Player.Weapons.Decorator
{
    public class ReloadAction : IAction
    {
        private readonly ReloadComponent _reloadComponent;
        private readonly IReloadStrategy _reloadStrategy;
        private readonly BoolStorage _boolStorage;

        public ReloadAction(ReloadComponent reloadComponent, IReloadStrategy reloadStrategy, BoolStorage boolStorage)
        {
            _reloadComponent = reloadComponent;
            _reloadStrategy = reloadStrategy;
            _boolStorage = boolStorage;
        }

        public void Execute()
        {
            _reloadStrategy.Reload(_reloadComponent, _boolStorage);
        }
    }
}