using UniRx;
using UnityEngine;

public class TestTransform
{
    private GameObject _bullet;
    private Vector3 _velocity;
    private CompositeDisposable _compositeDisposable = new();
    
    public TestTransform(GameObject bullet, Vector3 velocity)
    {
        _bullet = bullet;
        _velocity = velocity;
        SubscribeBulletLaunch();
    }

    private void SubscribeBulletLaunch()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => LaunchBullet())
            .AddTo(_compositeDisposable);
    }

    private void LaunchBullet()
    {
        _bullet.transform.position += _velocity * Time.deltaTime;
    }
    
    
}
