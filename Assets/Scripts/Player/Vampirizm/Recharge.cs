using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Recharge : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothSpeed;

    private Coroutine _coroutine;
    private float _startValue;
    private float _targetValue;

    public void SetValue(float value)
    {
        _startValue = _slider.value;
        _targetValue = value;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(SmoothUpdate());
    }

    private IEnumerator SmoothUpdate()
    {
        float time = 0f;

        while (time < _smoothSpeed)
        {
            time += Time.deltaTime;
            float progress = time / _smoothSpeed;
            _slider.value = Mathf.Lerp(_startValue, _targetValue, progress);
            yield return null;
        }

        _slider.value = _targetValue;
        _coroutine = null;
    }
}