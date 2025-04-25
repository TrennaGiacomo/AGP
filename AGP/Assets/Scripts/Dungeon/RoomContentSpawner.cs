using System.Collections.Generic;
using UnityEngine;

public class RoomContentSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] propPrefabs;
    [SerializeField] private int maxEnemiesToSpawn = 2;
    [SerializeField] private int maxPropsToSpawn = 3;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(6f, 6f);

    private Vector2Int gridPos;
    private Dictionary<Vector2Int,Room> placedRooms;

    public void Initialize()
    {
        SpawnProps();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int enemiesToSpawn = Random.Range(0, maxEnemiesToSpawn + 1);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 pos = GetRandomPointInRoom();
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity, transform);
        }
    }

    private void SpawnProps()
    {
        Room room = GetComponent<Room>();
        if (room == null) return;

        Vector2 roomSize = GetRoomSize();
        int propsToSpawn = Random.Range(0, maxPropsToSpawn);
        List<GameObject> spawned = new();

        for (int i = 0; i < propsToSpawn; i++)
        {
            var prefab = GetUniqueRandom(propPrefabs, spawned);
            if (prefab == null) continue;

            GameObject instance = Instantiate(prefab);
            instance.transform.localScale = prefab.transform.localScale;
            instance.transform.SetParent(transform);

            var placeable = instance.GetComponent<IPlaceable>();
            if (placeable != null)
            {
                placeable.Place(room, roomSize);
            }

            spawned.Add(prefab);
        }
    }

    private GameObject GetUniqueRandom(GameObject[] options, List<GameObject> alreadyPicked)
    {
        List<GameObject> valid = new(options);
        valid.RemoveAll(p => alreadyPicked.Contains(p));
        if (valid.Count == 0) return null;
        return valid[Random.Range(0, valid.Count)];
    }

    private Vector2 GetRoomSize()
    {
        var floor = GetComponentInChildren<Renderer>();
        if (floor != null)
        {
            Bounds bounds = floor.bounds;
            return new Vector2(bounds.size.x, bounds.size.z);
        }

        return new Vector2(20f, 20f);
    }

    private Vector3 GetRandomPointInRoom()
    {
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float z = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        return transform.position + new Vector3(x, 0f, z);
    }
}
