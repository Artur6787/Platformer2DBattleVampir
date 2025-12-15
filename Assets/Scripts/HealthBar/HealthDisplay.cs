using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothSpeed = 0.5f;

    private float _startValue;
    private float _targetValue = 1f;
    private Coroutine _smoothCoroutine;

    private void OnEnable()
    {
        _health.ValueChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _health.ValueChanged -= OnHealthChanged;
    }

    private void Start()
    {
        _slider.value = 1f;
    }

    private void OnHealthChanged(float value)
    {
        _startValue = _slider.value;
        _targetValue = value;

        if (_smoothCoroutine != null)
        {
            StopCoroutine(_smoothCoroutine);
        }

        _smoothCoroutine = StartCoroutine(SmoothUpdateSlider());
    }

    private IEnumerator SmoothUpdateSlider()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _smoothSpeed)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _smoothSpeed;
            _slider.value = Mathf.Lerp(_startValue, _targetValue, progress);
            yield return null;
        }

        _slider.value = _targetValue;
        _smoothCoroutine = null;
    }
}