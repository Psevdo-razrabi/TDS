using Cysharp.Threading.Tasks;
using UI.Storage;

namespace Game.Player.Weapons.InterfaseWeapon
{
    public interface IReloadStrategy
    {
        void Reload(ReloadComponent reloadComponent, BoolStorage boolStorage);
    }
}