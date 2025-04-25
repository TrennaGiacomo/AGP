using UnityEngine;
using System.Collections.Generic;

public class RoomConnector : MonoBehaviour
{
    [Header("Wall Objects")]
    [SerializeField] private GameObject wallNorth;
    [SerializeField] private GameObject wallSouth;
    [SerializeField] private GameObject wallEast;
    [SerializeField] private GameObject wallWest;

    private Vector2Int[] directions = new[]
    {
        Vector2Int.up,     
        Vector2Int.down,   
        Vector2Int.right,  
        Vector2Int.left    
    };

    public void SetupConnections(Room room, Dictionary<Vector2Int, Room> allRooms)
    {
        GameObject[] wallObjects = { wallNorth, wallSouth, wallEast, wallWest };

        room.wallNorth = wallNorth;
        room.wallSouth = wallSouth;
        room.wallEast = wallEast;
        room.wallWest = wallWest;

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int dir = directions[i];
            Vector2Int neighborPos = room.GridPosition + dir;

            bool hasNeighbor = allRooms.ContainsKey(neighborPos);

            if (hasNeighbor)
            {
                wallObjects[i].SetActive(false);
                room.ConnectedDirections.Add(dir);
                room.Connect(allRooms[neighborPos]);
            }
            else
            {
                wallObjects[i].SetActive(true);
            }
        }
    }
}
