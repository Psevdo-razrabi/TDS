using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Invoker;

namespace UI.Storage
{
    public class StorageModel
    {
        private readonly InvokerWeaponCommand _weaponCommand;

        public StorageModel(InvokerWeaponCommand weaponCommand)
        {
            _weaponCommand = weaponCommand;
        }
        
        public void ChangeWeapon(WeaponComponent weaponComponent)
        {
            _weaponCommand.OnInvokeCommands(weaponComponent);
        }
        public void ChangeAimWeapon(WeaponComponent weaponComponent)
        {
            _weaponCommand.OnInvokeSwitchAimComand(weaponComponent);
        }
    }
}