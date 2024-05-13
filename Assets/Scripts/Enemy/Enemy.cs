using Customs;
using Game.Core.Health;
using UI.Storage;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class Enemy: MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyHealthConfig healthConfigConfig;
        [SerializeField] private RagdollHelper ragdollHelper;
        public IHealthStats HealthStats { get; private set; }
        private ValueCountStorage<float> _valueCountStorage;
        private EventController _eventController;
        
        [Inject]
        private void Construct(ValueCountStorage<float> valueCountStorage, EventController eventController)
        {
            _valueCountStorage = valueCountStorage;
            _eventController = eventController;
        }
        
        private void Start()
        {
            HealthStats = new Health<Enemy>(healthConfigConfig.MaxHealth, _valueCountStorage, new Die<Enemy>(gameObject, _eventController, ragdollHelper));
        }
    }
}