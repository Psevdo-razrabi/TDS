using UnityEngine;

public class TestHeroMovement : MonoBehaviour
{
    private TestAIMovement _input;

    [SerializeField]
    private Camera _camera;
    private Vector3 _moveDir;
    [SerializeField]
    private float _speed = 1F;

    private void Keybord_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 m = obj.ReadValue<Vector2>();
        _moveDir = new Vector3(m.x, 0, m.y);
    }

    private void Awake()
    {
        _input = new TestAIMovement();
        _input.Move.Keybord.performed += Keybord_performed;
        _input.Enable();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _moveDir * _speed * Time.deltaTime;
        _camera.transform.position = new Vector3(transform.position.x, _camera.transform.position.y, transform.position.z - 10);
    }
}
