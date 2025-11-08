using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.0f, 0);

    private Transform target;
    private Camera cam;

    public void Initialize(Transform targetTransform)
    {
        target = targetTransform;
        cam = Camera.main;
    }

    public void UpdateHealth(float current, float max)
    {
        
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Seguir al enemigo
        transform.position = target.position + offset;

        // Mirar hacia la cámara
        if (cam != null)
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
