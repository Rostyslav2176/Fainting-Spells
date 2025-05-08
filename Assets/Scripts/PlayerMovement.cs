using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    public float speed;
    public float groundDrag;
    public float jumpForce;

    [Header("Key Binds")] 
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")] 
    public float playerHeight;
    public float fallMultiplier;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    bool _onGround;

    public Transform orientation;

    float _horizontalInput;
    float _verticalInput;

    Vector3 _moveDirection;

    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    void Update()
    {
        // Ground check using CapsuleCast
        Vector3 capsuleBottom = transform.position + Vector3.down * (playerHeight / 2f);
        Vector3 capsuleTop = transform.position + Vector3.up * 0.1f;
        _onGround = Physics.CheckCapsule(capsuleTop, capsuleBottom, groundCheckRadius, whatIsGround);

        // Apply drag only when grounded
        _rb.linearDamping = _onGround ? groundDrag : 0f;
        
        // Enhanced gravity while falling
        if (!_onGround && _rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Jump
        if (Input.GetKeyDown(jumpKey) && _onGround)
        {
            Jump();
        }
        
        SpeedControl();
    }

    void FixedUpdate()
    {
        PlayerInput();
        MovePlayer();
    }

    private void PlayerInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate move direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        _rb.AddForce(_moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
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
