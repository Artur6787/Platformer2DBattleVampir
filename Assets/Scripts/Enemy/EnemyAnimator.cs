using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private Enemy _enemy;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out _))
        {
            _enemy.OnHitAnimationStart();
            Debug.Log("Враг столкнулся с игроком, запускаем анимацию удара.");
            _animator.SetTrigger("HitTrigger");
        }
    }

    public void OnHitStart()
    {
        _enemy.OnHitAnimationStart();
    }

    public void OnHitEnd()
    {
        _enemy.OnHitAnimationEnd();
    }
}