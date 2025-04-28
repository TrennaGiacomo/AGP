using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public enum GeneratorType
{
    Linear,
    Maze
}
public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GeneratorType generatorType = GeneratorType.Linear;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject winTriggerPrefab;
    [SerializeField] private int roomCount = 5;
    [SerializeField] private float roomSpacing = 10f;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private MinimapManager minimapManager;
    [SerializeField] private NavMeshBaker navMeshBaker;
    
    private Transform dungeonRoot;
    private IRoomGenerator generator;

    private void Start()
    {
        if (dungeonRoot == null)
            dungeonRoot = new GameObject("DungeonRoot").transform;

        switch (generatorType)
        {
            case GeneratorType.Linear:
                generator = new LinearRoomGenerator(roomPrefab, roomSpacing, dungeonRoot);
                break;

            case GeneratorType.Maze:
                generator = new MazeRoomGenerator(roomPrefab, roomSpacing, dungeonRoot);
                break;
        }

        generator.GenerateDungeon(roomCount);
        navMeshBaker.BakeNavMesh();    

        Dictionary<Vector2Int, Room> placedRooms = generator.PlacedRooms;
        minimapManager.Init(placedRooms);

        PlaceWinTrigger(placedRooms, winTriggerPrefab, 5);

        var player = playerSpawner.SpawnPlayer();
        StartCoroutine(UpdateMinimapLoop(player));
    }

    private IEnumerator UpdateMinimapLoop(GameObject player)
    {
        while (true)
        {
            Vector3 playerPos = player.transform.position;

            Vector2Int gridPos = new(
                Mathf.RoundToInt(playerPos.x / roomSpacing),
                Mathf.RoundToInt(playerPos.z / roomSpacing)
            );

            minimapManager.UpdatePlayerPosition(gridPos);

            yield return new WaitForSeconds(0.4f);
        }
    }

    private void PlaceWinTrigger(Dictionary<Vector2Int, Room> placedRooms, GameObject winTriggerPrefab, int minimumDistance = 5)
    {
        Vector2Int startGridPos = Vector2Int.zero;
        List<Room> eligibleRooms = new();

        foreach (var kvp in placedRooms)
        {
            Vector2Int gridPos = kvp.Key;
            Room room = kvp.Value;

            int manhattanDistance = Mathf.Abs(gridPos.x - startGridPos.x) + Mathf.Abs(gridPos.y - startGridPos.y);

            if (manhattanDistance >= minimumDistance)
            {
                eligibleRooms.Add(room);
            }
        }

        if (eligibleRooms.Count == 0)
        {
            Debug.LogWarning("No eligible rooms found to place Win Trigger!");
            return;
        }

        Room selectedRoom = eligibleRooms[Random.Range(0, eligibleRooms.Count)];

        GameObject winTriggerInstance = Object.Instantiate(winTriggerPrefab);
        winTriggerInstance.transform.SetParent(selectedRoom.transform, false);
        winTriggerInstance.transform.localPosition = Vector3.zero;
    }
}