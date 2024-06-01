using System;
using System.ComponentModel;
using Game.Player;
using Game.Player.Interfaces;
using UI.Storage;
using UI.View;
using UI.ViewModel;
using UnityEngine;

namespace DI
{
    public class HealthBarBinder : MonoBehaviour
    {
        [SerializeField] private HealthBarView healthBarView;
        private HealthBarPresenter _healthBarPresenter;

        private void OnEnable()
        {
            BindHealthBar<Player>();
            BindHealthBar<Enemy.Enemy>();
        }

        private void BindHealthBar<T>()
        {
            if (!gameObject.TryGetComponent<IInitialaize>(out var element)) return;
            
            var valueCountStorage = new ValueCountStorage<float>();
            element.InitModel(valueCountStorage);
            _healthBarPresenter = new HealthBarPresenter(valueCountStorage, healthBarView);
                
            _healthBarPresenter.Initialize();
        }

        private void OnDisable()
        {
            _healthBarPresenter.Dispose();
        }
    }
}
