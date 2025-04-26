using UnityEngine;

public class VisualCuller : MonoBehaviour
{
    [SerializeField] private float cullDistance = 50f;
    [SerializeField] private float checkInterval = 0.2f;

    private Transform player;
    private Renderer[] renderers;
    private float checkTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        renderers = GetComponentsInChildren<Renderer>(true);
        checkTimer = Random.Range(0f, checkInterval);
    }

    private void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            CheckDistance();
            checkTimer = checkInterval;
        }
    }

    private void CheckDistance()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool shouldBeVisible = distance <= cullDistance;

        foreach (var rend in renderers)
        {
            if (rend != null)
                rend.enabled = shouldBeVisible;
        }
    }
}
