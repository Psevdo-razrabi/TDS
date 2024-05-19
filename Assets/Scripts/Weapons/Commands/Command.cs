using Cysharp.Threading.Tasks;

namespace Game.Player.Weapons.Commands
{
    public abstract class Command
    {
        public abstract UniTask Execute(WeaponComponent weaponComponent);
    }
}