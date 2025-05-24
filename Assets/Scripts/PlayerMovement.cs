using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Sprint/Walk")] 
    float _speed;
    public float currentSpeed;
    public float walkSpeed;
    private bool _walkInAir;
    float _groundDrag;
    public float currentDrag;
    
    [Header("Crouch")] 
    public float crouchSpeed;
    public float crouchYScale;
    float _startYScale;
    private bool _crouchInAir;
    RaycastHit _crouchHit;
    private float _crouchHeightCheck = 0.2f;
    private float _playerCrouchHeight = 0.5f;
    private bool _underObstacle;
    private bool _isCrouching = false;
    
    [Header("Jump")] 
    public float jumpForce; 
    public float airMultiplier;
    public bool extraJump;

    [Header("Key Binds")] 
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode walkKey = KeyCode.LeftShift;

    [Header("Ground Check")] 
    float _groundCheckDistance;
    float _bufferCheckDistance = 0.3f;
    bool _onGround = false;
    private bool _groundedLastFrame;
    
    [Header("Slope Check")] 
    private float _slopeCheckDistance = 1f;
    private RaycastHit _slopeHit;
    private float _playerHeight = 2f;

    [Header("Movement Input")] 
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
        //Turn gravity off on ground
        _rb.useGravity = !_onGround;
        
        PlayerInput();
        SpeedControl();
        Jump();
        ExtraJump();
        Walk();
        Crouch();
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance))
        {
            _onGround = true;
            if (!_groundedLastFrame)
            {
                //Just landed
                if (_crouchInAir)
                {
                    _speed = crouchSpeed;
                }

                if (_walkInAir)
                {
                    _speed = walkSpeed;
                }
            }
            extraJump = true;
            _crouchInAir = false;
            _walkInAir = false;
        }
        else
        {
            _onGround = false;
            
            if (Input.GetKeyDown(crouchKey)) _crouchInAir = true;
            if (Input.GetKeyUp(crouchKey)) _crouchInAir = false;
            if (Input.GetKeyDown(walkKey)) _walkInAir = true;
            if (Input.GetKeyUp(walkKey)) _walkInAir = false;
        }
        _groundedLastFrame = _onGround;
        
        if(Physics.Raycast(transform.position, Vector3.up, out _crouchHit, (_playerCrouchHeight / 0.5f) + _crouchHeightCheck ))
        {
            _underObstacle = true;
        }
        else
        {
            _underObstacle = false;
            
            // Automatically uncrouch if player is no longer under an obstacle
            if (_isCrouching && !_underObstacle && !Input.GetKey(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
                _speed = currentSpeed;
            }
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
        
        if (OnSlope())
        {
            // Project movement onto slope plane
            Vector3 slopeDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
            _rb.AddForce(slopeDirection * _speed * 10f, ForceMode.Force);
            
        }
        else
        {
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
        }
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
        // Jump
        if (Input.GetKeyDown(jumpKey) && _onGround)
        {
            // Reset vertical velocity to ensure consistent jump
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

            // Apply upward force
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ExtraJump()
    {
        if (Input.GetKeyDown(jumpKey) && !_onGround && extraJump)
        {
            extraJump = false;
            
            // Reset vertical velocity to ensure consistent jump
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            
            // Apply upward force 
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Crouch()
    {
        //Crouch
        if (Input.GetKeyDown(crouchKey) && _onGround) //On ground
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            _speed = crouchSpeed;
            _isCrouching = true;
        }
        
        if (Input.GetKeyDown(crouchKey) && !_onGround) //In air
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            _isCrouching = true;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            if (!_underObstacle)
            {
                transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
                _speed = currentSpeed;
                _isCrouching = false;
            }
        }
    }
    
    private void Walk()
    {
        //Walk
        if (Input.GetKeyDown(walkKey) && _onGround) //On ground
        {
            _speed = walkSpeed;
        }
        
        if(Input.GetKeyDown(walkKey) && !_onGround)
        {
            _speed = currentSpeed;
        }

        if (Input.GetKeyUp(walkKey))
        {
            _speed = currentSpeed;
        } 
    }

    private bool OnSlope()
    {
        if(!_onGround) return false;

        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, (_playerHeight / 2) + _slopeCheckDistance))
        {
            if (_slopeHit.normal != Vector3.up)
                return true;
        }
        return false;
    }
}
