using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;

namespace Game.Player.States.DirectionStrategy
{
    public interface IDirectionCalculator
    {
        float CalculatedSpeed(Vector3 mouse, Vector3 movement, PlayerMoveConfig moveConfig);
    }
}