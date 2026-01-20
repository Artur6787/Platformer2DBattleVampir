using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Rotator _directionHandler;
    [SerializeField] private AnimationHandler _animationHandler;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private Player _player;

    private Rigidbody2D _rigidbody2d;
    private Vector2 _currentInputVector;
    private bool _jumpRequested;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        _inputHandler = GetComponent<InputHandler>();
        _directionHandler = GetComponent<Rotator>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void OnEnable()
    {
        _inputHandler.MoveCommand += HandleMovementInput;
        _inputHandler.JumpCommand += HandleJumpInput;
        _groundDetector.GroundedChanged += SetGroundedState;
    }

    private void OnDisable()
    {
        _inputHandler.MoveCommand -= HandleMovementInput;
        _inputHandler.JumpCommand -= HandleJumpInput;
        _groundDetector.GroundedChanged -= SetGroundedState;
    }

    private void Update()
    {
        UpdateAnimation();
        UpdateDirection();
    }

    private void FixedUpdate()
    {
        ProcessMovement();
        ProcessJump();
    }

    private void HandleMovementInput(Vector2 moveInput)
    {
        _currentInputVector = moveInput;
    }

    private void HandleJumpInput()
    {
        if (IsGrounded && CanMove())
            _jumpRequested = true;
    }

    private void ProcessMovement()
    {
        if (!CanMove())
        {
            _rigidbody2d.velocity = new Vector2(0, _rigidbody2d.velocity.y);
            return;
        }

        _rigidbody2d.velocity = new Vector2(_currentInputVector.x * _speed,_rigidbody2d.velocity.y);
    }

    private void ProcessJump()
    {
        if (!_jumpRequested || !CanMove())
            return;

        _rigidbody2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        SetGroundedState(false);
        _jumpRequested = false;
    }

    private void UpdateDirection()
    {
        if (_directionHandler == null)
            return;

        if (_currentInputVector.x != 0)
            _directionHandler.Reflect(new Vector3(_currentInputVector.x, 0, 0));
    }

    private void UpdateAnimation()
    {
        bool isJumping = !IsGrounded && CanMove();
        bool isRunning = _currentInputVector.x != 0 && IsGrounded && CanMove();

        _animationHandler.UpdateJumpState(isJumping);
        _animationHandler.UpdateRunState(isRunning);
    }

    private void SetGroundedState(bool grounded)
    {
        IsGrounded = grounded;
    }

    private bool CanMove()
    {
        if (_player == null)
            return true;

        return _player.CanMove;
    }
}