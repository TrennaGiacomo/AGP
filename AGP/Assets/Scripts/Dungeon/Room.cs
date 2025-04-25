using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int GridPosition {get;set;}
    public List<Room> ConnectedRooms {get;} = new();

    public void Connect(Room otherRoom)
    {
        if(!ConnectedRooms.Contains(otherRoom))
            ConnectedRooms.Add(otherRoom);
    }
}
