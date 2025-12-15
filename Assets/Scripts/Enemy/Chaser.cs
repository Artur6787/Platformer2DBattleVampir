using UnityEngine;

[RequireComponent(typeof(Vision))]
public class Chaser : MonoBehaviour
{
    private Vision _vision;

    private void Awake()
    {
        _vision = GetComponent<Vision>();
    }

    public bool HasTarget()
    {
        return _vision != null && _vision.IsPlayerVisible();
    }

    public Vector2 GetTargetPosition()
    {
        Vector2 position = transform.position;

        if (_vision != null)
            position = _vision.GetTargetPosition();

        return position;
    }
}