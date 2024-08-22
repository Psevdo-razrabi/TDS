using Customs;
using Game.Core.Health;
using Game.Player;
using Game.Player.AnyScripts;
using Game.Player.Interfaces;
using UI.Storage;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class Enemy: MonoBehaviour, IHealth, IInitialaize, IHit, IInitializable<Enemy>
    {
        [SerializeField] private EnemyHealthConfig healthConfigConfig;
        [SerializeField] private RagdollHelper ragdollHelper;
        public IHealthStats HealthStats { get; private set; }
        public Subject<Unit> Hit { get; private set; } = new();
        private ValueCountStorage<float> _valueCountStorage;
        private DiContainer _diContainer;

        public void InitModel(ValueCountStorage<float> valueCountStorage)
        {
            _valueCountStorage = valueCountStorage;
        }

        [Inject]
        private void Construct(DiContainer diContainer) => _diContainer = diContainer;

        public void Initialize()
        {
            var die = new Die(ragdollHelper);
            HealthStats = new Health<Enemy>(healthConfigConfig.MaxHealth, _valueCountStorage, die);
            var operationWithHealth = new OperationWithHealth<PlayerComponents>(die, this,
                HealthResoringConstyl._playerRestoring);
            
            operationWithHealth.SubscribeDead(operationWithHealth.EnemyDie);
            operationWithHealth.SubscribeHit(operationWithHealth.EnemyHitBullet);
        }
    }

    public interface IHit
    {
        Subject<Unit> Hit { get; }
    }
}