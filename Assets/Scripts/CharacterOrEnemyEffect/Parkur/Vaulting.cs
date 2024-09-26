using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

using UniRx;

using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class Vaulting : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask layerMaskVault;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private float _colliderRadius;
    private Vector3 _endPos;
    private Quaternion _requiredRotation;
    void Start()
    {
        _colliderRadius = controller.radius;
    }

    void Update()
    {
        if (UnityEngine.Input.GetButtonDown("Jump"))
        {
            CheckVaultObj();
        }
    }

    private async void CheckVaultObj()
    {
        if (!Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hit, 1,
                layerMaskVault)) return;
        
        Debug.DrawLine(player.transform.position, hit.point, Color.cyan);
        if (!Physics.Raycast(hit.point - hit.normal, hit.normal, out RaycastHit hitSecSide, 2, layerMaskVault)) ;
        Debug.DrawLine(hit.point - hit.normal, hitSecSide.point, Color.green);
        
        float dist = Vector3.Distance(hit.point, hitSecSide.point);
        if (dist < 0.3f)
        {
            _endPos = hit.point - hit.normal * (dist + _colliderRadius);
            animator.CrossFade("Vault", 0);
            animator.applyRootMotion = true;
            animator.MatchTarget(_endPos, transform.rotation, AvatarTarget.LeftHand, 
                new MatchTargetWeightMask(new Vector3(0, 1, 0) + transform.forward, 0), 0.12f, 0.31f);


        }
        
    }
    
}
