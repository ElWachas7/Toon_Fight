using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IEnemy
{
    [Header("Stats")]
    [SerializeField] private float maxHealth;
    

    [Header("UI")]
    public HealthBarUI healthBarPrefab;
    public HealthBarUI healthBarInstance;

    public float distance;
    public float Distance {get {return distance;} set { distance = value;}}

    [SerializeField] public float enemyhealth;
    public float EnemyHealth { get {return enemyhealth; } set { enemyhealth = value;}}

    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }
    private void Start()
    {
        maxHealth = enemyhealth;

        // Instanciar barra de vida en el mundo
        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBarInstance.Initialize(transform);
            healthBarInstance.UpdateHealth(enemyhealth, maxHealth);
        }
    }
    public void TakeDamage(float damage) 
    {
        EnemyHealth -= damage;
        EnemyHealth = Mathf.Max(EnemyHealth, 0);

        

        if (healthBarInstance != null)
            healthBarInstance.UpdateHealth(enemyhealth, maxHealth);

    }

}
