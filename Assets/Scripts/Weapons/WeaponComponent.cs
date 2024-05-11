using UnityEngine;

namespace Game.Player.Weapons
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        public ReloadComponent reloadComponent { get; protected set; }
        public FireComponent fireComponent { get; protected set; }
    }
}