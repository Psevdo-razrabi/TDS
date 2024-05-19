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

        public ReloadAction(ReloadComponent reloadComponent, IReloadStrategy reloadStrategy)
        {
            _reloadComponent = reloadComponent;
            _reloadStrategy = reloadStrategy;
        }

        public void Execute()
        {
            _reloadStrategy.Reload(_reloadComponent, _reloadComponent.BoolStorage);
        }
    }
}