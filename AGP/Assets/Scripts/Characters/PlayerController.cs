using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Health))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turningSpeed = 10f;
    [SerializeField] private bool rotateTowardMovement = true;

    private Animator playerAnimator;
    private CharacterController controller;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        
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
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turningSpeed * Time.deltaTime);
            }
        }
    }
}
