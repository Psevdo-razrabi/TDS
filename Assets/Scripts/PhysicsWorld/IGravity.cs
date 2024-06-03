using Game.Player.PlayerStateMashine;
using UnityEngine;

namespace PhysicsWorld
{
    public interface IGravity
    {
        public CharacterController CharacterController { get; }
        public StateMachineData StateMachineData { get; }
    }
}