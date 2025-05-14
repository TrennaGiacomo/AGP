using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IEnemyRegistry
{
    public static EnemyManager Instance;

    private readonly List<EnemyAI> enemies = new();
    private Transform player;
    private readonly float checkInterval = 0.5f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(UpdateEnemyStates), 0f, checkInterval);
    }

    public void RegisterEnemy(EnemyAI enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyAI enemy)
    {
        enemies.Remove(enemy);
    }

    void UpdateEnemyStates()
    {
        foreach (EnemyAI enemy in enemies)
        {
            if (enemy == null) continue;
            enemy.CheckSleepState(player.position);
        }
    }
}
