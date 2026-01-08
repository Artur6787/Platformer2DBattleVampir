using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageReceiver : MonoBehaviour
{
    private Health _health;
    private Invincibility _invincibility;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _invincibility = GetComponent<Invincibility>();
    }

    public void TakeDamage(int damage)
    {
        if (_invincibility != null && _invincibility.IsProtected())
            return;

        if (damage < 0)
        {
            Debug.LogWarning($"Недопустимое значение урона: {damage}. Урон должен быть неотрицательным.", this);
            return;
        }

        _health.TakeDamage(damage);

        if (_invincibility != null)
            _invincibility.MakeProtected();
    }
}