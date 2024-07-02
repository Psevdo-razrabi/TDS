using Customs;
using Game.Core.Health;
using Game.Player.Interfaces;
using UI.Storage;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class Enemy: MonoBehaviour, IHealth, IInitialaize
    {
        [SerializeField] private EnemyHealthConfig healthConfigConfig;
        [SerializeField] private RagdollHelper ragdollHelper;
        public IHealthStats HealthStats { get; private set; }
        private ValueCountStorage<float> _valueCountStorage;
        private EventController _eventController;

        public void InitModel(ValueCountStorage<float> valueCountStorage)
        {
            _valueCountStorage = valueCountStorage;
        }
        
        [Inject]
        private void Construct(EventController eventController)
        {
            _eventController = eventController;
        }
        
        private void Start()
        {
            HealthStats = new Health<Enemy>(healthConfigConfig.MaxHealth, _valueCountStorage, new Die<Enemy>(_eventController, ragdollHelper));
        }
    }
}