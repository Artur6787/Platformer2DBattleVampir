using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private CollectibleItem _prefab;
    [SerializeField] private List<SpawnPoint> _spawnPoints = new();
    [SerializeField] private bool _enableRespawn = true;
    [SerializeField] private float _respawnDelay = 3f;

    private readonly Dictionary<CollectibleItem, SpawnPoint> _items = new Dictionary<CollectibleItem, SpawnPoint>();

    private void Start()
    {
        ValidateSpawnPoints();
        InitialSpawn();
    }

    private IEnumerator RespawnAfterDelay(SpawnPoint point)
    {
        yield return new WaitForSeconds(_respawnDelay);
        Spawn(point);
    }

    private void ValidateSpawnPoints()
    {
        if (_prefab == null || _spawnPoints.Count == 0)
        {
            Debug.LogError("CollectibleSpawner: нет префаба или точек спавна!", this);
            enabled = false;
            return;
        }
    }
    private void InitialSpawn()
    {
        foreach (var point in _spawnPoints)
        {
            Spawn(point);
        }
    }

    private void Spawn(SpawnPoint point)
    {
        if (point == null)
            return;

        var item = Instantiate(_prefab, point.transform.position, Quaternion.identity);
        item.Collected += OnCollected;
        _items[item] = point;
    }

    private void OnCollected(CollectibleItem item)
    {
        item.Collected -= OnCollected;

        if (_items.TryGetValue(item, out var point))
        {
            _items.Remove(item);

            if (_enableRespawn)
                StartCoroutine(RespawnAfterDelay(point));
        }

        Destroy(item.gameObject);
    }
}