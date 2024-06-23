using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ParkurSystem : MonoBehaviour
{
    [SerializeField] private Vector3 _ObjPosition;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _ObjectHeight;
    [SerializeField] private Vector3 ToAnimation;
    [SerializeField] private GameObject _step;
    [SerializeField] private GameObject modelRotate;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _isActionState;
    private bool _isClimb;

    [Header("Настройки для луча")] 
    
    [SerializeField] private float rayCastDistance;
    [SerializeField] public float distanceThreshold = 0.1f;
    [SerializeField] public float angleThreshold = -0.5f;
    [SerializeField] private float currentAngle;

    [Header("Настройки для угла")] 
    [SerializeField] private float startMaxAngle;
    

    private async void Update()
    {
        if (_animator.IsInTransition(0))
            return;
        
        Step();
        
        if (UnityEngine.Input.GetButtonDown("Jump") && _isClimb)
        {
            await ClimbAnimation();
        }

        //если у нас щас активен стейт то return, крч перенос в стейт машину проблему решит

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Climb"))
        {
            _animator
                .MatchTarget(_ObjPosition, transform.rotation, AvatarTarget.RightFoot,
                    new MatchTargetWeightMask(new Vector3(0f, 0.9f, 0.9f), 0), 0.14f, 0.33f);
        }
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Step"))
        {
            _animator
                .MatchTarget(_ObjPosition, transform.rotation, AvatarTarget.RightFoot,
                    new MatchTargetWeightMask(new Vector3(0f, 1f, 1f), 0), 0.42f, 0.50f);
        }
    }

    private void Step()
    {
        if (Physics.Raycast(_step.transform.position, _step.transform.forward, out RaycastHit hit, rayCastDistance,
                _layerMask))
        {
            Quaternion rotation = Quaternion.Euler(0f, modelRotate.transform.rotation.y, 0f);

            var forwardVector = transform.position + rotation * Vector3.forward * 1f;
            var rightVector = transform.position + rotation * Vector3.right * 1f;
            var leftVector = transform.position + rotation * Vector3.left * 1f;
            var backVector = transform.position + rotation * Vector3.back * 1f;
            
            CalculateAngle(forwardVector, hit.point);
            CalculateAngle(rightVector, hit.point);
            CalculateAngle(leftVector, hit.point);
            CalculateAngle(backVector, hit.point);

            _ObjectHeight = hit.collider.bounds.size.y;

            Vector3 newPoint = hit.point;
            
            Vector3 raisedEndPoint = newPoint + Vector3.up * (hit.collider.bounds.size.y - 0.05f);
            _ObjPosition = raisedEndPoint;
            _isClimb = true;
            
            DrawLine(newPoint, raisedEndPoint, forwardVector, rightVector, leftVector, backVector);
        }
        else
        {
            _isClimb = false;
        }
    }

    private void DrawLine(Vector3 endPoint, Vector3 raisedEndPoint, Vector3 forwardVector, Vector3 rightVector, Vector3 leftVector, Vector3 backVector)
    {
        Debug.DrawLine(_step.transform.position, endPoint, Color.red);
        Debug.DrawLine(endPoint, raisedEndPoint, Color.cyan);
        Debug.DrawLine(transform.position, forwardVector, Color.green);
        Debug.DrawLine(transform.position, rightVector, Color.yellow);
        Debug.DrawLine(transform.position, leftVector, Color.blue);
        Debug.DrawLine(transform.position, backVector, Color.magenta);
    }

    private void CalculateAngle(Vector3 direction, Vector3 endPoint)
    {
        Debug.DrawLine(transform.position, direction, Color.white);
        if (Physics.Raycast(transform.position, direction, rayCastDistance, _layerMask))
        {
            Quaternion forwardRotation = Quaternion.LookRotation(direction);
            Quaternion endRotation = Quaternion.LookRotation(endPoint);
            float angle = Quaternion.Angle(forwardRotation, endRotation);
            float sign = Mathf.Sign(forwardRotation.eulerAngles.y - endRotation.eulerAngles.y);
            angle *= sign;
            currentAngle = angle;
        }
    }

    private async UniTask ClimbAnimation()
    {
        if (_ObjectHeight is >= 0.4f and <= 0.8f)
        {
            _animator.CrossFade("Step", 0.2f);
            await UniTask.Yield();
        }
        
        if (_ObjectHeight >= 0.8 && _ObjectHeight <= 1.5f)
        {
            _animator.CrossFade("Climb", 0.2f);
            await UniTask.Yield();
        }

        _isActionState = true;
        _animator.applyRootMotion = true;
        _characterController.enabled = false;

        await UniTask.Delay(TimeSpan.FromSeconds(_animator.GetNextAnimatorStateInfo(0).length));

        _isActionState = false;
        _animator.applyRootMotion = false;
        _characterController.enabled = true;
    }
}
