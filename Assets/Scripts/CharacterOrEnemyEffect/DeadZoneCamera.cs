using UnityEngine;

public class DeadZoneCamera : MonoBehaviour
{
    private static int PosId = Shader.PropertyToID("_PlayerPosition");
    private static int SizeId = Shader.PropertyToID("_Size");

    [field: Header("ParametersShader")] 
    [field: SerializeField] public Material WallMaterial { get; private set; }
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public LayerMask LayerWall { get; private set; }
    
    private void Update()
    {
        CalculateVectors();
    }

    private void CalculateVectors()
    {
        var direction = Camera.transform.position - transform.position;
        var ray = new Ray(transform.position, direction.normalized);
        var view = Camera.WorldToViewportPoint(transform.position);
        SetPropertyMaterial(view, ray);
    }

    private void SetPropertyMaterial(Vector3 view, Ray ray)
    {
        WallMaterial.SetFloat(SizeId, Physics.Raycast(ray, 3000, LayerWall) ? 1 : 0);
        WallMaterial.SetVector(PosId, view);
    }
}
