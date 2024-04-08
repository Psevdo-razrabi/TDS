using Game.Player;
using Game.Player.Weapons;
using UnityEngine;
using Zenject;

namespace Game.Enemy
{
    public class EnemyCollision : MonoBehaviour
    {
        [SerializeField] private float _damage;
        [Inject] private RadiationWeapon _radiationWeapon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth player) == false) return;
            gameObject.TryGetComponent(out EnemyHealth enemy);
            player.TakeDamage(_damage);
            player.OnHealthChanged?.Invoke();
            gameObject.SetActive(false);
            _radiationWeapon.RemoveEnemy(enemy);
        }
    }
}