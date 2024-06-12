using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DetectingEnemies : MonoBehaviour
{
    [SerializeField] private Material enemyDetecting;
    private static readonly int DitherID = Shader.PropertyToID("_DitherParameters");

    private async UniTask InterpolateAlpha(float endValue)
    {
        await DOTween
            .To(() => enemyDetecting.GetFloat(DitherID), x => enemyDetecting.SetFloat(DitherID, x),
                endValue, 1f);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckComponent(0f, other);
    }
    
    private void OnTriggerStay(Collider other)
    {
        CheckComponent(1f, other);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        CheckComponent(1f, other);
    }

    private async void CheckComponent(float endValue, Collider collider)
    {
        if (collider.TryGetComponent(out Enemy.Enemy enemy))
        {
            await InterpolateAlpha(endValue);
        }
    }
}
