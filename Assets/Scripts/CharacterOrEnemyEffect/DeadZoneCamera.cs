using System;
using Customs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

public class DeadZoneCamera : MonoBehaviour
{
    private static readonly int ColorID = Shader.PropertyToID("_BaseColor");

    [field: SerializeField] private Camera _camera;
    [field: SerializeField] private LayerMask _layerWall;
    [field: SerializeField] [Range(0, 10)] private float _timeToHideMaterial;
    [field: SerializeField] [Range(0, 10)] private float _timeToShowMaterial;
    [field: SerializeField] private float _ratioAlpha;
    private Material[] _materials;
    private GameObject _obstacleGameObject;
    private bool _isWallRaycast;
    
    private void Update()
    {
        CalculateVectors();
    }

    private void CalculateVectors()
    {
        var direction = _camera.transform.position - transform.position;
        var ray = new Ray(transform.position, direction.normalized);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 3000, _layerWall))
        {
            OperationWithMaterial(_ratioAlpha, _timeToHideMaterial, true, 
                (mat, isTransparent) => mat.SetMaterialTransparent(isTransparent));
            _isWallRaycast = true;
            if(_obstacleGameObject == raycastHit.transform.gameObject) return;
            
            _obstacleGameObject = raycastHit.transform.gameObject;
            _materials = raycastHit.transform.gameObject.GetComponentInChildren<Renderer>().materials;
        }
        else if(_isWallRaycast)
        {
            OperationWithMaterial(1f, _timeToShowMaterial, false, null, 
                (mat, isTransparent) => mat.SetMaterialTransparent(isTransparent));
            _isWallRaycast = false;
        }
    }

    private void OperationWithMaterial(float endValue, float time, bool isTransparent, 
        Action<Material, bool> beforeAssigningRenderType = null, Action<Material, bool> afterAssigningRenderType = null)
    {
        _materials?.ForEach(async mat =>
        {
            beforeAssigningRenderType?.Invoke(mat, isTransparent);
            await InterpolateAlpha(endValue, time, mat);
            afterAssigningRenderType?.Invoke(mat, isTransparent);
        });
    }
    
    private async UniTask InterpolateAlpha(float endValue, float time, Material material)
    {
        await DOTween
            .To(() => material.GetVector(ColorID).w, x =>
                {
                    var vectorColor = material.GetColor(ColorID);
                    vectorColor.a = x;
                    material.SetColor(ColorID, vectorColor);
                },
                endValue, time);
    }
}
