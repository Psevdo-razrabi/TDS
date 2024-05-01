using System;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.Weapons.WeaponConfigs;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using OperationCanceledException = System.OperationCanceledException;
using Random = UnityEngine.Random;

public class ShootController : MonoBehaviour, IDisposable
{
    private CancellationTokenSource _cancellationToken = new();

    [SerializeField] private TestGun _gun; 
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Camera _camera;
    [SerializeField] private Crosshair _crosshair;
    [SerializeField] private ChangeCrosshair _changeCrosshair;
    
    [Inject] private CameraShakeConfig _cameraShakeConfig;
    [Inject] private BulletConfig _bulletConfig;
    [Inject] private RifleConfig _gunConfig;
    [Inject] private PoolObject<Bullet> _pool;
    private float _currentSpread;
    private float _stepSpread;
    private bool _isReducingSpread = false;
    private IDisposable _reductionSubscription;
    private void Start()
    {
        Debug.Log(_gunConfig.RecoilForce);
        _stepSpread = _gunConfig.MaxSpread / _gunConfig.MaxSpreadBullet;
        _currentSpread = _stepSpread;
        _pool.AddElementsInPool("bullet", _bulletConfig.BulletPrefab, _gunConfig.TotalAmmo);
        _gun.ShotFired += HandleShoot;
    }

    private void HandleShoot()
    {
        BulletSpawn();
        ShakeCamera();
        RecoilCursor();
        
        Debug.Log("текущий разброс : " + _currentSpread);
        
        StartSpreadReduction();
    }

    private void StartSpreadReduction()
    {
        if (_reductionSubscription != null)
        {
            _reductionSubscription.Dispose();
        }
        
        _reductionSubscription = Observable.Interval(TimeSpan.FromSeconds(_gunConfig.TimeToSpreadReduce))
            .Subscribe(_ =>
            {
                SpreadReduce();
                if (_currentSpread <= 0)
                {
                    _reductionSubscription.Dispose();
                }
            }).AddTo(this);
    }

    private async void BulletSpawn()
    {
        Bullet bullet = _pool.GetElementInPool("bullet");
        bullet.Initialize(_gunConfig.TotalAmmo);
        bullet.transform.position = _gunConfig.BulletPoint.transform.position;
        await BulletLaunch(bullet);
    }

    private async UniTask BulletLaunch(Bullet bullet)
    {
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        Vector3 velocity = _gunConfig.BulletPoint.transform.forward * _bulletConfig.BulletSpeed;
        bulletRigidbody.velocity = CalculatingSpread(velocity);
        await ReturnBullet(bullet);
    }

    private Vector3 CalculatingSpread(Vector3 velocity)
    {
        float spreadX = Random.Range(-_currentSpread, _currentSpread);
        Vector3 velocityWithSpread = velocity + new Vector3(spreadX, 0, 0);
        _currentSpread += _stepSpread;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        return velocityWithSpread;
    }

    private void ShakeCamera()
    {
        _camera.transform.DOShakePosition(_cameraShakeConfig.ShakeDuration, _cameraShakeConfig.ShakeStrength, 1, 90f,
                false, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InOutBounce).SetLink(_camera.gameObject);
    }

    private void RecoilCursor()
    {
        Vector3 forward = _gunConfig.BulletPoint.transform.forward;
        
        forward.Normalize();
        
        Vector3 perpendicular = Vector3.Cross(forward, Vector3.up);
        float sideRecoilStrength = Random.Range(-1f, 1f);
        Vector3 sideRecoil = perpendicular * sideRecoilStrength;
        
        Vector2 recoil = new Vector2(forward.x + sideRecoil.x, forward.z + sideRecoil.z) * _gunConfig.RecoilForce;

        _crosshair.RecoilPlus(recoil);
        _changeCrosshair.AdditionalCoeficent = 50f;
    }

    private void SpreadReduce()
    {
        Debug.Log("Уменяшаем разброс " + _currentSpread);
        _currentSpread -= _stepSpread;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        Debug.Log("Теперь разборос = " + _currentSpread);
    }

    private async UniTask ReturnBullet(Bullet bullet)
    {
        await UniTask.Delay(2000);
        bullet.gameObject.SetActive(false);
    }
    
    public void Dispose()
    {
        _gun.ShotFired -= HandleShoot;
    }
}
