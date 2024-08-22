using UnityEngine;
using Zenject;

namespace Game.Player.AnyScripts
{
    public class HealthMediator : MonoBehaviour
    {
        private IInitializable<Player> _player;
        [SerializeField] private Enemy.Enemy _enemy;

        [Inject]
        private void Construct(IInitializable<Player> player) => _player = player;

        private void Start()
        {
            _player.Initialize();
            _enemy.Initialize();
        }
    }
}