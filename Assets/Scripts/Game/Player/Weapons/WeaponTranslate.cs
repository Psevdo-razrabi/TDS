using UnityEngine;
using Zenject;

namespace Game.Player.Weapons
{
    public class WeaponTranslate : MonoBehaviour
    {
        [Inject] private PlayerMovement _playerMovement;
        private void Update() => transform.position = _playerMovement.transform.position;
    }
}