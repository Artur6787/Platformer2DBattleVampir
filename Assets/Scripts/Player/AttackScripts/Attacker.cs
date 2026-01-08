using System;
using System.Collections;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _hitRange = 0.8f;
    [SerializeField] private LayerMask _enemy;
    [SerializeField] private Transform _hitPosition;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private AnimationHandler _animationHandler;
    [SerializeField] private AnimationEventRelay _relay;
    [SerializeField] private float _attackMaxDuration = 0.6f;

    private bool _isAttacking;
    private bool _hasHit;
    private Coroutine _safetyCoroutine;

    public event Action AttackStarted;
    public event Action AttackEnded;
    public event Action AttackHit;

    private void OnEnable()
    {
        _inputHandler.AttackCommand += OnAttackInput;
        _relay.HitEvent += OnAnimationHit;
        _relay.AttackEndEvent += OnAnimationEnd;
    }

    private void OnDisable()
    {
        _inputHandler.AttackCommand -= OnAttackInput;
        _relay.HitEvent -= OnAnimationHit;
        _relay.AttackEndEvent -= OnAnimationEnd;
    }

    private void OnAttackInput()
    {
        if (_isAttacking == true)
            return;

        StartAttack();
    }

    private void StartAttack()
    {
        _isAttacking = true;
        _hasHit = false;
        _animationHandler.TriggerAttack();
        AttackStarted?.Invoke();

        if (_safetyCoroutine != null)
            StopCoroutine(_safetyCoroutine);

        _safetyCoroutine = StartCoroutine(AttackSafetyTimer());
    }

    private IEnumerator AttackSafetyTimer()
    {
        yield return new WaitForSeconds(_attackMaxDuration);
        OnAnimationEnd();
    }

    public void OnAnimationHit()
    {
        if (_isAttacking == false || _hasHit == true)
            return;

        PerformHit();
        _hasHit = true;
        AttackHit?.Invoke();
    }

    public void OnAnimationEnd()
    {
        if (_isAttacking == false)
            return;

        _isAttacking = false;
        _hasHit = false;

        if (_safetyCoroutine != null)
        {
            StopCoroutine(_safetyCoroutine);
            _safetyCoroutine = null;
        }

        AttackEnded?.Invoke();
    }

    private void PerformHit()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_hitPosition.position, _hitRange, _enemy);

        foreach (var enemy in enemies)
        {
            if (enemy.TryGetComponent(out DamageReceiver damageReceiver))
            {
                damageReceiver.TakeDamage(_damageAmount);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hitPosition.position, _hitRange);
    }
}