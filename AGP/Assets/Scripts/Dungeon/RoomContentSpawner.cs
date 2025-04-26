using System.Collections.Generic;
using UnityEngine;

public class RoomContentSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] propPrefabs;
    [SerializeField] private int maxEnemiesToSpawn = 2;
    [SerializeField] private int maxPropsToSpawn = 3;
    [SerializeField] private Vector2 spawnAreaSize = new(6f, 6f);


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
        if (!TryGetComponent<Room>(out var room)) return;

        Vector2 roomSize = GetRoomSize();
        int propsToSpawn = Random.Range(0, maxPropsToSpawn);
        List<Vector3> occupiedPositions = new();

        for (int i = 0; i < propsToSpawn; i++)
        {
            var prefab = GetUniqueRandom(propPrefabs);
            if (prefab == null) continue;

            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(transform);

            if (instance.TryGetComponent<IPlaceable>(out var placeable))
            {
                bool success = TryPlaceWithoutOverlap(placeable, room, roomSize, occupiedPositions);

                if (!success)
                {
                    Destroy(instance);
                }
            }
        }
    }

    private bool TryPlaceWithoutOverlap(IPlaceable placeable, Room room, Vector2 roomSize, List<Vector3> occupiedPositions)
    {
        const int MAXATTEMPTS = 10;
        const float MINSEPARATION = 1.5f;

        for (int attempt = 0; attempt < MAXATTEMPTS; attempt++)
        {
            placeable.Place(room, roomSize);

            Vector3 newPos = placeable.GetPosition();

            bool overlaps = false;
            foreach(var pos in occupiedPositions)
            {
                if(Vector3.Distance(newPos, pos) < MINSEPARATION)
                {
                    overlaps = true;
                    break;
                }
            }    

            if(!overlaps)
            {
                occupiedPositions.Add(newPos);
                return true;
            }
        }
        return false;
    }

    private GameObject GetUniqueRandom(GameObject[] options)
    {
        if (options == null || options.Length == 0)
            return null;

        return options[Random.Range(0, options.Length)];
    }

    private Vector2 GetRoomSize()
    {
        var floor = GetComponentInChildren<Renderer>();
        if (floor != null)
        {
            Bounds bounds = floor.bounds;
            return new Vector2(bounds.size.x, bounds.size.z);
        }

        return new Vector2(10f, 10f);
    }

    private Vector3 GetRandomPointInRoom()
    {
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float z = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        return transform.position + new Vector3(x, 0f, z);
    }
}
