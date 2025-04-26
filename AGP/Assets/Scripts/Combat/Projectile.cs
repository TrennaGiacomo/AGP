using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int projectileDamage = 25;

    private Vector3 direction;
    private GameObject shooter;
    public void Init(Vector3 direction, GameObject shooter)
    {
        this.direction = direction.normalized;
        this.shooter = shooter;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == shooter)
            return;
        
        if(other.TryGetComponent<Health>(out var targetHealth))
            targetHealth.TakeDamage(projectileDamage);

        Destroy(gameObject);
    }
}
