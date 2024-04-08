using System;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PlayerUIUpdater : MonoBehaviour
    {
        [SerializeField] private Image _hpBar;
        [SerializeField] private Image _energyBar;
        private PlayerHealth _playerHealth;

        private void Start() => UpdateHealthBar();

        private void OnEnable() => _playerHealth.OnHealthChanged += UpdateHealthBar;

        private void OnDisable() => _playerHealth.OnHealthChanged -= UpdateHealthBar;

        private void UpdateHealthBar() => _hpBar.fillAmount = Mathf.Clamp01(_playerHealth.CurrentHealth / _playerHealth.MaxHealth);

        [Inject] private void Construct(PlayerHealth health) => _playerHealth = health;
    }
}