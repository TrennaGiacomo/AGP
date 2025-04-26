using UnityEngine;

public interface IPlaceable
{
    void Place(Room room, Vector2 roomSize);
    Vector3 GetPosition();
}
