using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.5f;
    private float shootCooldownTimer = 0f;

    
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        HandleShooting();
        if(shootCooldownTimer > 0f)
            shootCooldownTimer -= Time.deltaTime;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && shootCooldownTimer <= 0f)
        {
            Shoot();
            shootCooldownTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Plane playerPlane = new(Vector3.up, firePoint.position);

        if (playerPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            Vector3 direction = hitPoint - firePoint.position;
            direction.y = 0f;
            direction.Normalize();

            Vector3 spawnPos = firePoint.position + direction;
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));
            projectile.GetComponent<Projectile>().Init(direction, gameObject);
        }
    }
}
