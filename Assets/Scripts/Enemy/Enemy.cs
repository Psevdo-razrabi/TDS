using Game.Core.Health;
using UI.Storage;
using UnityEngine;

namespace Enemy
{
    public class Enemy: MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyHealthConfig healthConfigConfig;
        public IHealthStats HealthStats { get; private set; }

        private void Start()
        {
            HealthStats = new Health<Enemy>(healthConfigConfig.MaxHealth, new ValueCountStorage<float>(), new Die<Enemy>(gameObject));
        }
    }
}