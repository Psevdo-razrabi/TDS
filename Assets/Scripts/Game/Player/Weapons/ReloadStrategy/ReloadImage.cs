using Game.Player.Weapons.InterfaseWeapon;
using UnityEngine;

namespace Game.Player.Weapons.ReloadStrategy
{
    public class ReloadImage : IReloadStrategy
    {
        public void Reload()
        {
            //реализация перезарядки с помощю картинки
            Debug.LogWarning("Relooooooooooooooooooooad");
        }
    }
}