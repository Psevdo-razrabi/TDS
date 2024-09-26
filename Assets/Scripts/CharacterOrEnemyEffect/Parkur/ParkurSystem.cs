using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class ParkurSystem : MonoBehaviour
{
    [SerializeField] private Vector3 _ObjPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float _ObjectHeight;
    [SerializeField] private Vector3 ToAnimation;
    [SerializeField] private LayerMask _layerMask, layerMaskVault;
    [SerializeField] private bool _isActionState = true;
    [SerializeField] private GameObject step;
    [SerializeField] private float HeightCorrectionForStep;
    [SerializeField] private float HeightCorrectionForClamb;
    [SerializeField] private float HeightCorrectionForClambOnWall;
    [SerializeField] private Player Player;
    [SerializeField] private bool _isPlayerOnObstacle;
    [SerializeField] private bool _lockAtObstacle;
    public GameObject ccc;
    private Quaternion _requiredRotation;
    private float ledgeRayHeight = 0.4f;
    public Vector3 Movement { get; set; }
    private CompositeDisposable _disposable = new();
    private Vector3 _endPos;
    private float _colliderRadius, diff;

    [Header("Настройки для луча")] 
    
    [SerializeField] private float rayCastDistance;
    [SerializeField] public float distanceThreshold = 0.1f;
    [SerializeField] public float angleThreshold = -0.5f;
    [SerializeField] private float currentAngle;

    [Header("Настройки для угла")] 
    [SerializeField] private float startMaxAngle;

    private void Start()
    {
        _colliderRadius = characterController.radius;
        Player.InputSystem.Move
            .Subscribe(vector => Movement = new Vector3(vector.x, 0f, vector.y).normalized)
            .AddTo(_disposable);
    }

    private void OnDestroy()
    {
        _disposable.Dispose();
        _disposable.Clear();
    }

    public void AnimationEnd()
    {
        _isPlayerOnObstacle = true;
    }
    
    private async void Update()
    {
        if (animator.IsInTransition(0))
            return;
        Step();
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Vault"))
            CheckVaultObj();

        animator.SetBool("onSurface", characterController.isGrounded);
        
        if (characterController.isGrounded)
        {
            //if(_isPlayerOnObstacle) Debug.LogWarning("я на уступе");
        }
        
        var checkLanding = CheckLanding();

        if (_isPlayerOnObstacle & _lockAtObstacle == false & checkLanding.isLanding)
        {
            _isPlayerOnObstacle = false;
            //Debug.LogWarning("персонаж упал");
        
            // if (_ObjectHeight is >= 0.8f and <= 1.5f)
            // {
            //     _animator.CrossFade("Climb", 0.2f);
            //     await UniTask.Yield();
            // }
            //
            if (_ObjectHeight is >= 1.5f and <= 3f)
            {
                animator.CrossFade("Jumping", 0.2f);
                await UniTask.Yield();
            }
        }
        
        if (UnityEngine.Input.GetButtonDown("Jump") && _lockAtObstacle)
        {
            await ClimbAnimation();
        }

        
        //если у нас щас активен стейт то return, крч перенос в стейт машину проблему решит

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Climb"))
        {
            animator
                .MatchTarget(_ObjPosition, transform.rotation, AvatarTarget.RightFoot,
                    new MatchTargetWeightMask(new Vector3(0f, 1f, 1f), 0), 0.14f, 0.33f);
        }
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Step"))
        {
            animator
                .MatchTarget(_ObjPosition, transform.rotation, AvatarTarget.RightFoot,
                    new MatchTargetWeightMask(new Vector3(0f, 1f, 0f), 0), 0.42f, 0.50f);
        }
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbToWall"))
        {
            animator
                .MatchTarget(_ObjPosition, transform.rotation, AvatarTarget.RightHand,
                    new MatchTargetWeightMask(new Vector3(0f, 1f, 1f), 0), 0.05f, 0.33f);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Vault"))
        {
            animator.MatchTarget(_endPos, transform.rotation, AvatarTarget.LeftHand, 
                new MatchTargetWeightMask(new Vector3(0, 1, 1), 0), 0.12f, 0.31f);
        }
    }

    private void Step()
    {

        if (Physics.Raycast(step.transform.position, step.transform.forward, out RaycastHit hit, rayCastDistance,
                _layerMask))
        {
            _ObjectHeight = hit.collider.bounds.size.y;
            Vector3 raisedEndPoint = Vector3.zero;

            if (_ObjectHeight is >= 0.4f and <= 0.8f)
            {
                raisedEndPoint = hit.point + Vector3.up * (hit.collider.bounds.size.y - HeightCorrectionForStep);
            }
        
            if (_ObjectHeight is >= 0.8f and <= 1.5f)
            {
                raisedEndPoint = hit.point + Vector3.up * (hit.collider.bounds.size.y - HeightCorrectionForClamb);
            }
        
            if (_ObjectHeight is >= 1.5f and <= 5f)
            {
                raisedEndPoint = hit.point + Vector3.up * (hit.collider.bounds.size.y - HeightCorrectionForClambOnWall);
            }
            
            _ObjPosition = raisedEndPoint;
            _lockAtObstacle = true;
            _requiredRotation = Quaternion.LookRotation(-hit.normal);
            //DrawLine(hit.point, raisedEndPoint, -hit.normal);
        }
        else
        {
            _lockAtObstacle = false;
        }
    }
    
    private async void CheckVaultObj()
    {
        if (!Physics.Raycast(step.transform.position, step.transform.forward, out RaycastHit hit, 1, layerMaskVault))
        {
            _lockAtObstacle = _lockAtObstacle == true;
            diff = 0;
            return;
        }
        if (!Physics.Raycast(hit.point - hit.normal, hit.normal, out RaycastHit hitSecSide, 2, layerMaskVault))
        {
            _lockAtObstacle = _lockAtObstacle == true;
            diff = 0;
            return;
        }
        
        _lockAtObstacle = true;
        float dist = Vector3.Distance(hit.point, hitSecSide.point);
        _endPos = hit.point - hit.normal * (dist + _colliderRadius);
        ccc.transform.position = _endPos;
        diff = dist;


    }

    /*private bool CheckLedge(Vector3 movementDirection)
    {
        var ledgeOriginOffset = 0f;

        var ledgeOrigin = _step.transform.position;
        
        if (!Physics.Raycast(ledgeOrigin, Vector3.down, out RaycastHit hit, rayCastDistance, _layerMask)) return false;
        var ledgeHeight = hit.collider.bounds.size.y;

        if (ledgeHeight > ledgeRayHeight) return true;
        return false;
    }*/

    private (bool isLanding, RaycastHit hitObject) CheckLanding()
    {
        if (_isActionState) return (false, default);

        if (Physics.Raycast(step.transform.position, Vector3.down, out RaycastHit hitInfo, rayCastDistance, _layerMask)) 
            return (false, default);
        //Debug.LogWarning("падаю");
        return (true, hitInfo);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(step.transform.position + Vector3.down * 0.2f, 0.5f);
        Gizmos.DrawLine(step.transform.position, step.transform.position + Vector3.down * 0.5f);
    }
    

    private void DrawLine(Vector3 endPoint, Vector3 raisedEndPoint, Vector3 normal)
    {
        Debug.DrawLine(step.transform.position, endPoint, Color.red);
        Debug.DrawLine(endPoint, raisedEndPoint, Color.cyan);
    }

    private async UniTask ClimbAnimation()
    {
        if (_ObjectHeight is >= 0.4f and <= 0.8f)
        {
            animator.CrossFade("Step", 0.2f);
            await UniTask.Yield();
        }
        
        if (_ObjectHeight is >= 0.8f and <= 1.5f)
        {
            animator.CrossFade("Climb", 0.2f);
            await UniTask.Yield();
        }
        
        if (_ObjectHeight is >= 1.5f and <= 3f)
        {
            animator.CrossFade("ClimbToWall", 0.2f);
            await UniTask.Yield();
        }
        if (diff is > 0 and < 0.3f)
        {
            animator.CrossFade("Vault", 0.2f);
            await UniTask.Yield();
        }
        

        _isActionState = true;
        animator.applyRootMotion = true;
        characterController.enabled = false;

        await DOTween.To(() => 0f, x =>
                {
                    if (_lockAtObstacle)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, _requiredRotation, 100f * x);
                    }
                },
                animator.GetNextAnimatorStateInfo(0).length, animator.GetNextAnimatorStateInfo(0).length);

        _isActionState = false;
        animator.applyRootMotion = false;
        characterController.enabled = true;
    }
}
