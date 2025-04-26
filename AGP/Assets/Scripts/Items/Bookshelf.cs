using UnityEngine;
using System.Collections.Generic;

public class Bookshelf : MonoBehaviour, IPlaceable
{
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

        if (availableWalls.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector2Int wallDir = availableWalls[Random.Range(0, availableWalls.Count)];

        float edgeOffsetX = room.transform.localScale.x * 5f - 1f;
        float edgeOffsetZ = room.transform.localScale.z * 5f - 1f;
        Vector3 localPos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        if (wallDir == Vector2Int.left)
        {
            localPos = new Vector3(-edgeOffsetX, 0, Random.Range(-edgeOffsetZ + 0.5f, edgeOffsetZ - 0.5f));
            rot = Quaternion.Euler(0, 90f, 0f);
        }
        else if (wallDir == Vector2Int.right)
        {
            localPos = new Vector3(edgeOffsetX, 0, Random.Range(-edgeOffsetZ + 0.5f, edgeOffsetZ - 0.5f));
            rot = Quaternion.Euler(0, -90f, 0f);
        }
        else if (wallDir == Vector2Int.up)
        {
            localPos = new Vector3(Random.Range(-edgeOffsetX + 0.5f, edgeOffsetX - 0.5f), 0, edgeOffsetZ);
            rot = Quaternion.Euler(0, 180f, 0f);
        }
        else if (wallDir == Vector2Int.down)
        {
            localPos = new Vector3(Random.Range(-edgeOffsetX + 0.5f, edgeOffsetX - 0.5f), 0, -edgeOffsetZ);
            rot = Quaternion.Euler(0, 0f, 0f);
        }

        transform.SetLocalPositionAndRotation(localPos, rot);
    }

    private bool IsWallVisible(GameObject wall)
    {
        if (wall == null) return false;

        if (wall.TryGetComponent<Renderer>(out var renderer))
            return renderer.enabled && wall.activeInHierarchy;

        return wall.activeInHierarchy;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}