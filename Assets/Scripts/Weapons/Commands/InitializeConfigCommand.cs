using Cysharp.Threading.Tasks;
using Game.Player.Weapons.Commands.Recievers;

namespace Game.Player.Weapons.Commands
{
    public class InitializeConfigCommand : Command
    {
        private readonly DistributionConfigs _distributionConfigs;

        public InitializeConfigCommand(DistributionConfigs distributionConfigs)
        {
            _distributionConfigs = distributionConfigs;
        }
        
        public override async UniTask Execute(WeaponComponent weaponComponent)
        {
            await _distributionConfigs.Distribution(weaponComponent); //откуда то будет приходить тип класса
        }
    }
}