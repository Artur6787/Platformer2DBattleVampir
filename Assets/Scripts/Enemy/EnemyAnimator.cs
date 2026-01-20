using System;
using UnityEngine;

[RequireComponent(typeof(AnimationHandler))]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private AnimationHandler _animationHandler;

    public event Action AttackHit;
    public event Action AttackEnded;

    private void Awake()
    {
        _animationHandler = GetComponent<AnimationHandler>();
    }

    public void PlayAttack()
    {
        _animationHandler.TriggerEnemyHit();
    }

    public void DealDamage()
    {
        AttackHit?.Invoke();
    }

    public void OnAttackAnimationEnd()
    {
        AttackEnded?.Invoke();
    }
}