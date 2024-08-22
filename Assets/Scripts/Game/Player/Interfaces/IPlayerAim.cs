using UnityEngine;

namespace Game.Player.Interfaces
{
    public interface IPlayerAim
    {
        void Aim();
        void FreezeAim(Quaternion rotation, Player player);
    }
}