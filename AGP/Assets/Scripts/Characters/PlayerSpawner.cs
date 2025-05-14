using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2Int spawnRoomPosition = Vector2Int.zero;
    [SerializeField] private float roomSpacing = 20f;
    [SerializeField] private MinimapManager minimapManager;
 
    public GameObject SpawnPlayer()
    {
        Vector3 spawnPos = new(spawnRoomPosition.x * roomSpacing, 0f,spawnRoomPosition.y * roomSpacing);
        var player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        player.GetComponent<PlayerExploration>().SetMiniMapManager(minimapManager);
        
        
        if (Camera.main.TryGetComponent<CameraFollow>(out var camFollow))
        {
            camFollow.SetTarget(player.transform);
        }

        return player;
    }
}
