using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player;
using UniRx;
using UnityEngine;

public class ParkourSystem : MonoBehaviour
{ 
    private Vector3 _targetPosition;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController; 
    private float _obstacleHeight;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _isActionState = true;
    [SerializeField] private GameObject _step;
    [SerializeField] private float _heightCorrectionForStep;
    [SerializeField] private float _heightCorrectionForClimb;
    [SerializeField] private float _heightCorrectionForClimbOnWall;
    [SerializeField] private Player _player;
    [SerializeField] private bool _isPlayerOnObstacle;
    [SerializeField] private bool _lockAtObstacle;
    private Quaternion _requiredRotation;
    
    private CompositeDisposable _disposable = new();

    [Header("Raycast Settings")] 
    [SerializeField] private float _rayCastDistance;

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

        //Step();
        _animator.SetBool("onSurface", _characterController.isGrounded);
        var (isLanding, _) = CheckLanding();

        if (_isPlayerOnObstacle && !_lockAtObstacle && isLanding)
        {
            _isPlayerOnObstacle = false;
            await HandleObstacleJump();
        }

        // MatchAnimatorState("Climb", AvatarTarget.RightFoot, new Vector3(0f, 1f, 1f), 0.14f, 0.33f);
        // MatchAnimatorState("Step", AvatarTarget.RightFoot, new Vector3(0f, 1f, 0f), 0.42f, 0.50f);
        // MatchAnimatorState("ClimbToWall", AvatarTarget.RightHand, new Vector3(0f, 1f, 1f), 0.05f, 0.33f);
    }

    /*private void Step()
    {
        if (Physics.Raycast(_step.transform.position, _step.transform.forward, out RaycastHit hit, _rayCastDistance,
                _layerMask))
        {
            _obstacleHeight = hit.collider.bounds.size.y;
            _targetPosition = GetRaisedEndPoint(hit);
            _lockAtObstacle = true;
            _requiredRotation = Quaternion.LookRotation(-hit.normal);
            DrawDebugLine(hit.point, _targetPosition, -hit.normal);
        }
        else
        {
            _lockAtObstacle = false;
        }
    }*/

    /*private Vector3 GetRaisedEndPoint(RaycastHit hit)
    {
        float correctionHeight = _obstacleHeight switch
        {
            >= 0.4f and <= 0.8f => _heightCorrectionForStep,
            >= 0.8f and <= 1.5f => _heightCorrectionForClimb,
            >= 1.5f and <= 5f => _heightCorrectionForClimbOnWall,
            _ => 0f
        };

        return hit.point + Vector3.up * (hit.collider.bounds.size.y - correctionHeight);
    }*/

    private (bool isLanding, RaycastHit hitObject) CheckLanding()
    {
        if (_isActionState) return (false, default);


        if (!Physics.Raycast(_step.transform.position, Vector3.down, out RaycastHit hitInfo, _rayCastDistance,
                _layerMask))
        {
            return (true, hitInfo);
        }

        return (false, default);
    }

    private async UniTask HandleObstacleJump()
    {
        if (_obstacleHeight is >= 3f and <= 4f)
        {
            //PlayJumpAnimation("Jumping", _animationJumpOffBigSpeed);

        }
        else if (_obstacleHeight is >= 1.2f and <= 1.6f)
        {
            //PlayJumpAnimation("JumpDown", _animationJumpOffMediumSpeed);

        }
    }

    private async UniTask PlayJumpAnimation(string animationName, float animationSpeed)
    {
        _animator.Play(animationName);
        _animator.speed = animationSpeed;

        while (_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            if (_characterController.isGrounded)
            {
                _animator.Play("Idle");
                break;
            }

        await UniTask.Yield();

    }

    /*private async UniTask PlayClimbAnimation()
    {
        string animationName = _obstacleHeight switch
        {
            >= 0.4f and <= 0.8f => "Step",
            >= 0.8f and <= 1.5f => "Climb",
            >= 1.5f and <= 4f => "ClimbToWall",
            _ => string.Empty
        };

        if (!string.IsNullOrEmpty(animationName))
        {
             _animator.CrossFade(animationName, 0.2f);
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

    private void MatchAnimatorState(string stateName, AvatarTarget target, Vector3 weightMask, float start, float end)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            _animator.MatchTarget(_targetPosition, transform.rotation, target, new MatchTargetWeightMask(weightMask, 0),
                start, end);
        }
    }*/

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(_step.transform.position + Vector3.down * 0.2f, 0.5f);
    //     Gizmos.DrawLine(_step.transform.position, _step.transform.position + Vector3.down * 0.5f);
    // }
    //
    // private void DrawDebugLine(Vector3 startPoint, Vector3 endPoint, Vector3 normal)
    // {
    //     Debug.DrawLine(_step.transform.position, startPoint, Color.red);
    //     Debug.DrawLine(startPoint, endPoint, Color.cyan);
    // }
}