using System.Collections;
using Game.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Player.Weapons
{
    public abstract class Projectile : MonoBehaviour
    {
        protected WaitForSeconds Timer;
        protected float Damage;

        protected virtual void OnEnable() => StartCoroutine(TimerToHide());

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                float damage = Random.Range(Damage / 1.5f, Damage * 1.8f);
                if (damage < 1)
                    damage = 1;
                enemy.TakeDamage(damage);
            }
        }

        private IEnumerator TimerToHide()
        {
            yield return Timer;
            gameObject.SetActive(false);
        }
    }
}