using System.Collections.Generic;
using UnityEngine;

public interface IRoomGenerator
{
    List<Room> GenerateDungeon(int roomCount);
    Dictionary<Vector2Int, Room> PlacedRooms {get;}
}
