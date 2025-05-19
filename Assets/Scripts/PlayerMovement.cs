using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    float _speed;
    public float currentSpeed;
    public float walkSpeed;
    public float airMultiplier;
    float _groundDrag;
    public float currentDrag;
    public float jumpForce;
    public float crouchSpeed;
    public float crouchYScale;
    float _startYScale;

    [Header("Key Binds")] 
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode walkKey = KeyCode.LeftShift;

    [Header("Ground Check")] 
    float _groundCheckDistance;
    float _bufferCheckDistance = 0.1f;
    bool _onGround = false;
    
    [Header("On Slope Check")] 
    public float maxSlopeAngle;
    public RaycastHit SlopeHit;
    private float _playerHeight = 2.0f;

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
        SpeedControl();

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
        if (Input.GetKeyDown(crouchKey) && _onGround) //On ground
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            _speed = crouchSpeed;
        }
        
        if (Input.GetKeyDown(crouchKey) && !_onGround) //In air
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            _speed = crouchSpeed;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
            _speed = currentSpeed;
            
        }
        
        //Walk
        if (Input.GetKeyDown(walkKey))
        {
            _speed = walkSpeed;
        }

        if (Input.GetKeyUp(walkKey))
        {
            _speed = currentSpeed;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate move direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        
        //On ground
        if (_onGround)
        {
            _rb.AddForce(_moveDirection.normalized * _speed * 10f, ForceMode.Force);
        }
        
        //In air
        if (!_onGround)
        {
            _rb.AddForce(_moveDirection.normalized * _speed * 10f * airMultiplier, ForceMode.Force); 
        }
        
        //On slope turn off gravity
        _rb.useGravity = !OnSlope();
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

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, _playerHeight * 0.5f + 0.3f))
        {
           float angle = Vector3.Angle(SlopeHit.normal, Vector3.up);
           return angle < maxSlopeAngle && angle != 0;
        }
        
        return false;
    }
}
