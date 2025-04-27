using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int GridPosition;
    public List<Room> ConnectedRooms {get;} = new();
    public HashSet<Vector2Int> ConnectedDirections = new HashSet<Vector2Int>();
    [HideInInspector] public GameObject wallNorth;
    [HideInInspector] public GameObject wallSouth;
    [HideInInspector] public GameObject wallEast;
    [HideInInspector] public GameObject wallWest;

    public void Connect(Room otherRoom)
    {
        if(!ConnectedRooms.Contains(otherRoom))
            ConnectedRooms.Add(otherRoom);
    }
}
