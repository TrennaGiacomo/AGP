using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider fill;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    public void SetHealth(float percent)
    {
        fill.value = percent;
    }

    private void LateUpdate()
    {
        if (cam == null) return;

        Vector3 dir = transform.position - cam.transform.position;
        dir.y = 0;  

        transform.rotation = Quaternion.LookRotation(dir);  
    }
}
