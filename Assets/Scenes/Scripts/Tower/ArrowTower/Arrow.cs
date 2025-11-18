using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Arrow : MonoBehaviour
{
    public ObjectPool<Arrow> pool;
    private int damage;
    private float speed;
    private Vector3 initialSpeed;
    private Vector3 tempPos;
    private float elapsedTime;
    private static readonly float gravity = Mathf.Abs(Physics.gravity.y);
    private Transform enemyTransform;


    public void Shoot(IEnemy enemy, int damage, float projectileSpeed)
    {
        this.damage = damage;
        this.speed = projectileSpeed;
        this.elapsedTime = 0f;
        this.enemyTransform = enemy.transform;
        float distance = Vector3.Distance(transform.position, enemyTransform.position);
        float estimatedTime = distance / speed;
        initialSpeed = CalculateInitialSpeed(enemyTransform.position, transform.position, estimatedTime);
        StartCoroutine(MoveToTarget(enemy));
    }

    private IEnumerator MoveToTarget(IEnemy enemy)
    {
        Vector3 origin = transform.position;
        while (elapsedTime < 2f)
        {
            if (enemy == null || enemyTransform == null || !enemyTransform.gameObject.activeInHierarchy)
                break;
            elapsedTime += Time.deltaTime;
            // Calculamos nueva posición según el tiempo transcurrido
            Vector3 newPosition = CalculatePositionInTime(origin, initialSpeed, elapsedTime);
            transform.position = newPosition;
            // Mirar hacia adelante en la dirección del movimiento
            transform.forward = initialSpeed + Physics.gravity * elapsedTime;

            if (Vector3.Distance(transform.position, enemyTransform.position) < 0.3f)
                break;

            yield return null;
        }

        // Cuando termina el recorrido o desaparece el enemigo
        if (enemy != null && enemyTransform != null && enemyTransform.gameObject.activeInHierarchy)
        {
            enemy.TakeDamage(damage);
        }
        pool.Release(this);
    }

    private Vector3 CalculateInitialSpeed(Vector3 destiny, Vector3 origin, float time)
    {
        Vector3 distance = destiny - origin;
        float initialSpeedX = distance.x / time;
        float initialSpeedZ = distance.z / time;
        float initialSpeedY = distance.y / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;
        return new Vector3(initialSpeedX, initialSpeedY, initialSpeedZ);
    }

    private Vector3 CalculatePositionInTime(Vector3 origin, Vector3 initialSpeed, float time)
    {
        tempPos.x = origin.x + initialSpeed.x * time;
        tempPos.y = origin.y + initialSpeed.y * time - 0.5f * gravity * (time * time);
        tempPos.z = origin.z + initialSpeed.z * time;
        return tempPos;
    }
}
