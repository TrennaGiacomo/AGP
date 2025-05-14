using TMPro;
using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private KeyCode menuKey = KeyCode.Escape;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private TextMeshProUGUI endGameText;

    void Start()
    {
        SceneManagerPersistent.Instance.onGameEnded.AddListener(ActivateEndGameScreen);
    }

    void Update()
    {
        if(Input.GetKeyDown(menuKey))
        {
            if(!pauseMenu.activeSelf) OpenMenu();
            else CloseMenu();
        }
    }

    private void OpenMenu() 
    {
        pauseMenu.SetActive(true);
        SceneManagerPersistent.Instance.onPauseToggled.Invoke(true);
    }

    private void CloseMenu()
    {
        pauseMenu.SetActive(false);
        SceneManagerPersistent.Instance.onPauseToggled.Invoke(false);
    }

    private void ActivateEndGameScreen(bool win)
    {
        endGameScreen.SetActive(true);

        if (win) endGameText.text = "VICTORY";
        else endGameText.text = "GAME OVER";
    }
}
