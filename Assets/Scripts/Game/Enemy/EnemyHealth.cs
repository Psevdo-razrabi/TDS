using System.Collections;
using Game.Core.Health;
using Game.Player.Weapons;
using UnityEngine;
using Zenject;

namespace Game.Enemy
{
    public class EnemyHealth : ObjectHealth
    {
        private WaitForSeconds _tick = new WaitForSeconds(1f);
        [Inject] private RadiationWeapon _radiationWeapon;
        
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            if (CurrentHealth <= 0)
            {
                gameObject.SetActive(false);
                _radiationWeapon.RemoveEnemy(this);
            }
        }

        public void Burn(float damage) => StartCoroutine(GetBurn(damage));
        
        private IEnumerator GetBurn(float damage)
        {
            if(gameObject.activeSelf == false)
                yield break;
            float tickDamage = damage / 3f;
            if (tickDamage < 1f) 
                tickDamage = 1f;
            for (int i = 0; i < 3; i++)
            {
               TakeDamage(tickDamage);
               yield return _tick;
            }
        }
    }
}