using System.Buffers.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.0f, 0);

    private float baseY;

    private Transform target;
    private Camera cam;
    private string enemyType;
    public SpriteRenderer enemySprite;
    public SpriteRenderer shadow;

    private bool isFlashing = false;

    public void Initialize(Transform targetTransform, string type)
    {
        target = targetTransform;
        cam = Camera.main;
        enemyType = type;
        baseY = target.transform.position.y;
    }

    public void UpdateHealth(float current, float max)
    {
        
        fillImage.fillAmount = Mathf.Clamp01(current / max);
        RecivedDamage();
    }
    public void RecivedDamage()
    {
        if (!isFlashing)
            StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        isFlashing = true;

        Color originalEnemyColor = enemySprite.color;
        

      
        enemySprite.color = Color.red;

        yield return new WaitForSeconds(0.5f);

      
        enemySprite.color = originalEnemyColor;

        isFlashing = false;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dynamicOffset = offset;
        if (enemyType == "FlyGob")
        {
            float amplitude = 2f;
            float frequency = 2f;

            float hoverOffset = Mathf.Abs(Mathf.Sin(Time.time * frequency)) * amplitude;
            dynamicOffset.y += hoverOffset + 2;
            shadow.transform.position = new Vector3(target.position.x, baseY, target.position.z);
        }
        // Seguir al enemigo
        transform.position = target.position + dynamicOffset;

        // Mirar hacia la cámara
        if (cam != null)
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        
        
    }
}
