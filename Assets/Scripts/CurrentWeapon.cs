using Game.AsyncWorker.Interfaces;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Game.Player.Weapons;
using Weapons.InterfaceWeapon;
using Game.Player.PlayerStateMashine;
using Game.Player.Weapons.Prefabs;
using Input;
using UI.Storage;
using UniRx;
using Zenject;

public class CurrentWeapon : IVisitWeaponType, IInitializable
{
    public WeaponComponent WeaponComponent { get; private set; }
    public WeaponPrefabs WeaponPrefabs { get; private set; }
    private readonly Weapon _weapon;
    private InputSystemMouse _inputSystemMouse;
    private BaseWeaponConfig _weaponConfig;
    private BaseWeaponConfig _aimWeaponConfig;
    private StateMachineData _stateMachineData;
    private CompositeDisposable _compositeDisposable = new();
    private StorageModel _storageModel;
    private WeaponPrefabs _weaponPrefabs;
    private IAwaiter _awaiter;

    private bool _isAiming;
    
    public CurrentWeapon(Weapon weapon)
    {
        _weapon = weapon;
    }
    
    [Inject]
    public void Construct(StateMachineData stateMachineData, StorageModel storageViewModel, WeaponPrefabs weaponPrefabs, IAwaiter awaiter)
    {
        _stateMachineData = stateMachineData;
        _storageModel = storageViewModel;
        WeaponPrefabs = weaponPrefabs;
        _awaiter = awaiter;
    }
    
    public async void Initialize()
    {
        await _awaiter.AwaitLoadOrInitializeParameter(_stateMachineData);
        SubscribeAim();
    }
    
    public BaseWeaponConfig CurrentWeaponConfig => _isAiming ? _aimWeaponConfig : _weaponConfig;

    public void LoadConfig(WeaponComponent weaponComponent)
    {
        WeaponComponent = weaponComponent;
        weaponComponent.Accept(this);
    }

    public void Visit(Pistol pistol)
    {
        _weaponConfig = _weapon.PistolConfig;
        _aimWeaponConfig = _weapon.PistolAimConfig;
    }

    public void Visit(Rifle rifle)
    {
        _weaponConfig = _weapon.RifleConfig;
        _aimWeaponConfig = _weapon.RifleAimConfig;
    }

    public void Visit(Shotgun shotgun)
    {
        _weaponConfig = _weapon.ShotgunConfig;
        _aimWeaponConfig = _weapon.ShotgunAimConfig;
    }

    private void SwitchAim()
    {
        _isAiming = _stateMachineData.GetValue<ReactiveProperty<bool>>(Name.IsAiming).Value;
        _storageModel.ChangeAimWeapon(WeaponComponent);
    }

    private void SubscribeAim()
    {
        _stateMachineData.GetValue<ReactiveProperty<bool>>(Name.IsAiming)
            .Subscribe(_ =>
            {
                SwitchAim();
            })
            .AddTo(_compositeDisposable);
    }
}