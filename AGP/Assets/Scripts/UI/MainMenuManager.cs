using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManagerPersistent.Instance.onSceneChangeRequest.Invoke("GameScene");
    }

    public void QuitGame()
    {
        SceneManagerPersistent.Instance.onQuitRequest.Invoke();
    }
}
