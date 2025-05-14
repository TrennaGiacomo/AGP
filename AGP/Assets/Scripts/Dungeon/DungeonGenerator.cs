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
    [SerializeField] private NavMeshBaker navMeshBaker;

    private Transform dungeonRoot;
    private IRoomGenerator generator;

    public Dictionary<Vector2Int, Room> Generate()
    {
        if (dungeonRoot == null)
            dungeonRoot = new GameObject("DungeonRoot").transform;

        generator = generatorType switch
        {
            GeneratorType.Linear => new LinearRoomGenerator(roomPrefab, roomSpacing, dungeonRoot),
            GeneratorType.Maze => new MazeRoomGenerator(roomPrefab, roomSpacing, dungeonRoot),
            _ => null
        };

        var rooms = generator.GenerateDungeon(roomCount);
        navMeshBaker.BakeNavMesh();
        return generator.PlacedRooms;
    }
}