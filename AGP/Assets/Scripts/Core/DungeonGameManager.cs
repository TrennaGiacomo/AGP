using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonGameManager : MonoBehaviour
{
    [SerializeField] private DungeonGenerator generator;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private MinimapManager minimapManager;
    [SerializeField] private GameObject winTriggerPrefab;
    [SerializeField] private float roomSpacing = 10f;

    private void Start()
    {
        Dictionary<Vector2Int, Room> placedRooms = generator.Generate();

        minimapManager.Init(placedRooms);
        PlaceWinTrigger(placedRooms, winTriggerPrefab);

        var player = playerSpawner.SpawnPlayer();
        StartCoroutine(UpdateMinimapLoop(player));
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
        winTriggerInstance.SetActive(true); 
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
}
