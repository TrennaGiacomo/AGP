using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private int roomCount = 5;
    [SerializeField] private float roomSpacing = 10f;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private MinimapManager minimapManager;
    [SerializeField] private Transform dungeonRoot;

    private IRoomGenerator generator;

    private void Start()
    {
        if (dungeonRoot == null)
            dungeonRoot = new GameObject("DungeonRoot").transform;

        generator = new LinearRoomGenerator(roomPrefab, roomSpacing, dungeonRoot);
        generator.GenerateDungeon(roomCount);

        Dictionary<Vector2Int, Room> placedRooms = (generator as LinearRoomGenerator).PlacedRooms;
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
