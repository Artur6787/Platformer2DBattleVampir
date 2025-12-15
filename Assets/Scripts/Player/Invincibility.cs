using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Invincibility : MonoBehaviour
{
    private const string PlayerLayer = "Player";
    private const string EnemyLayer = "Enemy";

    [SerializeField] private float _protectionDuration = 2f;
    [SerializeField] private float _blinkSpeed = 0.2f;

    private bool _isProtected = false;
    private Renderer _objectRenderer;
    private WaitForSeconds _blinkWait;
    private int _playerLayerIndex;
    private int _enemyLayerIndex;
    private Coroutine _blinkCoroutine;
    private Coroutine _protectionCoroutine;

    private void Awake()
    {
        _objectRenderer = GetComponentInChildren<Renderer>();
        _blinkWait = new WaitForSeconds(_blinkSpeed);
        CacheLayerIndices();
    }

    public void MakeProtected()
    {
        if (_isProtected)
            return;

        _isProtected = true;
        SetLayerCollision(true);

        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);

        if (_protectionCoroutine != null)
            StopCoroutine(_protectionCoroutine);

        _blinkCoroutine = StartCoroutine(Blinking());
        _protectionCoroutine = StartCoroutine(ProtectionTimer());
    }

    public bool IsProtected()
    {
        return _isProtected;
    }

    private void CacheLayerIndices()
    {
        _playerLayerIndex = LayerMask.NameToLayer(PlayerLayer);
        _enemyLayerIndex = LayerMask.NameToLayer(EnemyLayer);
    }

    private void SetLayerCollision(bool ignore)
    {
        Physics2D.IgnoreLayerCollision(_playerLayerIndex, _enemyLayerIndex, ignore);
    }

    private IEnumerator ProtectionTimer()
    {
        yield return new WaitForSeconds(_protectionDuration);
        DisableProtection();
    }

    private IEnumerator Blinking()
    {
        while (_isProtected)
        {
            _objectRenderer.enabled = !_objectRenderer.enabled;
            yield return _blinkWait;
        }

        _objectRenderer.enabled = true;
    }

    private void DisableProtection()
    {
        _isProtected = false;
        SetLayerCollision(false);
    }
}