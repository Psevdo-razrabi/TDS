using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOAP
{
    public class IdleStrategy : IActionStrategy
    {
        private readonly float _duration;
        private readonly Transform _enemy;
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        public IdleStrategy(float duration, Transform enemy)
        {
            _duration = duration + 3f;
            _enemy = enemy;
        }

        public async void Start()
        {
            Complete = false;
            for (int i = 0; i < 3; i++)
            {
                await _enemy.DORotateQuaternion(Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), _duration / 3);
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
            }

            Complete = true;
        }
        
    }
}