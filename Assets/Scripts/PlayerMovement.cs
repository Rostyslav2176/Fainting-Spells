using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Sprint/Walk")]
    public float normalSpeed;
    private float _speed;
    public float currentDrag;
    private float _groundDrag;

    [Header("Crouch")]
    public float crouchSpeed;
    private float _originalCapsuleHeight;
    private Vector3 _originalCapsuleCenter;
    private bool _isCrouching = false;
    public float crouchHeight = 1f;

    [Header("Jump")]
    public float jumpForce;
    public float airMultiplier;
    public bool extraJump;

    [Header("Key Binds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    private float _groundCheckDistance;
    private float _bufferCheckDistance = 0.3f;
    private bool _onGround = false;
    private bool _groundedLastFrame;

    [Header("Slope Check")]
    private float _slopeCheckDistance = 1f;
    private RaycastHit _slopeHit;
    private float _playerHeight = 2f;

    [Header("Movement Input")]
    public Transform orientation;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    private CapsuleCollider _capsule;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool _canDash = true;
    private bool _isDashing = false;
    private float _dashTimer;
    private float _dashCooldownTimer;
    public ParticleSystem dashEffect;

    [Header("Unlockable Abilities")]
    public bool hasDoubleJump = false;
    public bool hasDash = false;

    public bool couldDoubleJump { get; private set; } = false;
    public bool couldDash { get; private set; } = false;

    public bool isPaused = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsule = GetComponent<CapsuleCollider>();

        _rb.freezeRotation = true;

        _speed = normalSpeed;
        _groundDrag = currentDrag;

        _originalCapsuleHeight = _capsule.height;
        _originalCapsuleCenter = _capsule.center;

        couldDash = hasDash;
        couldDoubleJump = hasDoubleJump;
    }

    private void Update()
    {
        if (isPaused) return;

        _groundCheckDistance = (_capsule.height / 2f) + _bufferCheckDistance;
        _rb.linearDamping = _onGround ? _groundDrag : 0f;
        _rb.useGravity = !_onGround;

        PlayerInput();
        SpeedControl();
        Dash();

        // Ground Check
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, _groundCheckDistance))
        {
            _onGround = true;
            if (!_groundedLastFrame && _isCrouching)
            {
                TryCrouch();
            }
            extraJump = true;
        }
        else
        {
            _onGround = false;
        }

        _groundedLastFrame = _onGround;

        // Auto uncrouch if crouch key is released
        if (_isCrouching && !Input.GetKey(crouchKey))
        {
            TryUncrouch();
        }

        // Dash cooldown handling
        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
            {
                _isDashing = false;
            }
        }
        else if (!_canDash)
        {
            _dashCooldownTimer -= Time.deltaTime;
            if (_dashCooldownTimer <= 0f)
            {
                _canDash = true;
            }
        }

        _rb.linearDamping = _isDashing ? 0f : (_onGround ? _groundDrag : 0f);
    }

    private void LateUpdate()
    {
        if (isPaused) return;

        Crouch();
        Jump();
        ExtraJump();
    }

    private void FixedUpdate()
    {
        if (isPaused) return;

        MovePlayer();
    }

    private void PlayerInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        if (_isDashing) return;

        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (OnSlope())
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
            _rb.AddForce(slopeDirection * _speed * 10f, ForceMode.Force);
        }
        else if (_onGround)
        {
            _rb.AddForce(_moveDirection.normalized * _speed * 10f, ForceMode.Force);
        }
        else
        {
            _rb.AddForce(_moveDirection.normalized * _speed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        if (_isDashing) return;

        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVel.magnitude > _speed)
        {
            Vector3 limitedVel = flatVel.normalized * _speed;
            _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && _onGround)
        {
            DoJump();
        }
    }

    private void ExtraJump()
    {
        if (hasDoubleJump && Input.GetKeyDown(jumpKey) && !_onGround && extraJump)
        {
            extraJump = false;
            couldDoubleJump = true;
            DoJump();
        }
    }

    private void DoJump()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            TryCrouch();
        }

        if (Input.GetKeyUp(crouchKey))
        {
            TryUncrouch();
        }
    }

    private void TryCrouch()
    {
        if (_isCrouching) return;

        _capsule.height = crouchHeight;
        _capsule.center = new Vector3(_capsule.center.x, crouchHeight / 2f, _capsule.center.z);

        _isCrouching = true;
        _speed = crouchSpeed;

        if (_onGround)
        {
            _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
    }

    private void TryUncrouch()
    {
        float castDistance = (_originalCapsuleHeight - crouchHeight) + 0.1f;
        Vector3 castOrigin = transform.position + Vector3.up * crouchHeight / 2f;

        if (Physics.Raycast(castOrigin, Vector3.up, castDistance))
        {
            return; // Can't stand up
        }

        _capsule.height = _originalCapsuleHeight;
        _capsule.center = _originalCapsuleCenter;

        _isCrouching = false;
        _speed = normalSpeed;
    }

    private bool OnSlope()
    {
        if (!_onGround) return false;

        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, (_playerHeight / 2f) + _slopeCheckDistance))
        {
            return _slopeHit.normal != Vector3.up;
        }

        return false;
    }

    private void Dash()
    {
        if (hasDash && Input.GetKeyDown(dashKey) && _canDash && !_isDashing)
        {
            _isDashing = true;
            _canDash = false;
            _dashTimer = dashDuration;
            _dashCooldownTimer = dashCooldown;

            couldDash = true;

            Vector3 dashDirection = (orientation.forward * _verticalInput + orientation.right * _horizontalInput).normalized;

            if (dashDirection == Vector3.zero)
            {
                dashDirection = orientation.forward;
            }

            _rb.linearVelocity = dashDirection * dashForce;

            TriggerDashEffects();
        }
    }

    private void TriggerDashEffects()
    {
        if (dashEffect != null)
        {
            dashEffect.Play();
        }
    }
}
