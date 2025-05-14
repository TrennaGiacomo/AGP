using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class SceneManagerPersistent : MonoBehaviour
{
    public static SceneManagerPersistent Instance;

    public UnityEvent<string> onSceneChangeRequest = new UnityEvent<string>();
    public UnityEvent onQuitRequest = new UnityEvent();
    public UnityEvent<bool> onPauseToggled = new UnityEvent<bool>();
    public UnityEvent<bool> onGameEnded;
    private bool isPaused = false;
    private NavMeshAgent[] allAgents;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            onSceneChangeRequest.AddListener(LoadScene);
            onQuitRequest.AddListener(Quit);
            onPauseToggled.AddListener(SetPause);
        }
        else Destroy(gameObject);
    }

    public void LoadScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);  
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = isPaused ? 0f : 1f;

        if(isPaused) PauseAgents();
        else ResumeAgents();
    }

    private void PauseAgents()
    {
        allAgents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);
        foreach(var agent in allAgents)
            if(agent != null && agent.isOnNavMesh) agent.isStopped = true;
    }

    private void ResumeAgents()
    {
        if(allAgents == null) return;
        foreach(var agent in allAgents)
            if(agent != null && agent.isOnNavMesh) agent.isStopped = false;
    }

    public void EndGame(bool win)
    {
        onGameEnded.Invoke(win);
    }
}
