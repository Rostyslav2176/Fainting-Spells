using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;

    public float groundDrag;

    [Header("Ground Check")] 
    public float playerHeight;
    public LayerMask whatIsGround;
    bool _onGround;
    
    public Transform orientation;
    
    float _horizontalInput;
    float _verticalInput;
    
    Vector3 _moveDirection;
    
    Rigidbody _rb;
    void Start()
    {
        //Set rigidbody and freeze its rotation
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    void Update()
    {
        //Groung check
        _onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        //Handle drag
        if(_onGround)
            _rb.linearDamping = groundDrag;
        else
            _rb.linearDamping = 0;
        
        PlayerInput();
        SpeedControl();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        //Get player input
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        //Calculate movement direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        
        _rb.AddForce(_moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        
        //Limit velocity
        if (flatVel.magnitude > speed)
        {
            Vector3 limitVel = flatVel.normalized * speed;
            _rb.linearVelocity = limitVel;
        }
    }
    
}
