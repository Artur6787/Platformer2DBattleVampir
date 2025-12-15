using UnityEngine;
using UnityEngine.UI;

public class VampirismBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void SetStateReady()
    {
        _slider.value = 1f;
    }

    public void SetStateActive(float duration)
    {
        _slider.maxValue = duration;
        _slider.value = duration;
    }

    public void UpdateActive(float timeLeft)
    {
        _slider.value = Mathf.Clamp(timeLeft, 0f, _slider.maxValue);
    }

    public void SetStateCooldown(float cooldown)
    {
        _slider.maxValue = cooldown;
        _slider.value = 0f;
    }

    public void UpdateCooldown(float timeLeft)
    {
        float elapsed = _slider.maxValue - Mathf.Clamp(timeLeft, 0f, _slider.maxValue);
        _slider.value = elapsed;
    }
}