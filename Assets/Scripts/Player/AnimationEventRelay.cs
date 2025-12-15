using UnityEngine;
using System;

public class AnimationEventRelay : MonoBehaviour
{
    public event Action HitEvent;
    public event Action AttackEndEvent;

    public void OnHit()
    {
        HitEvent.Invoke();
    }

    public void OnAttackEnd()
    {
        AttackEndEvent.Invoke();
    }
}