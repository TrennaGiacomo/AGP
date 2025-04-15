using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LinearRoomGenerator : IRoomGenerator
{
    private GameObject roomPrefab;
    private float spacing;
    private readonly Vector2Int[] directions = new[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
    };

    private Dictionary<Vector2Int, Room> placedRooms = new();

    public LinearRoomGenerator(GameObject roomPrefab, float spacing)
    {
        this.roomPrefab = roomPrefab;
        this.spacing = spacing;
    }

    public List<Room> GenerateDungeon(int roomCount)
    {
        List<Room> rooms = new();
        Vector2Int currentPos = Vector2Int.zero;

        GameObject roomGO = Object.Instantiate(roomPrefab, PositionFromGrid(currentPos), Quaternion.identity);
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
            GameObject newRoomGO = Object.Instantiate(roomPrefab, PositionFromGrid(currentPos), Quaternion.identity);
            Room newRoom = newRoomGO.GetComponent<Room>();
            newRoom.GridPosition = currentPos;
            rooms.Add(newRoom);
            placedRooms[currentPos] = newRoom;
        }

        foreach (Room room in rooms)
        {
            var connector = room.GetComponent<RoomConnector>();
            if(connector != null)
                connector.SetupConnections(room, placedRooms);
        }

        return rooms;
    }

    private Vector3 PositionFromGrid(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * spacing, 0, gridPos.y * spacing);
    }

    private Vector2Int? GetAvailableDirection(Vector2Int fromPos)
    {
        var shuffledDirs = directions.OrderBy(x => Random.value).ToArray();
        foreach (var dir in shuffledDirs)
        {
            var targetPos = fromPos + dir;
            if (!placedRooms.ContainsKey(targetPos))
                return targetPos;
        }
        return null;
    }
}
