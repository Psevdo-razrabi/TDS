using Customs;
using Cysharp.Threading.Tasks;

namespace Game.Player.Weapons.Commands
{
    public class CommandSetFireMode : Command
    {
        private SetFireMode _fireMode;

        public CommandSetFireMode(SetFireMode fireMode)
        {
            _fireMode = fireMode;
        }

        public override async UniTask Execute(WeaponComponent weaponComponent)
        {
            _fireMode.SetFireModes(weaponComponent);
            await UniTask.Yield();
        }
    }
}