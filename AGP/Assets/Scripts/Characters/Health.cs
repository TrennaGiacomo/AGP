using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("HealthBar Settings")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Vector3 healthBarOffset = new(0,4f,0);

    private GameObject healthBarRoot;
    private HealthBarUI healthBarUI;

    private void Start()
    {
        currentHealth = maxHealth;

        if(healthBarPrefab != null)
        {
            var bar = Instantiate(healthBarPrefab, transform);
            bar.transform.localPosition = healthBarOffset;
            healthBarRoot = bar;
            healthBarUI = bar.GetComponentInChildren<HealthBarUI>();

            healthBarRoot.SetActive(false);
        }
    }

    public void TakeDamage(int amount)
    {
        if(transform.GetComponent<EnemyAI>())
        {
            var enemy = transform.GetComponent<EnemyAI>();
            enemy.Activate();
        }

        currentHealth -= amount;

        if(healthBarRoot != null && !healthBarRoot.activeSelf)
            healthBarRoot.SetActive(true);

        UpdateHealthBar();

        if(currentHealth <= 0)
            Die();
    }

    private void UpdateHealthBar() 
    {
        if(healthBarUI != null)
        {
            float percent = (float) currentHealth / maxHealth;
            healthBarUI.SetHealth(percent);
        }
    }

    private void Die()
    {
        if(healthBarUI != null)
            Destroy(healthBarUI.gameObject);

        Destroy(gameObject);
    }
}
