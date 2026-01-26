using System;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private EnemyAnimator _animator;

    private bool _canAttack = true;

    public event Action AttackStarted;

    private void OnEnable()
    {
        _animator.AttackHit += DealDamage;
    }

    private void OnDisable()
    {
        _animator.AttackHit -= DealDamage;
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<EnemyAnimator>();
    }

    private void Update()
    {
        if (_canAttack && IsPlayerInRange())
            StartAttack();
    }

    private void StartAttack()
    {
        _canAttack = false;
        AttackStarted?.Invoke();
        _animator.PlayAttack();
        Invoke(nameof(ResetCooldown), _attackCooldown);
    }

    private void ResetCooldown()
    {
        _canAttack = true;
    }

    private bool TryGetPlayerInRange(out Player player)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _attackRange);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out player))
                return true;
        }

        player = null;
        return false;
    }

    private bool IsPlayerInRange()
    {
        return TryGetPlayerInRange(out _);
    }

    private void DealDamage()
    {
        if (TryGetPlayerInRange(out Player player))
        {
            player.TakeDamage(_damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
    //    [SerializeField] private int _damage = 10;
    //    [SerializeField] private float _attackRange = 2f;
    //    [SerializeField] private EnemyAnimator _animator;
    //    [SerializeField] private float _attackCooldown = 1f;

    //    private bool _playerInRange;
    //    private bool _canAttack = true;
    //    private Player _cachedPlayer;

    //    public event Action AttackStarted;

    //    private void OnEnable()
    //    {
    //        _animator.AttackHit += DealDamage;
    //    }

    //    private void OnDisable()
    //    {
    //        _animator.AttackHit -= DealDamage;
    //    }

    //    private void Awake()
    //    {
    //        _animator = GetComponentInChildren<EnemyAnimator>();
    //    }

    //    private void Update()
    //    {
    //        if (_playerInRange && _canAttack)
    //            StartAttack();
    //    }

    //    private void StartAttack()
    //    {
    //        _canAttack = false;
    //        AttackStarted?.Invoke();
    //        _animator.PlayAttack();
    //        Invoke(nameof(ResetCooldown), _attackCooldown);
    //    }

    //    private void ResetCooldown()
    //    {
    //        _canAttack = true;
    //    }

    //    private void OnTriggerEnter2D(Collider2D other)
    //    {
    //        if (!other.TryGetComponent(out Player player))
    //            return;

    //        _cachedPlayer = player;
    //        _playerInRange = true;
    //    }

    //    private void OnTriggerExit2D(Collider2D other)
    //    {
    //        if (other.TryGetComponent<Player>(out _))
    //        {
    //            _cachedPlayer = null;
    //            _playerInRange = false;
    //        }
    //    }

    //    public void DealDamage()
    //    {
    //        if (_cachedPlayer == null)
    //            return;

    //        float sqrDistance = (_cachedPlayer.transform.position - transform.position).sqrMagnitude;

    //        if (sqrDistance > _attackRange * _attackRange)
    //            return;

    //        _cachedPlayer.TakeDamage(_damage);
    //    }

    //    private void OnDrawGizmosSelected()
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(transform.position, _attackRange);
    //    }
}