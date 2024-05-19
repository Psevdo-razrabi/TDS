using Cysharp.Threading.Tasks;
using Game.Player.Weapons.ChangeWeapon;

namespace Game.Player.Weapons.Commands
{
    public class ChangeWeaponCommand : Command
    {
        private WeaponChange _weaponChange;

        public ChangeWeaponCommand(WeaponChange weaponChange)
        {
            _weaponChange = weaponChange;
        }

        public override async UniTask Execute(WeaponComponent weaponComponent)
        {
            _weaponChange.Change(weaponComponent);
            await UniTask.Yield();
        }
    }
}