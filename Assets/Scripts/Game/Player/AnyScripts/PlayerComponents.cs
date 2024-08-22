using Customs;
using Game.Core.Health;
using Game.Player.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Player.AnyScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerComponents : MonoBehaviour, IHealth, ICharacterController
    {
        [field: SerializeField] public GameObject PlayerModelRotate { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public RagdollHelper RagdollHelper { get; private set; }
        public IPlayerAim PlayerAim { get; private set; }
        public IHealthStats HealthStats { get; private set; }
        public FOWRadiusChanger RadiusChanger { get; private set; }

        public void InitHealth(IHealthStats stats)
        {
            HealthStats = stats;
            HealthStats.Subscribe();
        }

        [Inject]
        private void Construct(IPlayerAim playerAim, FOWRadiusChanger radiusChanger)
        {
            PlayerAim = playerAim;
            RadiusChanger = radiusChanger;
        }
        
        private void OnDisable()
        {
            HealthStats.Unsubscribe();
        }
    }
}