using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationHandler : MonoBehaviour
{
    private static readonly int s_isJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int s_isRunningHash = Animator.StringToHash("isRunning");
    private static readonly int s_attackTriggerHash = Animator.StringToHash("attackTrigger");
    private static readonly int s_comboAttackTriggerHash = Animator.StringToHash("comboAttackTrigger");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateJumpState(bool isJumping)
    {
        if (_animator == null) return;

        _animator.SetBool(s_isJumpingHash, isJumping);
    }

    public void UpdateRunState(bool isRunning)
    {
        _animator.SetBool(s_isRunningHash, isRunning);
    }

    public void TriggerAttack()
    {
        _animator.SetTrigger(s_attackTriggerHash);
    }

    public void TriggerComboAttack()
    {
        _animator.SetTrigger(s_comboAttackTriggerHash);
    }
}