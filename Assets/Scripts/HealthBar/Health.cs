using System;
using UnityEngine;

public class Health : MonoBehaviour, IFillable
{
    [SerializeField] private float _maxHealthPoints = 100f;

    private float _currentHealthPoints;

    public event Action<float> ValueChanged;
    public event Action<float> Damaged;
    public event Action<float> Healed;
    public event Action Died;

    public float CurrentHealthPoints => _currentHealthPoints;
    public float MaxHealthPoints => _maxHealthPoints;
    public bool IsDead => _currentHealthPoints <= 0f;

    private void Awake()
    {
        _currentHealthPoints = _maxHealthPoints;
        InvokeValueChanged(_currentHealthPoints, _maxHealthPoints);
    }

    public float TakeDamage(float amount)
    {
        if (amount < 0f || IsDead || amount == 0f)
            return 0f;

        float oldHealth = _currentHealthPoints;
        _currentHealthPoints = Mathf.Clamp(_currentHealthPoints - amount, 0f, _maxHealthPoints);

        float actualDamage = oldHealth - _currentHealthPoints;

        if (actualDamage > 0f)
        {
            Damaged?.Invoke(actualDamage);
            InvokeValueChanged(_currentHealthPoints, _maxHealthPoints);

            if (IsDead)
                HandleDeath();
        }

        return actualDamage;
    }

    public void Heal(float amount)
    {
        if (amount < 0f)
            throw new ArgumentOutOfRangeException(nameof(amount), "Исцеление не должно быть отрицательным.");

        if (IsDead || amount == 0f)
            return;

        float oldHealth = _currentHealthPoints;
        _currentHealthPoints = Mathf.Clamp(_currentHealthPoints + amount, 0f, _maxHealthPoints);
        float actualHeal = _currentHealthPoints - oldHealth;

        if (actualHeal <= 0f)
            return;

        Healed?.Invoke(actualHeal);
        InvokeValueChanged(_currentHealthPoints, _maxHealthPoints);
    }

    private void InvokeValueChanged(float current, float max)
    {
        ValueChanged?.Invoke(current / max);
    }

    private void HandleDeath()
    {
        Died?.Invoke();
    }
}