
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player;
using UniRx;
using UnityEngine;

public class ParkurSystem : MonoBehaviour
{
    [SerializeField] private Vector3 _ObjPosition;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _ObjectHeight;
    [SerializeField] private Vector3 ToAnimation;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _isActionState = true;
    [SerializeField] private GameObject _step;
    [SerializeField] private float HeightCorrectionForStep;
    [SerializeField] private float HeightCorrectionForClamb;
    [SerializeField] private float HeightCorrectionForClambOnWall;
    [SerializeField] private Player Player;
    [SerializeField] private bool _isPlayerOnObstacle;
    [SerializeField] private bool _lockAtObstacle;
    private Quaternion _requiredRotation;
    private float ledgeRayHeight = 0.4f;
    public Vector3 Movement { get; set; }
    private CompositeDisposable _disposable = new();

    [Header("Настройки для луча")] 
    
    [SerializeField] private float rayCastDistance;
    [SerializeField] public float distanceThreshold = 0.1f;
    [SerializeField] public float angleThreshold = -0.5f;
    [SerializeField] private float currentAngle;

    [Header("Настройки для угла")] 
    [SerializeField] private float startMaxAngle;

    private void Start()
    {
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
        if (_animator.IsInTransition(0))
            return;
        Step();

        _animator.SetBool("onSurface", _characterController.isGrounded);
        
        var checkLanding = CheckLanding();

        if (_isPlayerOnObstacle & _lockAtObstacle == false & checkLanding.isLanding)
        {
            _isPlayerOnObstacle = false;
        
            // if (_ObjectHeight is >= 0.8f and <= 1.5f)
            // {
            //     _animator.CrossFade("Climb", 0.2f);
            //     await UniTask.Yield();
            // }
            //
            if (_ObjectHeight is >= 1.5f and <= 3f)
            {
                _animator.CrossFade("Jumping", 0.2f);
                await UniTask.Yield();
            }
        }
        
        if (UnityEngine.Input.GetButtonDown("Jump") && _lockAtObstacle)
        {
            await ClimbAnimation();
        }

        //если у нас щас активен стейт то return, крч перенос в стейт машину проблему решит

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Climb"))
        {
            _animator
                .MatchTarget(_ObjPosition, transform.rotation, AvatarTarget.RightFoot,
                    new MatchTargetWeightMask(new Vector3(0f, 1f, 1f), 0), 0.14f, 0.33f);
        }
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Step"))
        {
            _animator
                .MatchTarget(_ObjPosition, transform.rotation, AvatarTarget.RightFoot,
                    new MatchTargetWeightMask(new Vector3(0f, 1f, 0f), 0), 0.42f, 0.50f);
        }
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbToWall"))
        {
            _animator
                .MatchTarget(_ObjPosition, transform.rotation, AvatarTarget.RightHand,
                    new MatchTargetWeightMask(new Vector3(0f, 1f, 1f), 0), 0.05f, 0.33f);
        }
    }

    private void Step()
    {
        if (Physics.Raycast(_step.transform.position, _step.transform.forward, out RaycastHit hit, rayCastDistance,
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
            DrawLine(hit.point, raisedEndPoint, -hit.normal);
        }
        else
        {
            _lockAtObstacle = false;
        }
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

        if (Physics.Raycast(_step.transform.position, Vector3.down, out RaycastHit hitInfo, rayCastDistance, _layerMask)) 
            return (false, default);
        return (true, hitInfo);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_step.transform.position + Vector3.down * 0.2f, 0.5f);
        Gizmos.DrawLine(_step.transform.position, _step.transform.position + Vector3.down * 0.5f);
    }
    

    private void DrawLine(Vector3 endPoint, Vector3 raisedEndPoint, Vector3 normal)
    {
        Debug.DrawLine(_step.transform.position, endPoint, Color.red);
        Debug.DrawLine(endPoint, raisedEndPoint, Color.cyan);
    }

    private async UniTask ClimbAnimation()
    {
        if (_ObjectHeight is >= 0.4f and <= 0.8f)
        {
            _animator.CrossFade("Step", 0.2f);
            await UniTask.Yield();
        }
        
        if (_ObjectHeight is >= 0.8f and <= 1.5f)
        {
            _animator.CrossFade("Climb", 0.2f);
            await UniTask.Yield();
        }
        
        if (_ObjectHeight is >= 1.5f and <= 5f)
        {
            _animator.CrossFade("ClimbToWall", 0.2f);
            await UniTask.Yield();
        }

        _isActionState = true;
        _animator.applyRootMotion = true;

        await DOTween
            .To(() => 0f, x =>
                {
                    if (_lockAtObstacle)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, _requiredRotation, 100f * x);
                    }
                },
                _animator.GetNextAnimatorStateInfo(0).length, _animator.GetNextAnimatorStateInfo(0).length);

        _isActionState = false;
        _animator.applyRootMotion = false;
    }
}
