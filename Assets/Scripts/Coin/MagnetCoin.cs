using UnityEngine;

[RequireComponent(typeof(Chaser))]
public class MagnetCoin : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Chaser _chaser;

    private Transform _playerTarget;
    private bool _isLockedOn;

    private void Awake()
    {
        _chaser = GetComponent<Chaser>();
        TryCachePlayer();
    }

    private void Update()
    {
        if (_isLockedOn == false)
        {
            if (_chaser != null)
            {
                if (_chaser.HasTarget())
                {
                    _isLockedOn = true;

                    if (_playerTarget == null)
                    {
                        TryCachePlayer();
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        if (_playerTarget == null)
        {
            TryCachePlayer();
        }

        Vector2 currentPos = transform.position;
        Vector2 targetPos = _playerTarget.position;
        transform.position = Vector2.MoveTowards(currentPos, targetPos, _moveSpeed * Time.deltaTime);
    }

    private void TryCachePlayer()
    {
        Player player = FindAnyObjectByType<Player>();

        if (player != null)
        {
            _playerTarget = player.transform;
        }
    }
}