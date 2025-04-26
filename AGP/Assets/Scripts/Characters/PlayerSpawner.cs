using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2Int spawnRoomPosition = Vector2Int.zero;
    [SerializeField] private float roomSpacing = 20f;

    public GameObject SpawnPlayer()
    {
        Vector3 spawnPos = new(spawnRoomPosition.x * roomSpacing, 0f,spawnRoomPosition.y * roomSpacing);
        var player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        
        
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.SetTarget(player.transform);
        }

        return player;
    }
}
