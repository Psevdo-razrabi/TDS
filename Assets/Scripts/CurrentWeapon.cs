using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Game.Player.Weapons;
using Weapons.InterfaceWeapon;
using Game.Player.PlayerStateMashine;
using Input;
using UI.Storage;
using UniRx;
using Zenject;

public class CurrentWeapon : IVisitWeaponType
{
    private readonly WeaponConfigs _weaponConfigs;

    private WeaponComponent _weaponComponent;
    private InputSystemMouse _inputSystemMouse;
    private BaseWeaponConfig _weaponConfig;
    private BaseWeaponConfig _aimWeaponConfig;
    private StateMachineData _stateMachineData;
    private CompositeDisposable _compositeDisposable = new();
    private StorageModel _storageModel;

    private bool _isAiming;
    
    public CurrentWeapon(WeaponConfigs weaponConfigs)
    {
        _weaponConfigs = weaponConfigs;
    }
    
    [Inject]
    public void Construct(StateMachineData stateMachineData, StorageModel storageViewModel)
    {
        _stateMachineData = stateMachineData;
        _storageModel = storageViewModel;
        
        _isAiming = _stateMachineData.IsAiming.Value;
        SubscribeAim();
    }
    
    public BaseWeaponConfig CurrentWeaponConfig => _isAiming ? _aimWeaponConfig : _weaponConfig;

    public void LoadConfig(WeaponComponent weaponComponent)
    {
        _weaponComponent = weaponComponent;
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

    private void SwitchAim()
    {
        _isAiming = _stateMachineData.IsAiming.Value;
        _storageModel.ChangeAimWeapon(_weaponComponent);
    }

    private void SubscribeAim()
    {
        _stateMachineData.IsAiming
            .Subscribe(_ =>
            {
                SwitchAim();
            })
            .AddTo(_compositeDisposable);
    }
}