using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int GridPosition {get;set;}
    public List<Room> ConnectedRooms {get;} = new();
    public HashSet<Vector2Int> ConnectedDirections = new HashSet<Vector2Int>();

    public void Connect(Room otherRoom)
    {
        if(!ConnectedRooms.Contains(otherRoom))
            ConnectedRooms.Add(otherRoom);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach(var dir in ConnectedDirections)
        {
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(dir.x, 0f, dir.y));
        }
    }
}
