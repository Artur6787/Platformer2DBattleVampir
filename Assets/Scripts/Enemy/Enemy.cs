using UnityEngine;

[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(Vision))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Patroller _patroller;
    [SerializeField] private Health _health;
    [SerializeField] private Rotator _directionHandler;
    [SerializeField] private Vision _vision;

    private bool _isHitAnimationPlaying = false;
    private bool _lookAtPlayerDuringAttack = false;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _patroller = GetComponent<Patroller>();
        _health = GetComponent<Health>();
        _directionHandler = GetComponent<Rotator>();
        _vision = GetComponent<Vision>();
    }

    private void FixedUpdate()
    {
        if (_lookAtPlayerDuringAttack && _vision != null && _vision.IsPlayerVisible())
        {
            Vector2 directionToPlayer = (_vision.GetTargetPosition() - (Vector2)transform.position).normalized;
            _directionHandler.Reflect(directionToPlayer);
            return;
        }

        if (_isHitAnimationPlaying)
            return;

        Vector2 targetPos;

        if (_vision != null && _vision.IsPlayerVisible())
        {
            targetPos = _vision.GetTargetPosition();
        }
        else
        {
            targetPos = _patroller.GetNextTargetPosition();
        }

        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        _mover.MoveTowards(targetPos);
        _directionHandler.Reflect(direction);
    }

    private void OnEnable()
    {
        _health.Died += OnDeath;
    }

    private void OnDisable()
    {
        _health.Died -= OnDeath;
    }

    public void OnHitAnimationStart()
    {
        _isHitAnimationPlaying = true;
        _lookAtPlayerDuringAttack = true;
    }

    public void OnHitAnimationEnd()
    {
        _isHitAnimationPlaying = false;
        _lookAtPlayerDuringAttack = false;
    }

    private void OnDeath()
    {
        Debug.Log($"{name} уничтожен");
    }
}