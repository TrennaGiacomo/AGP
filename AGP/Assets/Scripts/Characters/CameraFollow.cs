using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Vector3 offset = new(0, 15, 0);
    [SerializeField] private bool lookDown = true;

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        if (lookDown)
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        else
            transform.LookAt(target);
        
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
