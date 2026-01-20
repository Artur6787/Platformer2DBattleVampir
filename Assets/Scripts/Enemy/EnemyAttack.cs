using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackRange = 2f;

    private Player _cachedPlayer;
    [SerializeField] private EnemyAnimator _animator;

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

        //_animator = GetComponent<EnemyAnimator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Player player))
            return;

        _cachedPlayer = player;
        AttackStarted?.Invoke();
        _animator.PlayAttack();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            _cachedPlayer = null;
        }
    }

    // вызывается анимационным эвентом
    public void DealDamage()
    {
        if (_cachedPlayer == null)
            return;

        float sqrDistance = (_cachedPlayer.transform.position - transform.position).sqrMagnitude;

        if (sqrDistance > _attackRange * _attackRange)
            return;

        _cachedPlayer.TakeDamage(_damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
