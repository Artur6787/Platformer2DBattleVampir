using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(InputHandler))]
public class Vampirism : MonoBehaviour
{
    [SerializeField] private float _duration = 6f;
    [SerializeField] private float _cooldown = 4f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _damagePerSecond = 10f;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private InputHandler _inputHandler;

    private bool _isActive;
    private bool _isOnCooldown;

    public event Action Activated;
    public event Action Deactivated;

    public float Radius => _radius;

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
        if (_isActive)
        {
            return;
        }

        if (_isOnCooldown)
        {
            return;
        }

        StartCoroutine(VampirismRoutine());
    }

    private void Activate()
    {
        _isActive = true;
        _isOnCooldown = false;
        Activated?.Invoke();
    }

    private void Deactivate()
    {
        _isActive = false;
        _isOnCooldown = true;
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
            timer -= Time.deltaTime;
            yield return null;
        }

        Deactivate();
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        float timer = _cooldown;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        _isOnCooldown = false;
    }

    private void ProcessDrain(float deltaTime)
    {
        Enemy enemy = FindNearestEnemy();

        if (enemy == null)
        {
            return;
        }

        bool healthOfEnemy = enemy.TryGetComponent(out Health enemyHealth);

        if (healthOfEnemy == false)
        {
            return;
        }

        ApplyDrain(enemyHealth, deltaTime);
    }

    private Enemy FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);

        if (hits.Length == 0)
        {
            return null;
        }

        Enemy nearestEnemy = null;
        float bestDist = float.MaxValue;
        Vector2 selfPos = transform.position;

        foreach (Collider2D hit in hits)
        {
            bool hasEnemy = hit.TryGetComponent(out Enemy enemy);

            if (hasEnemy == false)
            {
                continue;
            }

            float dist = ((Vector2)enemy.transform.position - selfPos).sqrMagnitude;

            if (dist < bestDist)
            {
                bestDist = dist;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void ApplyDrain(Health enemyHealth, float deltaTime)
    {
        float damage = _damagePerSecond * deltaTime;
        enemyHealth.TakeDamage(damage);
        _playerHealth.Heal(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}