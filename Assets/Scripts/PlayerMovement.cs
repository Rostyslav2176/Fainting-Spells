using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    float _speed;
    public float currentSpeed;
    float _groundDrag;
    public float currentDrag;
    public float maxDrag;
    public float jumpForce;
    public float crouchSpeed;
    public float crouchYScale;
    float _startYScale;

    [Header("Key Binds")] 
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")] 
    public float playerHeight = 2f;
    float _groundCheckDistance;
    float _bufferCheckDistance = 0.1f;
    bool _onGround = false;

    public Transform orientation;

    float _horizontalInput;
    float _verticalInput;

    Vector3 _moveDirection;

    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _speed = currentSpeed;
        _groundDrag = currentDrag;
        _startYScale = transform.localScale.y;
    }

    void Update()
    {
        // Ground check 
       _groundCheckDistance = (GetComponent<CapsuleCollider>().height/2) + _bufferCheckDistance;

        // Apply drag only when grounded
        _rb.linearDamping = _onGround ? _groundDrag : 0f;
        
        PlayerInput();

        // Jump
        if (Input.GetKeyDown(jumpKey) && _onGround)
        {
            Jump();
        }
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance))
        {
            _onGround = true;
        }
        else
        {
            _onGround = false;
        }
        
        //Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            _speed = crouchSpeed;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
            _speed = currentSpeed;
            
        }
        
        SpeedControl();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        
        if (_horizontalInput == 0 && _verticalInput == 0)
        {
            _groundDrag = maxDrag;
        }
        else
        {
            _groundDrag = currentDrag;
        }
    }

    private void MovePlayer()
    {
        // Calculate move direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        _rb.AddForce(_moveDirection.normalized * _speed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVel.magnitude > _speed)
        {
            Vector3 limitedVel = flatVel.normalized * _speed;
            _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset vertical velocity to ensure consistent jump
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        // Apply upward force
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
