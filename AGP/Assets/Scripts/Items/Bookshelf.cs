using UnityEngine;
using System.Collections.Generic;

public class Bookshelf : MonoBehaviour, IPlaceable
{
    public bool FacingCenter => false;

    public void Place(Room room, Vector2 roomSize)
    {
        transform.SetParent(room.transform);
        transform.localRotation = Quaternion.identity;

        List<Vector2Int> availableWalls = new();

        if (!room.ConnectedDirections.Contains(Vector2Int.left) && IsWallVisible(room.wallWest))
            availableWalls.Add(Vector2Int.left);

        if (!room.ConnectedDirections.Contains(Vector2Int.right) && IsWallVisible(room.wallEast))
            availableWalls.Add(Vector2Int.right);

        if (!room.ConnectedDirections.Contains(Vector2Int.up) && IsWallVisible(room.wallNorth))
            availableWalls.Add(Vector2Int.up);

        if (!room.ConnectedDirections.Contains(Vector2Int.down) && IsWallVisible(room.wallSouth))
            availableWalls.Add(Vector2Int.down);


        Debug.Log($"[Bookshelf] Room {room.GridPosition} | Eligible walls: {availableWalls.Count}");

        if (availableWalls.Count == 0)
        {
            transform.localPosition = Vector3.zero;
            return;
        }

        Vector2Int wallDir = availableWalls[Random.Range(0, availableWalls.Count)];

        float edgeOffset = roomSize.x / 2f - .8f;
        Vector3 localPos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        if (wallDir == Vector2Int.left)
        {
            localPos = new Vector3(-edgeOffset, 0, Random.Range(-edgeOffset, edgeOffset));
            rot = Quaternion.Euler(0, 90f, 0f);
        }
        else if (wallDir == Vector2Int.right)
        {
            localPos = new Vector3(edgeOffset, 0, Random.Range(-edgeOffset, edgeOffset));
            rot = Quaternion.Euler(0, -90f, 0f);
        }
        else if (wallDir == Vector2Int.up)
        {
            localPos = new Vector3(Random.Range(-edgeOffset, edgeOffset), 0, edgeOffset);
            rot = Quaternion.Euler(0, 180f, 0f);
        }
        else if (wallDir == Vector2Int.down)
        {
            localPos = new Vector3(Random.Range(-edgeOffset, edgeOffset), 0, -edgeOffset);
            rot = Quaternion.Euler(0, 0f, 0f);
        }

        transform.localPosition = localPos;
        transform.localRotation = rot;

        Debug.Log($"[Bookshelf] Final pos: {transform.localPosition}, parent: {transform.parent.name}");
    }

    private bool IsWallVisible(GameObject wall)
    {
        if (wall == null) return false;

        var renderer = wall.GetComponent<Renderer>();
        if (renderer != null)
            return renderer.enabled && wall.activeInHierarchy;

        return wall.activeInHierarchy;
    }

}