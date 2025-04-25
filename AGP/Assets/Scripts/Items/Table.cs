using UnityEngine;

public class Table : MonoBehaviour, IPlaceable
{
    [SerializeField] private float centerOffset = 4f;

    public void Place(Room room, Vector2 roomSize)
    {       
        float randomOffsetX = Random.Range(-centerOffset, centerOffset); 
        float randomOffsetZ = Random.Range(-centerOffset, centerOffset);

        Vector3 localPos = new Vector3(randomOffsetX, 0f, randomOffsetZ);

        transform.SetParent(room.transform);
        
        transform.SetLocalPositionAndRotation(localPos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
    }
}
