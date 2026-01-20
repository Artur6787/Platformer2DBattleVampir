using UnityEngine;

[RequireComponent(typeof(DamageReceiver))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mover))]
public class Player : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Attacker _attacker;
    [SerializeField] private DamageReceiver _damageReceiver;

    public bool CanMove { get; private set; } = true;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _attacker = GetComponent<Attacker>();
        _damageReceiver = GetComponent<DamageReceiver>();
    }

    private void OnEnable()
    {
        _attacker.AttackStarted += OnAttackStarted;
        _attacker.AttackEnded += OnAttackEnded;
    }

    private void OnDisable()
    {
        _attacker.AttackStarted -= OnAttackStarted;
        _attacker.AttackEnded -= OnAttackEnded;
    }

    public void TakeDamage(int damage)
    {
        _damageReceiver.TakeDamage(damage);
    }

    private void OnAttackStarted()
    {
        CanMove = false;
    }

    private void OnAttackEnded()
    {
        CanMove = true;
    }
}