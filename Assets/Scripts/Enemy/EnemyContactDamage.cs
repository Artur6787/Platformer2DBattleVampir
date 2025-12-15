using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(damageAmount);
        }
    }
}