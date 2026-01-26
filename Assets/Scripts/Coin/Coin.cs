using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Coin : CollectibleItem
{
    [SerializeField] private MoverObject _mover;
    [SerializeField] private Vision _vision;

    private void Awake()
    {
        _mover = GetComponent<MoverObject>();
        _vision = GetComponent<Vision>();
    }

    private void FixedUpdate()
    {
        if (_vision.IsPlayerVisible() == false)
            return;

        Vector2 targetPosition = _vision.GetTargetPosition();
        _mover.MoveTowards(targetPosition);
    }
}