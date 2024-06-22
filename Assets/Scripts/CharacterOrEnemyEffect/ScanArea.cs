using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CharacterOrEnemyEffect
{
    public class ScanArea : MonoBehaviour
    {
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] [Range(0, 100)] private float radiusSphere;
        [SerializeField] [Range(0, 10)] private float timeToScanArea;
        [SerializeField] [Range(0, 10)] private float rechargeTime;
        [SerializeField] [Range(0, 10)] private float defaultRadius;

        private void Start()
        {
            sphereCollider.radius = defaultRadius;
            InvokeRepeating(nameof(ScanAreaWithSphere), 0f, timeToScanArea + rechargeTime + 0.1f);
        }

        private async void ScanAreaWithSphere()
        {
            await Scan();
            sphereCollider.radius = defaultRadius;
            await UniTask.Delay(TimeSpan.FromSeconds(rechargeTime));
        }
        
        private async UniTask Scan()
        {
            await DOTween
                .To(() => 0f, x => sphereCollider.radius = x,
                    radiusSphere, timeToScanArea);
        }
    }
}