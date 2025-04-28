using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LinearRoomGenerator : IRoomGenerator
{
    private readonly GameObject roomPrefab;
    private readonly float spacing;
    private readonly Transform roomParent;
    private readonly Vector2Int[] directions = new[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
    };

    readonly Dictionary<Vector2Int, Room> placedRooms = new();
    public Dictionary<Vector2Int, Room> PlacedRooms => placedRooms;
    readonly List<RoomContentSpawner> roomSpawners = new();


    public LinearRoomGenerator(GameObject roomPrefab, float spacing, Transform roomParent)
    {
        this.roomPrefab = roomPrefab;
        this.spacing = spacing;
        this.roomParent = roomParent;
    }

    public List<Room> GenerateDungeon(int roomCount)
    {
        List<Room> rooms = new();
        Vector2Int currentPos = Vector2Int.zero;

        GameObject roomGO = Object.Instantiate(roomPrefab, PositionFromGrid(currentPos), Quaternion.identity,roomParent);
        Room firstRoom = roomGO.GetComponent<Room>();
        firstRoom.GridPosition = currentPos;
        rooms.Add(firstRoom);
        placedRooms[currentPos] = firstRoom;

        for (int i = 1; i < roomCount; i++)
        {
            Vector2Int? nextPos = GetAvailableDirection(currentPos);
            if (nextPos == null)
            {
                currentPos = rooms[Random.Range(0, rooms.Count)].GridPosition;
                i--;
                continue;
            }

            currentPos = nextPos.Value;
            GameObject newRoomGO = Object.Instantiate(roomPrefab, PositionFromGrid(currentPos), Quaternion.identity, roomParent);
            Room newRoom = newRoomGO.GetComponent<Room>();
            newRoom.GridPosition = currentPos;

            roomSpawners.Add(newRoomGO.GetComponent<RoomContentSpawner>());
            
            rooms.Add(newRoom);
            placedRooms[currentPos] = newRoom;
        }

        foreach (Room room in rooms)
            if(room.TryGetComponent<RoomConnector>(out var connector))
                connector.SetupConnections(room, placedRooms);
        

        foreach (RoomContentSpawner spawner in roomSpawners)
            spawner?.Initialize();
        
        return rooms;
    }

    private Vector3 PositionFromGrid(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * spacing, 0, gridPos.y * spacing);
    }

    private Vector2Int? GetAvailableDirection(Vector2Int fromPos)
    {
        var biasedDirs = new List<Vector2Int>
        {
            Vector2Int.right,  
            Vector2Int.right, 
            Vector2Int.right,
            Vector2Int.up,     
            Vector2Int.left,   
            Vector2Int.down    
        };

        biasedDirs = biasedDirs.OrderBy(x => Random.value).ToList();

        foreach (var dir in biasedDirs)
        {
            Vector2Int nextPos = fromPos + dir;
            if (!placedRooms.ContainsKey(nextPos))
                return nextPos;
        }

        return null;
    }
}
