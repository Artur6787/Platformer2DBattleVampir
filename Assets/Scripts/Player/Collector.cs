using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CollectibleItem>(out var collectible))
        {
            switch (collectible)
            {
                case HealthPickup healthPickup:

                    if (TryGetComponent<Health>(out var health))
                    {
                        health.Heal(healthPickup.HealAmount);
                        healthPickup.HandleCollected();
                    }
                    break;

                case Coin coin:
                    coin.HandleCollected();
                    break;

                default:
                    collectible.HandleCollected();
                    break;
            }
        }
    }
}