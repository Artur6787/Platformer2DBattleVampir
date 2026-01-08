using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Invincibility : MonoBehaviour
{
    private const string PlayerLayer = "Player";
    private const string EnemyLayer = "Enemy";
    private const float FullyVisibleAlpha = 1f;

    [SerializeField] private float _protectionDuration = 2f;
    [SerializeField] private float _blinkSpeed = 0.2f;
    [SerializeField, Range(0f, 1f)] private float _blinkAlpha = 0.3f;

    private bool _isProtected = false;
    private SpriteRenderer _sprite;
    private WaitForSeconds _blinkWait;
    private int _playerLayerIndex;
    private int _enemyLayerIndex;
    private Coroutine _blinkCoroutine;
    private Coroutine _protectionCoroutine;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _blinkWait = new WaitForSeconds(_blinkSpeed);
        CacheLayerIndices();
    }

    public void MakeProtected()
    {
        _isProtected = true;
        SetLayerCollision(true);
        StopAllCoroutines();
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
            SetAlpha(_blinkAlpha);
            yield return _blinkWait;
            SetAlpha(FullyVisibleAlpha);
            yield return _blinkWait;
        }

        SetAlpha(FullyVisibleAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _sprite.color;
        color.a = alpha;
        _sprite.color = color;
    }

    private void DisableProtection()
    {
        _isProtected = false;
        SetLayerCollision(false);
        SetAlpha(FullyVisibleAlpha);
    }
}