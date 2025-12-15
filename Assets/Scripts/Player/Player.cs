using UnityEngine;

[RequireComponent(typeof(DamageReceiver))]
public class Player : MonoBehaviour
{
    [SerializeField] private Health _health;

    private DamageReceiver _damageReceiver;

    private void Awake()
    {
        _damageReceiver = GetComponent<DamageReceiver>();
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Died += OnDeath;
    }

    private void OnDisable()
    {
        _health.Died -= OnDeath;
    }

    public void TakeDamage(int damage)
    {
        _damageReceiver.TakeDamage(damage);
    }

    private void OnDeath()
    {
        Debug.Log("Игрок погиб!");
    }
}