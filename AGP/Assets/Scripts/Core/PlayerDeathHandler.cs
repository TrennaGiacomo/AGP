using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour, IDeathHandler
{
    public void HandleDeath()
    {
        SceneManagerPersistent.Instance.EndGame(false);
    }
}
