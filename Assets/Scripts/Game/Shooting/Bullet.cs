using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
   private const float LifeTime = 2f;
   
   private Rigidbody _rigidbody;

   private float _damage;
   private float _speed;
   private Transform _bulletPoint;

   public void Init(BulletConfig bulletConfig, GunConfig gunConfig)
   {
      _damage = gunConfig.Damage;
      _speed = bulletConfig.BulletSpeed;
      _bulletPoint = gunConfig.BulletPoint;
      BulletLaunch();
   }

   private void BulletLaunch()
   {
      Vector3 velocity = _bulletPoint.forward * _speed;
      _rigidbody.velocity = velocity;
   }
}
