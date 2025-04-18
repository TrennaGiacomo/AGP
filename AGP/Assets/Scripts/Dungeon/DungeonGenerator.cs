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
    [SerializeField] private int roomCount = 5;
    [SerializeField] private float roomSpacing = 10f;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private MinimapManager minimapManager;
    
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

        Dictionary<Vector2Int, Room> placedRooms = generator.PlacedRooms;
        minimapManager.Init(placedRooms);

        var player = playerSpawner.SpawnPlayer();
        
        StartCoroutine(UpdateMinimapLoop(player));
    }

    private IEnumerator UpdateMinimapLoop(GameObject player)
    {
        while (true)
        {
            Vector3 playerPos = player.transform.position;

            Vector2Int gridPos = new Vector2Int
            (
                Mathf.RoundToInt(playerPos.x / roomSpacing),
                Mathf.RoundToInt(playerPos.z / roomSpacing)
            );

            minimapManager.UpdatePlayerPosition(gridPos);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
