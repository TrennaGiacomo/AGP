using UnityEngine;

public class PlayerExploration : MonoBehaviour
{
    private MinimapManager minimapManager;
    public void SetMiniMapManager(MinimapManager manager)
    {
        minimapManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Room>() is Room room)
            minimapManager.MarkRoomVisited(room.GridPosition);
        if(other.CompareTag("Win"))
            SceneManagerPersistent.Instance.EndGame(true);
    }
}
