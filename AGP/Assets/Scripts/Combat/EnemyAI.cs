using UnityEngine;
using UnityEngine.Animations;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private bool active = false;
    private Transform player;
    private Animator NPCAnimator;

    private void Start()
    {
        NPCAnimator = GetComponent<Animator>(); 
    }
    public void Activate(Transform playerTransform)
    {
        player = playerTransform;
        active = true;
        NPCAnimator.SetBool("isActive", active);
    }

    private void Update()
    {
        if(!active || player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z);

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
