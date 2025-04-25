using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            EnemyAI[] enemies = GetComponentsInChildren<EnemyAI>();

            foreach(EnemyAI enemy in enemies)
            {
                enemy.Activate(other.transform);
            }
        }
    }
}
