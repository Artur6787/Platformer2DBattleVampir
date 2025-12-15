using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidbody2d;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 newPosition = Vector2.MoveTowards(_rigidbody2d.position, targetPosition, _speed * Time.fixedDeltaTime);
        _rigidbody2d.MovePosition(newPosition);
    }
}