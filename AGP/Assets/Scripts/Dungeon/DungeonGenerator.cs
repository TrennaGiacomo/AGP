using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private int roomCount = 5;
    [SerializeField] private float roomSpacing = 10f;

    private IRoomGenerator generator;

    private void Start()
    {
        generator = new LinearRoomGenerator(roomPrefab, roomSpacing);
        generator.GenerateDungeon(roomCount);
    }
}
