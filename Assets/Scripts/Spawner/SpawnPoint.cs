using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Color _gizmoColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}