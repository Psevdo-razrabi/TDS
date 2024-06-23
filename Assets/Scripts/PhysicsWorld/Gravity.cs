using PhysicsWorld;
using UnityEngine;
using Zenject;

public class Gravity : ISetGravityForce, ITickable
{
    private float _gravityForce = 9.8f;
    private IGravity _playerComponents;
        
    public float GravityForce
    {
        get => _gravityForce;
        set
        {
            if (value >= 0)
                _gravityForce = value;
        }
    }

    [Inject]
    public void Construct(IGravity playerComponents) => _playerComponents = playerComponents;

    public void Tick()
    {
        GravityHandling();
    }
        
    private void GravityHandling()
    {
        _playerComponents.StateMachineData.TargetDirectionY = _playerComponents.CharacterController.isGrounded switch
        {
            false => -_gravityForce * Time.deltaTime,
            true when _playerComponents.CharacterController.velocity.y <= 0 => -0.12f,
            _ => _playerComponents.StateMachineData.TargetDirectionY
        };
    }
}
