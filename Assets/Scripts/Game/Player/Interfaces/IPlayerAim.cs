using UnityEngine;

namespace Game.Player.Interfaces
{
    public interface IPlayerAim
    {
        void Aim();
        (bool, Vector3) GetMousePosition();
    }
}