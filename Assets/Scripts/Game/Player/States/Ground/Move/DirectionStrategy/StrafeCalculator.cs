using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;

namespace Game.Player.States.DirectionStrategy
{
    public class StrafeCalculator : IDirectionCalculator
    {
        public float CalculatedSpeed(Vector3 mouse, Vector3 movement, PlayerMoveConfig moveConfig)
        {
            return Mathf.Abs(mouse.x) > Mathf.Abs(mouse.y) ? moveConfig.SpeedStrafe : moveConfig.SpeedBackwards;
        }
    }
}