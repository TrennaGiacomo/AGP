using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;
    public List<GameObject> enemyPrefabs;
    public int poolSize = 20;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            var enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            GameObject enemy = Instantiate(enemyToSpawn, transform);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
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
