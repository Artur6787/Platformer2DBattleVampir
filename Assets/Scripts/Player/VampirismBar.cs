using UnityEngine;

public class VampirismBar : MonoBehaviour
{
    [SerializeField] private Vampirism _vampirism;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer.enabled = false;
    }

    private void OnEnable()
    {
        _vampirism.Activated += OnActivated;
        _vampirism.Deactivated += OnDeactivated;
    }

    private void OnDisable()
    {
        _vampirism.Activated -= OnActivated;
        _vampirism.Deactivated -= OnDeactivated;
    }

    private void OnActivated()
    {
        _spriteRenderer.enabled = true;
    }

    private void OnDeactivated()
    {
        _spriteRenderer.enabled = false;
    }
}