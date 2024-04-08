using System;
using System.Collections;
using Game.Core.Health;
using UnityEngine;

namespace Game.Player
{
    public class PlayerHealth : ObjectHealth
    {
        public Action OnHealthChanged;
        private WaitForSeconds _regenerationInterval = new WaitForSeconds(5f);
        private float _regenerationValue = 1f;

        public void Heal(float heal)
        {
            TakeHeal(30f);
            OnHealthChanged?.Invoke();
        }

        private void Start() => StartCoroutine(Regeneration());

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            OnHealthChanged?.Invoke();
            if(CurrentHealth<=0)
                Debug.Log("Player is dead");
        }

        private IEnumerator Regeneration()
        {
            while (true)
            {
                TakeHeal(_regenerationValue);
                OnHealthChanged?.Invoke();
                yield return _regenerationInterval;
            }
        }
    }
}