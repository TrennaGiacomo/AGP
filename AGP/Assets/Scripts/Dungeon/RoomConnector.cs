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

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int dir = directions[i];
            Vector2Int neighborPos = room.GridPosition + directions[i];

            bool hasNeighbor = allRooms.ContainsKey(neighborPos);
            wallObjects[i].SetActive(!hasNeighbor);

            if (hasNeighbor)
            {
                room.Connect(allRooms[neighborPos]);
                room.ConnectedDirections.Add(dir);
            }
        }
    }
}
