using System;
using UnityEngine;

//[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    //private static readonly int HitTrigger = Animator.StringToHash("HitTrigger");

    public event Action AttackHit;
    public event Action AttackEnded;

    private AnimationHandler _animationHandler;
    //private Animator _animator;

    private void Awake()
    {
        _animationHandler = GetComponent<AnimationHandler>();
        //_animator = GetComponent<Animator>();
    }

    public void PlayAttack()
    {
        _animationHandler.TriggerEnemyHit();
        //_animator.SetTrigger(HitTrigger);
    }

    // Animation Event
    public void DealDamage()
    {
        AttackHit?.Invoke();
    }

    public void OnAttackAnimationEnd()
    {
        AttackEnded?.Invoke();
    }
    //private static readonly int HitTrigger = Animator.StringToHash("HitTrigger");

    //private Animator _animator;
    //private Enemy _enemy;

    //private void Awake()
    //{
    //    _animator = GetComponent<Animator>();
    //    _enemy = GetComponentInParent<Enemy>();
    //}

    //public void PlayAttack()
    //{
    //    _enemy.StartAttack();
    //    _animator.SetTrigger(HitTrigger);
    //}

    //// вызывается анимационным эвентом
    //public void OnAttackAnimationEnd()
    //{
    //    _enemy.EndAttack();
    //}
}