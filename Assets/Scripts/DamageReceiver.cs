using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageReceiver : MonoBehaviour
{
    //[SerializeField] private Invincibility _invincibility;
    [SerializeField] private Invincibility _invincibility;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
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

        //float actualDamage = _health.TakeDamage(damage);

        //if (actualDamage > 0 && _invincibility != null)
        //    _invincibility.MakeProtected();
        _health.TakeDamage(damage);

        if (_invincibility != null)
            _invincibility.MakeProtected();
    }
}