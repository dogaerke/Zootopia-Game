using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed = 150f;

    private Camera _camera;
    private DynamicJoystick _dynamicJoystick;
    private Canvas _canvasObject;
    private float _horizontal;
    private float _vertical;
    [SerializeField] private Animator animator;
    
    private static readonly int SpeedAnim = Animator.StringToHash("Speed");
    private static readonly int Carry = Animator.StringToHash("Carry");

    public float Speed
    {
        get => speed;
        set => speed = value;
    }


    private void Awake()
    {
        _dynamicJoystick = FindObjectOfType<DynamicJoystick>();
        _camera = Camera.main;
    }

    private void Update()
    {
        GetMovementInputs();

    }

    private void FixedUpdate()
    {
        SetMovement();
        SetRotation();
    }


    private void SetMovement()
    {
        var velocity = playerRigidBody.velocity = GetNewVelocity();
        
        if (Player.Instance.stackIndex > 0)
        {
            animator.SetBool(Carry, true);
        }
        else if(Player.Instance.stackIndex <= 0)
        {
            animator.SetBool(Carry, false);
        }
        
        animator.SetFloat(SpeedAnim, velocity.magnitude);
        
        
    }

    private Vector3 GetNewVelocity()
    {
        var cameraTransform = _camera.transform;
        var forward = cameraTransform.forward * _vertical;
        var right = cameraTransform.right * _horizontal;
        
        forward.y = 0f;
        right.y = 0f;
        
        return (forward + right).normalized * (speed * Time.fixedDeltaTime);
    }

    private void GetMovementInputs()
    {
        _horizontal = _dynamicJoystick.Horizontal;
        _vertical = _dynamicJoystick.Vertical;
    }

    private void SetRotation()
    {
        if (_horizontal != 0 || _vertical != 0 && GetNewVelocity() != Vector3.zero)
        {
            playerTransform.rotation = Quaternion.LookRotation(GetNewVelocity());
        }
    }
}
