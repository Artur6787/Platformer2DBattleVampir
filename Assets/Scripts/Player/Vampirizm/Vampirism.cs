using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(InputHandler))]
public class Vampirism : MonoBehaviour, IFillable
{
    [SerializeField] private float _duration = 6f;
    [SerializeField] private float _cooldown = 4f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _damagePerSecond = 10f;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private InputHandler _inputHandler;

    private bool _isActive;
    private bool _isOnCooldown;
    private float _currentFill = 1f;

    public event Action Activated;
    public event Action Deactivated;
    public event Action<float> ValueChanged;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
        _inputHandler = GetComponent<InputHandler>();
    }

    private void OnEnable()
    {
        _inputHandler.VampirCommand += OnVampirCommand;
    }

    private void OnDisable()
    {
        _inputHandler.VampirCommand -= OnVampirCommand;
    }

    private void OnVampirCommand()
    {
        if (_isActive == true || _isOnCooldown == true)
        {
            return;
        }

        if (_playerHealth.CurrentHealthPoints >= _playerHealth.MaxHealthPoints)
        {
            return;
        }

        StartCoroutine(VampirismRoutine());
    }

    private void Activate()
    {
        _isActive = true;
        Activated?.Invoke();
    }

    private void Deactivate()
    {
        _isActive = false;
        Deactivated?.Invoke();
    }

    private IEnumerator VampirismRoutine()
    {
        Activate();
        float timer = _duration;

        while (timer > 0f)
        {
            float deltaTime = Time.deltaTime;
            ProcessDrain(deltaTime);

            if (_playerHealth.CurrentHealthPoints >= _playerHealth.MaxHealthPoints)
            {
                break;
            }

            timer -= Time.deltaTime;
            _currentFill = timer / _duration;
            ValueChanged?.Invoke(_currentFill);
            yield return null;
        }

        Deactivate();
        StartCoroutine(CooldownRoutine(_currentFill));
    }

    private IEnumerator CooldownRoutine(float startValue)
    {
        _isOnCooldown = true;
        float timer = _cooldown;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            _currentFill = Mathf.Lerp(startValue, 1f, 1f - timer / _cooldown);
            ValueChanged?.Invoke(_currentFill);
            yield return null;
        }

        _currentFill = 1f;
        ValueChanged?.Invoke(1f);
        _isOnCooldown = false;
    }

    private void ProcessDrain(float deltaTime)
    {
        Enemy enemy = FindNearestEnemy();

        if (enemy == null)
        {
            return;
        }

        if (enemy.TryGetComponent(out Health enemyHealth) == false)
        {
            return;
        }

        ApplyDrain(enemyHealth, deltaTime);
    }

    private Enemy FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        Enemy nearest = null;
        float bestDist = float.MaxValue;
        Vector2 selfPos = transform.position;

        foreach (Collider2D hit in hits)
        {
            if (!hit.TryGetComponent(out Enemy enemy))
                continue;

            float dist = ((Vector2)enemy.transform.position - selfPos).sqrMagnitude;

            if (dist < bestDist)
            {
                bestDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    private void ApplyDrain(Health enemyHealth, float deltaTime)
    {
        if (_playerHealth.CurrentHealthPoints >= _playerHealth.MaxHealthPoints)
            return;

        float wantedDamage = _damagePerSecond * deltaTime;
        float actualDamage = enemyHealth.TakeDamage(wantedDamage);

        if (actualDamage > 0f)
            _playerHealth.Heal(actualDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}