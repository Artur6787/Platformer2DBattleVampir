using UnityEngine;

public class Vampirism : MonoBehaviour
{
    [SerializeField] private float _duration = 6f;
    [SerializeField] private float _cooldown = 4f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _damagePerSecond = 10f;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private VampirismBar _uiBar;
    [SerializeField] private InputHandler _inputHandler;

    private float _timer;
    private bool _isActive;
    private bool _isOnCooldown;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
        _inputHandler = GetComponent<InputHandler>();
        _uiBar = GetComponent<VampirismBar>();
    }

    private void OnEnable()
    {
        _inputHandler.VampirCommand += OnVampirCommand;
    }

    private void OnDisable()
    {
        _inputHandler.VampirCommand -= OnVampirCommand;
    }

    private void Update()
    {
        HandleState(Time.deltaTime);

        if (_isActive)
        {
            DrainNearestEnemy(Time.deltaTime);
        }
    }

    private void OnVampirCommand()
    {
        TryActivate();
    }

    private void TryActivate()
    {
        if (_isActive == true || _isOnCooldown == true)
            return;

        _isActive = true;
        _isOnCooldown = false;
        _timer = _duration;

        if (_uiBar != null)
            _uiBar.SetStateActive(_duration);
    }

    private void HandleState(float deltaTime)
    {
        if (_isActive == true)
        {
            _timer -= deltaTime;

            if (_uiBar != null)
                _uiBar.UpdateActive(_timer);

            if (_timer <= 0f)
            {
                _isActive = false;
                _isOnCooldown = true;
                _timer = _cooldown;

                if (_uiBar != null)
                    _uiBar.SetStateCooldown(_cooldown);
            }
        }
        else if (_isOnCooldown == true)
        {
            _timer -= deltaTime;

            if (_uiBar != null)
                _uiBar.UpdateCooldown(_timer);

            if (_timer <= 0f)
            {
                _isOnCooldown = false;

                if (_uiBar != null)
                    _uiBar.SetStateReady();
            }
        }
    }

    private void DrainNearestEnemy(float deltaTime)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);

        if (hits.Length == 0)
            return;

        Enemy nearestEnemy = null;
        float bestDistSqr = float.MaxValue;
        Vector2 selfPos = transform.position;

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy == null)
                continue;

            float sqr = ((Vector2)enemy.transform.position - selfPos).sqrMagnitude;

            if (sqr < bestDistSqr)
            {
                bestDistSqr = sqr;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy == null)
            return;

        Health enemyHealth = nearestEnemy.GetComponent<Health>();

        if (enemyHealth == null)
            return;

        float damage = _damagePerSecond * deltaTime;
        enemyHealth.TakeDamage(damage);

        if (_playerHealth != null)
            _playerHealth.Heal(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}