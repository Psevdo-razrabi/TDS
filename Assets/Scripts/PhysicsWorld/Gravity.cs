using Game.Player;
using Game.Player.Interfaces;
using PhysicsWorld;
using UnityEngine;
using Zenject;

public class Gravity : ISetGravityForce, ITickable
{
    private float _gravityForce = 6f;
    private readonly ICharacterController _characterController;
    private IStateData _stateData;

    public Gravity(IStateData stateData, ICharacterController characterController)
    {
        _stateData = stateData;
        _characterController = characterController;
    }

    public float GravityForce
    {
        get => _gravityForce;
        set
        {
            if (value >= 0)
                _gravityForce = value;
        }
    }

    public void Tick()
    {
        GravityHandling();
    }
        
    private void GravityHandling()
    {
        _stateData.Data.TargetDirectionY = _characterController.CharacterController.isGrounded switch
        {
            false => -_gravityForce * Time.deltaTime,
            true when _characterController.CharacterController.velocity.y <= 0 => -0.02f,
            _ => _stateData.Data.TargetDirectionY
        };
    }
}
