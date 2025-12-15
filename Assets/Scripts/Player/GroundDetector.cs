using System;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private float _checkDistance = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Vector2 _offset = Vector2.zero;

    public event Action<bool> GroundedChanged;

    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        Vector2 origin = (Vector2)transform.position + _offset;
        bool wasGrounded = IsGrounded;
        IsGrounded = Physics2D.Raycast(origin, Vector2.down, _checkDistance, _groundLayer);

        if (IsGrounded != wasGrounded)
        {
            GroundedChanged?.Invoke(IsGrounded);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 origin = (Vector2)transform.position + _offset;
        Gizmos.DrawLine(origin, origin + Vector2.down * _checkDistance);
    }
}