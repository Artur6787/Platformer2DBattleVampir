using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Animator _animator;
    [SerializeField] private Player _cachedPlayer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            _cachedPlayer = player;
            _enemy.OnHitAnimationStart();
            _animator.SetTrigger("HitTrigger");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out _))
        {
            _cachedPlayer = null;
        }
    }

    public void DealDamage()
    {
        Vector2 enemyPosition = transform.position;
        Vector2 playerPosition = _cachedPlayer.transform.position;
        float sqrDistance = (playerPosition - enemyPosition).sqrMagnitude;

        if (sqrDistance >= _attackRange)
            return;

        _cachedPlayer.TakeDamage(_damage);
    }

    public void OnHitEnd()
    {
        _enemy.OnHitAnimationEnd();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}