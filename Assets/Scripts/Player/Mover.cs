using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Rotator _directionHandler;
    [SerializeField] private Attacker _attacker;
    [SerializeField] private AnimationHandler _animationHandler;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private GroundDetector _groundDetector;

    private Rigidbody2D _rigidbody2d;   
    private Vector2 _currentInputVector;   
    private bool _jumpRequested;
    private bool _isAttacking;

    public event Action<bool> GroundedStateChanged;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _inputHandler = GetComponent<InputHandler>();
        _directionHandler = GetComponent<Rotator>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void OnEnable()
    {
        _inputHandler.MoveCommand += HandleMovementInput;
        _inputHandler.JumpCommand += HandleJumpInput;
        _groundDetector.GroundedChanged += SetGroundedState;

        if (_attacker != null)
        {
            _attacker.AttackStarted += StartAttack;
            _attacker.AttackEnded += EndAttack;
        }
    }

    private void OnDisable()
    {
        _inputHandler.MoveCommand -= HandleMovementInput;
        _inputHandler.JumpCommand -= HandleJumpInput;
        _groundDetector.GroundedChanged -= SetGroundedState;

        if (_attacker != null)
        {
            _attacker.AttackStarted -= StartAttack;
            _attacker.AttackEnded -= EndAttack;
        }
    }

    private void Update()
    {
        UpdateAnimation();

        if (_directionHandler != null && _currentInputVector.x != 0 && _isAttacking == false)
        {
            Vector3 direction = new Vector3(_currentInputVector.x, 0, 0);
            _directionHandler.Reflect(direction);
        }
    }

    private void FixedUpdate()
    {
        ProcessMovement();
        Jump();
    }

    public void StartAttack()
    {
        _isAttacking = true;
        _animationHandler.TriggerAttack();
    }

    public void EndAttack()
    {
        _isAttacking = false;
    }

    private void HandleMovementInput(Vector2 moveInput)
    {
        _currentInputVector = moveInput;
    }

    private void HandleJumpInput()
    {
        if (IsGrounded && _isAttacking == false)
        {
            _jumpRequested = true;
        }
    }

    private void ProcessMovement()
    {
        if (_isAttacking == false)
        {
            _rigidbody2d.velocity = new Vector2(_currentInputVector.x * _speed, _rigidbody2d.velocity.y);
        }
        else
        {
            _rigidbody2d.velocity = new Vector2(0, _rigidbody2d.velocity.y);
        }
    }

    private void Jump()
    {
        if (_jumpRequested && _isAttacking == false)
        {
            _rigidbody2d.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            SetGroundedState(false);
            _jumpRequested = false;
        }
    }

    private void UpdateAnimation()
    {
        bool isJumping = IsGrounded == false && _isAttacking == false;
        bool isRunning = _currentInputVector.x != 0 && IsGrounded && _isAttacking == false;
        _animationHandler.UpdateJumpState(isJumping);
        _animationHandler.UpdateRunState(isRunning);
    }

    private void SetGroundedState(bool grounded)
    {
        if (IsGrounded != grounded)
        {
            IsGrounded = grounded;
            GroundedStateChanged?.Invoke(IsGrounded);
        }
    }
}