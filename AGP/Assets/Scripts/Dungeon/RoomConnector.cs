using UnityEngine;
using System.Collections.Generic;

public class RoomConnector : MonoBehaviour
{
    [Header("Door Spawn Points")]
    [SerializeField] private Transform doorPointNorth;
    [SerializeField] private Transform doorPointSouth;
    [SerializeField] private Transform doorPointEast;
    [SerializeField] private Transform doorPointWest;

    [Header("Wall Objects (solid walls)")]
    [SerializeField] private GameObject wallNorth;
    [SerializeField] private GameObject wallSouth;
    [SerializeField] private GameObject wallEast;
    [SerializeField] private GameObject wallWest;

    [Header("Door Prefab")]
    [SerializeField] private GameObject doorPrefab;

    private Vector2Int[] directions = new[]
    {
        Vector2Int.up,     
        Vector2Int.down,   
        Vector2Int.right,  
        Vector2Int.left    
    };

    public void SetupConnections(Room room, Dictionary<Vector2Int, Room> allRooms)
    {
        Transform[] spawnPoints = { doorPointNorth, doorPointSouth, doorPointEast, doorPointWest };
        GameObject[] wallSolids = { wallNorth, wallSouth, wallEast, wallWest };

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int neighborPos = room.GridPosition + directions[i];
            bool connected = allRooms.ContainsKey(neighborPos);

            if (connected)
            {
                wallSolids[i].SetActive(false);

                
                Transform doorSpawn = spawnPoints[i];
                GameObject door = Instantiate(doorPrefab, doorSpawn.position, doorSpawn.rotation, transform);

                room.Connect(allRooms[neighborPos]);
            }
            else
            {
                wallSolids[i].SetActive(true);
            }
        }
    }
}
