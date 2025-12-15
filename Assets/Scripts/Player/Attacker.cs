using System;
using System.Collections;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Collider2D _attackHitbox;
    [SerializeField] private Vector2 _initialLocalPosition;
    [SerializeField] private float _startTime = 0.2f;
    [SerializeField] private Transform _hitPosition;
    [SerializeField] private LayerMask _enemy;
    [SerializeField] private float _hitRange;
    [SerializeField] private int _damageAmount;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private AnimationEventRelay _relay;

    private bool _isHitting;
    private bool _hasHit;

    public event Action AttackStarted;
    public event Action AttackEnded;
    public event Action AttackHit;

    private void Start()
    {
        if (_relay != null)
        {
            _relay.HitEvent += OnAnimationHit;
            _relay.AttackEndEvent += OnAnimationEnd;
        }
    }

        private void OnEnable()
    {
        _inputHandler.AttackCommand += HandleHitInput;
    }

    private void OnDisable()
    {
        _inputHandler.AttackCommand -= HandleHitInput;

        if (_relay != null)
        {
            _relay.HitEvent -= OnAnimationHit;
            _relay.AttackEndEvent -= OnAnimationEnd;
        }
    }

    private void LateUpdate()
    {
        float directionSign = Mathf.Sign(transform.localScale.x);
        _attackHitbox.transform.localPosition = new Vector3(_initialLocalPosition.x * directionSign, _initialLocalPosition.y, 0);
    }

    private void OnDrawGizmosSelected()
    {
        if (_hitPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_hitPosition.position, _hitRange);
        }
    }

    public void OnAnimationHit()
    {
        if (_isHitting == true && _hasHit == false)
        {
            PerformHit();
            AttackHit?.Invoke();
        }
    }

    public void OnAnimationEnd()
    {
        ResetAttack();
    }

    private void HandleHitInput()
    {
        if (_isHitting == false)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        _isHitting = true;
        _hasHit = false;
        AttackStarted?.Invoke();
        StartCoroutine(AttackRoutine());
    }

    private void PerformHit()
    {
        Debug.Log("PerformHit вызван");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_hitPosition.position, _hitRange, _enemy);
        Debug.Log("Найдено врагов: " + enemies.Length);

        foreach (var enemyCollider in enemies)
        {
            Debug.Log("Пробуем нанести урон: " + enemyCollider.name);

            if (enemyCollider.TryGetComponent<DamageReceiver>(out DamageReceiver damageReceiver))
            {
                Debug.Log("DamageReceiver найден, наносим урон: " + enemyCollider.name);
                damageReceiver.TakeDamage(_damageAmount);
            }
            else
            {
                Debug.Log("DamageReceiver НЕ найден у: " + enemyCollider.name);
            }
        }

        _hasHit = true;
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(_startTime);
    }

    private void ResetAttack()
    {
        _isHitting = false;
        _hasHit = false;
        AttackEnded?.Invoke();
    }
}