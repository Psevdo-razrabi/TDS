using System;
using UniRx;
using UnityEngine;

public class IKSystem : MonoBehaviour
{
    [SerializeField] private GameObject objRotation;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private CharacterController characterController;
    
    private const float DISTANCE_TO_GROUND = 0.084f;

    private float _dynamicHeight;
    private CompositeDisposable _compositeDisposable = new();
    private readonly Vector3[] _hitPoints = new Vector3[2];

    private enum Foots
    {
        LeftFoot = 0,
        RightFoot = 1
    }

    private void Start()
    {
        _dynamicHeight = characterController.height;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
        
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
        
        SetFoot(AvatarIKGoal.LeftFoot, Foots.LeftFoot);
        SetFoot(AvatarIKGoal.RightFoot, Foots.RightFoot);
        
        if (_hitPoints[(int)Foots.LeftFoot].y > _hitPoints[(int)Foots.RightFoot].y)
            AdjustCollider(Foots.LeftFoot);
        else if (_hitPoints[(int)Foots.LeftFoot].y < _hitPoints[(int)Foots.RightFoot].y)
            AdjustCollider(Foots.RightFoot);
        else characterController.height = _dynamicHeight;
    }

    private void SetFoot(AvatarIKGoal footIK, Foots footNum)
    {
        RaycastHit hit;
        var ray = new Ray(animator.GetIKPosition(footIK) + Vector3.up, Vector3.down);
        if (!Physics.Raycast(ray, out hit, DISTANCE_TO_GROUND + 2f, layerGround)) return;
        _hitPoints[(int)footNum] = hit.point;
        var footPosition = hit.point;
        footPosition.y += DISTANCE_TO_GROUND;
        animator.SetIKPosition(footIK, footPosition);
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
        animator.SetIKRotation(footIK, Quaternion.LookRotation(forward, hit.normal));
    }
    
    private void AdjustCollider(Foots feet)
    {
        var diff = Math.Abs(_hitPoints[(int)feet].y) - Math.Abs(_hitPoints[feet == 0 ? 1 : 0].y);
        if(diff is > 0.2f and < 0.9f)
            characterController.height = _dynamicHeight - diff * (2f / 3f);
    }

    public void ChangeColliderInitHeight(float newHeight)
    {
        _dynamicHeight = newHeight;
    }
}