using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Game.Player.Weapons;
using Weapons.InterfaceWeapon;
using Game.Player.PlayerStateMashine;
using Input;
using Zenject;

public class CurrentWeapon : IVisitWeaponType
{
    private readonly WeaponConfigs _weaponConfigs;
    
    private InputSystemMouse _inputSystemMouse;
    private BaseWeaponConfig _weaponConfig;
    private BaseWeaponConfig _aimWeaponConfig;
    private StateMachineData _stateMachineData;
    
    private bool _isAiming;

    public CurrentWeapon(WeaponConfigs weaponConfigs)
    {
        _weaponConfigs = weaponConfigs;
    }
    
    [Inject]
    public void Construct(StateMachineData stateMachineData)
    {
        _stateMachineData = stateMachineData;
    }
    
    public BaseWeaponConfig CurrentWeaponConfig => _weaponConfig;

    public void LoadConfig(WeaponComponent weaponComponent)
    {
        VisitWeapon(weaponComponent);
    }

    public void VisitWeapon(WeaponComponent component)
    {
        Visit((dynamic)component);
    }

    public void Visit(Pistol pistol)
    {
        _weaponConfig = _weaponConfigs.PistolConfig;
        _aimWeaponConfig = _weaponConfigs.PistolAimConfig;
    }

    public void Visit(Rifle rifle)
    {
        _weaponConfig = _weaponConfigs.RifleConfig;
        _aimWeaponConfig = _weaponConfigs.RifleAimConfig;
    }

    public void Visit(Shotgun shotgun)
    {
        _weaponConfig = _weaponConfigs.ShotgunConfig;
        _aimWeaponConfig = _weaponConfigs.ShotgunAimConfig;
    }
}