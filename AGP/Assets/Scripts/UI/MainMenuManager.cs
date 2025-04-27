using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManagerPersistent.Instance.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        SceneManagerPersistent.Instance.Quit();
    }
}
