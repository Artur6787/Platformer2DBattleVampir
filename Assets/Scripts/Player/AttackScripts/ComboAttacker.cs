using UnityEngine;
using System.Collections;

public class ComboAttacker : MonoBehaviour
{
    [SerializeField] private Attacker _attacker;
    [SerializeField] private AnimationHandler _animationHandler;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private float _doubleClickTime = 0.25f;
    [SerializeField] private float _comboResetDelay = 0.4f;

    private int _clickCount = 0;
    private float _clickTimer = 0f;

    private Coroutine _resetCoroutine;

    private void OnEnable()
    {
        _inputHandler.AttackCommand += OnClick;
    }

    private void OnDisable()
    {
        _inputHandler.AttackCommand -= OnClick;
        StopResetCoroutine();
    }

    private void Update()
    {
        if (_clickCount > 0)
        {
            _clickTimer += Time.deltaTime;

            if (_clickTimer > _doubleClickTime)
                ResetState();
        }
    }

    private void OnClick()
    {
        _clickCount++;
        _clickTimer = 0;

        if (_clickCount == 1)
        {
            return;
        }

        if (_clickCount == 2)
        {
            PlaySecondAttack();
        }
    }

    private void PlaySecondAttack()
    {
        _animationHandler.TriggerComboAttack();
        Invoke(nameof(ResetState), _comboResetDelay);
    }

    private void StartResetCoroutine()
    {
        StopResetCoroutine();
        _resetCoroutine = StartCoroutine(ResetAfterDelay());
    }

    private void StopResetCoroutine()
    {
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
            _resetCoroutine = null;
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(_comboResetDelay);
        ResetState();
    }

    private void ResetState()
    {
        _clickCount = 0;
        _clickTimer = 0f;
        StopResetCoroutine();
    }
}