using Cysharp.Threading.Tasks;
using Weapons.Commands.Recievers;

namespace Game.Player.Weapons.Commands
{
    public class AudioWeaponCommand : Command
    {
        private readonly InitializeWeaponAudio _weaponAudio;

        public AudioWeaponCommand(InitializeWeaponAudio weaponAudio)
        {
            _weaponAudio = weaponAudio;
        }
        
        public override async UniTask Execute(WeaponComponent weaponComponent)
        {
            _weaponAudio.ChangeAudioWeapon(weaponComponent);
            await UniTask.Yield();
        }
    }
}