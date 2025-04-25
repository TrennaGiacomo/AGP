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
    private Dictionary<Vector2Int, Room> placedRooms;

    public void Initialize(Vector2Int gridPos, Dictionary<Vector2Int, Room> placedRooms)
    {
        this.gridPos = gridPos;
        this.placedRooms = placedRooms;

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
        int propsToSpawn = Random.Range(0, maxPropsToSpawn + 1);
        var spawnedProps = new List<GameObject>();

        for (int i = 0; i < propsToSpawn && spawnedProps.Count < propPrefabs.Length; i++)
        {
            GameObject propPrefab = GetUniqueRandomProp(spawnedProps);
            if (propPrefab == null) break;

            if (!TryGetWallSpawnPoint(out Vector3 pos, out Quaternion rot)) continue;

            Instantiate(propPrefab, pos, rot, transform);
            spawnedProps.Add(propPrefab);
        }
    }

    private GameObject GetUniqueRandomProp(List<GameObject> alreadyUsed)
    {
        List<GameObject> unused = new();

        foreach (var prop in propPrefabs)
        {
            if (!alreadyUsed.Contains(prop))
                unused.Add(prop);
        }

        if (unused.Count == 0) return null;
        return unused[Random.Range(0, unused.Count)];
    }

    private bool TryGetWallSpawnPoint(out Vector3 position, out Quaternion rotation)
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;

        List<Vector2Int> availableWalls = new();
        Room thisRoom = GetComponent<Room>();

        if (!thisRoom.ConnectedDirections.Contains(Vector2Int.left))  availableWalls.Add(Vector2Int.left);
        if (!thisRoom.ConnectedDirections.Contains(Vector2Int.right)) availableWalls.Add(Vector2Int.right);
        if (!thisRoom.ConnectedDirections.Contains(Vector2Int.up))    availableWalls.Add(Vector2Int.up);
        if (!thisRoom.ConnectedDirections.Contains(Vector2Int.down))  availableWalls.Add(Vector2Int.down);


        if (availableWalls.Count == 0)
            return false;

        Vector2Int chosen = availableWalls[Random.Range(0, availableWalls.Count)];

        float edgeOffset = spawnAreaSize.x / 2f - 0.5f;
        Vector3 offset = Vector3.zero;

        if (chosen == Vector2Int.left)
            offset = new Vector3(-edgeOffset, 0, Random.Range(-edgeOffset, edgeOffset));
        if (chosen == Vector2Int.right)
            offset = new Vector3(edgeOffset, 0, Random.Range(-edgeOffset, edgeOffset));
        if (chosen == Vector2Int.up)
            offset = new Vector3(Random.Range(-edgeOffset, edgeOffset), 0, edgeOffset);
        if (chosen == Vector2Int.down)
            offset = new Vector3(Random.Range(-edgeOffset, edgeOffset), 0, -edgeOffset);

        position = transform.position + offset;
        rotation = GetRotationFacingIntoRoom(chosen);
        return true;
    }

    private Quaternion GetRotationFacingIntoRoom(Vector2Int wallDir)
    {
        if (wallDir == Vector2Int.left)   return Quaternion.Euler(0f, 90f, 0f);
        if (wallDir == Vector2Int.right)  return Quaternion.Euler(0f, -90f, 0f);
        if (wallDir == Vector2Int.up)     return Quaternion.Euler(0f, 180f, 0f);
        if (wallDir == Vector2Int.down)   return Quaternion.Euler(0f, 0f, 0f);

        return Quaternion.identity;
    }

    private Vector3 GetRandomPointInRoom()
    {
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float z = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        return transform.position + new Vector3(x, 0f, z);
    }
}
