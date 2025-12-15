using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HealthSpawner : MonoBehaviour
{
    [SerializeField] private float _respawnDelay = 3f;
    [SerializeField] private HealthPickup _healthPickupPrefab;
    [SerializeField] private List<SpawnPoint> _spawnPoints = new();

    private Dictionary<HealthPickup, SpawnPoint> _healthPickupSpawnPoints = new Dictionary<HealthPickup, SpawnPoint>();

    private void Start()
    {
        ValidateSpawnPoints();
        InitialSpawn();
    }

    private void OnValidate()
    {
        if (_healthPickupPrefab == null)
            Debug.LogError("Coin Prefab не назначен!", this);

        if (_spawnPoints.Count == 0)
            Debug.LogError("Не добавлены точки спавна!", this);
    }

    private IEnumerator RespawnProcess(SpawnPoint point)
    {
        yield return new WaitForSeconds(_respawnDelay);
        SpawnHealthPickup(point);
    }

    private void ValidateSpawnPoints()
    {
        bool _hasNull = false;

        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i] == null)
            {
                Debug.LogError($"Обнаружена пустая точка спавна в списке под индексом {i}!", this);
                _hasNull = true;
            }
        }

        if (_hasNull)
        {
            Debug.LogError("Ошибка: найдены пустые точки спавна. Спавнер будет отключен.", this);
            this.enabled = false;
        }
    }

    private void InitialSpawn()
    {
        foreach (var point in _spawnPoints)
        {
            SpawnHealthPickup(point);
        }
    }

    private void SpawnHealthPickup(SpawnPoint point)
    {
        HealthPickup newHealthPickup = Instantiate(_healthPickupPrefab, point.transform.position, Quaternion.identity);
        newHealthPickup.Collected += OnCoinCollected;
        _healthPickupSpawnPoints[newHealthPickup] = point;
    }

    private void OnCoinCollected(CollectibleItem item)
    {
        if (item is HealthPickup healthPickup)
        {
            healthPickup.Collected -= OnCoinCollected;

            if (_healthPickupSpawnPoints.TryGetValue(healthPickup, out var spawnPoint))
            {
                StartCoroutine(RespawnProcess(spawnPoint));
                _healthPickupSpawnPoints.Remove(healthPickup);
            }

            Destroy(healthPickup.gameObject);
        }
    }
}