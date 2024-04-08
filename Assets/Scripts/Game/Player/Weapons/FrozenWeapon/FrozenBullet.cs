using Game.Enemy;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.FrozenWeapon
{
    public class FrozenBullet : Projectile
    {
        private FrozenWeapon _frozenWeapon;

        protected override void OnEnable()
        {
            Timer = new WaitForSeconds(_frozenWeapon.WeaponStats[_frozenWeapon.CurrentLevel-1].Speed);
            Damage = _frozenWeapon.WeaponStats[_frozenWeapon.CurrentLevel - 1].Damage;
            base.OnEnable();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.TakeDamage(Damage);
                enemy.gameObject.TryGetComponent(out EnemyMove enemyMove);
                enemyMove.Freeze();
            }
            if(_frozenWeapon.CurrentLevel <3)
                gameObject.SetActive(false);
        }

        private void Update() => transform.position += transform.forward * 
                                                       (_frozenWeapon.WeaponStats[_frozenWeapon.CurrentLevel - 1].Speed * Time.deltaTime);
        
        [Inject] private void Construct(FrozenWeapon frozenWeapon) => _frozenWeapon = frozenWeapon;
    }
}