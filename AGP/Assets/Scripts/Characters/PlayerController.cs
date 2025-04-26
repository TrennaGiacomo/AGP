using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Health))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool rotateTowardMovement = true;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.5f;

    private Animator playerAnimator;
    private Camera cam;
    private CharacterController controller;
    private float shootCooldownTimer = 0f;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();

        if(shootCooldownTimer > 0f)
            shootCooldownTimer -= Time.deltaTime;
        
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0, v).normalized;
        playerAnimator.SetFloat("WalkingSpeed", direction.magnitude);

        if (direction.magnitude > 0.1f)
        {
            controller.Move(moveSpeed * Time.deltaTime * direction);

            if (rotateTowardMovement)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10f * Time.deltaTime);
            }
        }
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
