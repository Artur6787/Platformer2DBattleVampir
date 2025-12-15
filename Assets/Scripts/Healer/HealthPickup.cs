using UnityEngine;

public class HealthPickup : CollectibleItem
{
    [SerializeField] public int HealAmount { get; private set; } = 20;

    public override void HandleCollected()
    {
        base.HandleCollected();
    }
}