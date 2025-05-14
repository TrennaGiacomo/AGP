using UnityEngine;

public interface IEnemyRegistry
{
    void RegisterEnemy(EnemyAI enemy);
    void UnregisterEnemy(EnemyAI enemy);
}
