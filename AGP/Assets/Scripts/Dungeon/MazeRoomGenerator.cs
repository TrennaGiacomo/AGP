using System.Collections.Generic;
using UnityEngine;

public class MazeRoomGenerator : IRoomGenerator
{
    private GameObject roomPrefab;
    private float spacing;
    private Transform roomParent;
    private float BranchingChance = 0.6f;
    private Dictionary<Vector2Int, Room> placedRooms = new();
    public Dictionary<Vector2Int, Room> PlacedRooms => placedRooms;
    private List<Vector2Int> openPositions = new();
    List<RoomContentSpawner> pendingSpawners = new();


    public MazeRoomGenerator(GameObject roomPrefab, float spacing, Transform roomParent)
    {
        this.roomPrefab = roomPrefab;
        this.spacing = spacing;
        this.roomParent = roomParent;
    }

    public List<Room> GenerateDungeon(int roomCount)
    {
        Vector2Int startPos = Vector2Int.zero;
        AddRoom(startPos);

        while (placedRooms.Count < roomCount && openPositions.Count > 0)
        {
            Vector2Int current = (Random.value < 0.8f) 
                ? openPositions[openPositions.Count - 1] 
                : openPositions[Random.Range(0, openPositions.Count)];

            List<Vector2Int> directions = GetShuffledDirections();
            bool addedRoom = false;

            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;

                if (placedRooms.ContainsKey(next))
                    continue;

                if (Random.value > BranchingChance)
                    continue;

                int neighborCount = 0;
                foreach (var neighborDir in GetShuffledDirections())
                {
                    Vector2Int neighbor = next + neighborDir;
                    if (placedRooms.ContainsKey(neighbor))
                        neighborCount++;
                }

                if (neighborCount > 1)
                    continue;

                AddRoom(next);
                addedRoom = true;

                if (placedRooms.Count >= roomCount)
                    break;
            }

            if (addedRoom)
                openPositions.Remove(current);
        }

        foreach (var room in placedRooms.Values)
        {
            room.GetComponent<RoomConnector>().SetupConnections(room, placedRooms);
        }

        foreach(var spawner in pendingSpawners)
            spawner.Initialize();

        return new List<Room>(placedRooms.Values);
    }

    private void AddRoom(Vector2Int gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x * spacing, 0f, gridPos.y * spacing);
        GameObject roomGO = Object.Instantiate(roomPrefab, worldPos, Quaternion.identity, roomParent);

        Room room = roomGO.GetComponent<Room>();
        room.GridPosition = gridPos;

        var spawner = roomGO.GetComponent<RoomContentSpawner>();
        if (spawner != null) pendingSpawners.Add(spawner);


        placedRooms[gridPos] = room;
        openPositions.Add(gridPos);
    }

    private List<Vector2Int> GetShuffledDirections()
    {
        var dirs = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        for (int i = 0; i < dirs.Count; i++)
        {
            var temp = dirs[i];
            int randIndex = Random.Range(i, dirs.Count);
            dirs[i] = dirs[randIndex];
            dirs[randIndex] = temp;
        }

        return dirs;
    }
}
