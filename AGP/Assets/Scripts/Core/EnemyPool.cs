using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;
    public List<GameObject> enemyPrefabs;
    public int poolSize = 20;

    private readonly Queue<GameObject> enemyPool = new();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            var enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            GameObject enemy = Instantiate(enemyToSpawn, transform);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
            
            var ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
                ai.SetRegistry(EnemyManager.Instance);

        }
    }

    public GameObject GetEnemy(Vector3 position, Transform parent)
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.transform.position = position;
            enemy.transform.SetParent(parent);
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            return null;
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
