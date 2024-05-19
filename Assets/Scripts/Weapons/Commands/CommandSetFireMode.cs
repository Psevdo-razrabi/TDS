using System.Collections.Generic;
using Customs;
using Customs.Data;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.WeaponClass;
using Weapons.InterfaceWeapon;

namespace Game.Player.Weapons.Commands
{
    public class CommandSetFireMode : Command
    {
        private SetFireMode _fireMode;
        
        public override async UniTask Execute(WeaponComponent weaponComponent)
        {
            _fireMode.SetFireModes(weaponComponent);
            await UniTask.Yield();
        }
    }
}