using UnityEngine;

[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(MoverObject))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Vision))]
[RequireComponent(typeof(EnemyAttacker))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyAttacker _attack;
    [SerializeField] private MoverObject _mover;
    [SerializeField] private Patroller _patroller;
    [SerializeField] private Health _health;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Vision _vision;

    private bool _isAttacking;

    private void Awake()
    {
        _mover = GetComponent<MoverObject>();
        _patroller = GetComponent<Patroller>();
        _health = GetComponent<Health>();
        _rotator = GetComponent<Rotator>();
        _vision = GetComponent<Vision>();
        _animator = GetComponentInChildren<EnemyAnimator>();
        _attack = GetComponent<EnemyAttacker>();
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
        if (_vision.IsPlayerVisible() == false)
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
}