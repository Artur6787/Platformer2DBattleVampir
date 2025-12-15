using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private AnimationEventRelay _relay;

    private void Awake()
    {
        _relay = GetComponentInParent<AnimationEventRelay>();
    }

    public void OnAnimationHit()
    {
            _relay.OnHit();
    }

    public void OnAnimationEnd()
    {
            _relay.OnAttackEnd();
    }
}