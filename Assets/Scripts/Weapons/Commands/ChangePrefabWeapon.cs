using Cysharp.Threading.Tasks;
using Game.Player.Weapons.Commands.Recievers;

namespace Game.Player.Weapons.Commands
{
    public class ChangePrefabWeapon : Command
    {
        private WeaponChanger _weaponChanger;

        public ChangePrefabWeapon(WeaponChanger weaponChanger)
        {
            _weaponChanger = weaponChanger;
        }

        public override async UniTask Execute(WeaponComponent weaponComponent)
        {
            _weaponChanger.WeaponChange(weaponComponent);
            await UniTask.Yield();
        }
    }
}