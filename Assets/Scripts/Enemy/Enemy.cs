using UnityEngine;

[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Vision))]
[RequireComponent(typeof(EnemyAttack))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyAttack _attack;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Patroller _patroller;
    [SerializeField] private Health _health;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Vision _vision;

    private bool _isAttacking;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _patroller = GetComponent<Patroller>();
        _health = GetComponent<Health>();
        _rotator = GetComponent<Rotator>();
        _vision = GetComponent<Vision>();
        _animator = GetComponentInChildren<EnemyAnimator>();
        _attack = GetComponent<EnemyAttack>();
    }

    private void OnEnable()
    {
        _health.Died += OnDeath;
        _animator.AttackEnded += EndAttack;
        _attack.AttackStarted += StartAttack;
    }

    private void OnDisable()
    {
        _health.Died -= OnDeath;
        _animator.AttackEnded -= EndAttack;
        _attack.AttackStarted -= StartAttack;
    }

    private void FixedUpdate()
    {
        if (_isAttacking)
        {
            LookAtPlayer();
            return;
        }

        Vector2 targetPosition;

        if (_vision.IsPlayerVisible())
        {
            targetPosition = _vision.GetTargetPosition();
        }
        else
        {
            targetPosition = _patroller.GetNextTargetPosition();
        }


        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        _mover.MoveTowards(targetPosition);
        _rotator.Reflect(direction);
    }

    private void LookAtPlayer()
    {
        if (!_vision.IsPlayerVisible())
            return;

        Vector2 direction = (_vision.GetTargetPosition() - (Vector2)transform.position).normalized;

        _rotator.Reflect(direction);
    }

    public void StartAttack()
    {
        _isAttacking = true;
    }

    public void EndAttack()
    {
        _isAttacking = false;
    }

    private void OnDeath()
    {
        Debug.Log($"{name} уничтожен");
    }



    //[SerializeField] private EnemyMover _mover;
    //[SerializeField] private Patroller _patroller;
    //[SerializeField] private Health _health;
    //[SerializeField] private Rotator _directionHandler;
    //[SerializeField] private Vision _vision;

    //private bool _isHitAnimationPlaying = false;
    //private bool _lookAtPlayerDuringAttack = false;

    //private void Awake()
    //{
    //    _mover = GetComponent<EnemyMover>();
    //    _patroller = GetComponent<Patroller>();
    //    _health = GetComponent<Health>();
    //    _directionHandler = GetComponent<Rotator>();
    //    _vision = GetComponent<Vision>();
    //}

    //private void FixedUpdate()
    //{
    //    if (_lookAtPlayerDuringAttack && _vision != null && _vision.IsPlayerVisible())
    //    {
    //        Vector2 directionToPlayer = (_vision.GetTargetPosition() - (Vector2)transform.position).normalized;
    //        _directionHandler.Reflect(directionToPlayer);
    //        return;
    //    }

    //    if (_isHitAnimationPlaying)
    //        return;

    //    Vector2 targetPos;

    //    if (_vision != null && _vision.IsPlayerVisible())
    //    {
    //        targetPos = _vision.GetTargetPosition();
    //    }
    //    else
    //    {
    //        targetPos = _patroller.GetNextTargetPosition();
    //    }

    //    Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
    //    _mover.MoveTowards(targetPos);
    //    _directionHandler.Reflect(direction);
    //}

    //private void OnEnable()
    //{
    //    _health.Died += OnDeath;
    //}

    //private void OnDisable()
    //{
    //    _health.Died -= OnDeath;
    //}

    //public void OnHitAnimationStart()
    //{
    //    _isHitAnimationPlaying = true;
    //    _lookAtPlayerDuringAttack = true;
    //}

    //public void OnHitAnimationEnd()
    //{
    //    _isHitAnimationPlaying = false;
    //    _lookAtPlayerDuringAttack = false;
    //}

    //private void OnDeath()
    //{
    //    Debug.Log($"{name} уничтожен");
    //}
}