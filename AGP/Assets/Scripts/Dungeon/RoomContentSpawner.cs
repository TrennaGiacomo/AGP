using System.Collections.Generic;
using UnityEngine;

public class RoomContentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] propPrefabs;
    [SerializeField] private int maxEnemiesToSpawn = 2;
    [SerializeField] private int maxPropsToSpawn = 3;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(6f, 6f);

    private void Start()
    {
        SpawnProps();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        var enemiesToSpawn = Random.Range(0, maxEnemiesToSpawn);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 pos = GetRandomPointInRoom();
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity, transform);
        }
    }

    private void SpawnProps()
    {
        var propsToSpawn = Random.Range(0, maxPropsToSpawn);
        var spawnedProps = new List<GameObject>();

        for (int i = 0; i < propsToSpawn; i++)
        {
            Vector3 pos = GetRandomPointInRoom();
            var randomProp = Random.Range(0, propPrefabs.Length);

            while(spawnedProps.Contains(propPrefabs[randomProp]))
                randomProp = Random.Range(0, propPrefabs.Length);

            Instantiate(propPrefabs[randomProp], pos, Quaternion.identity, transform);
            spawnedProps.Add(propPrefabs[randomProp]);
        }
    }

    private Vector3 GetRandomPointInRoom()
    {
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float z = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        return transform.position + new Vector3(x, 0f, z);
    }
}
