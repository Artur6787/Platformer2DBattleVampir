using UnityEngine;

[RequireComponent(typeof(Health))]
public class Death : MonoBehaviour
{
    [SerializeField] private float _destroyDelay = 0.1f;

    private Health _health;

    private void Awake()
    {
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

    private void OnDeath()
    {
        Destroy(gameObject, _destroyDelay);
    }
}