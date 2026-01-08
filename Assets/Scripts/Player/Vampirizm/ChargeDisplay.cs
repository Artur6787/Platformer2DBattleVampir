using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Recharge))]
public class ChargeDisplay : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _source;
    [SerializeField] private Slider _slider;

    private Recharge _recharge;
    private IFillable _fillable;

    private void Awake()
    {
        _recharge = GetComponent<Recharge>();
        _fillable = _source as IFillable;
    }

    private void OnEnable()
    {
            _fillable.ValueChanged += OnChargeChanged;
    }

    private void OnDisable()
    {
            _fillable.ValueChanged -= OnChargeChanged;
    }

    private void Start()
    {
        _slider.value = 1f;
    }

    private void OnChargeChanged(float value)
    {
        _recharge.SetValue(value);
    }
}