using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private float radius; // 1.6
    public float Radius { get { return radius; } set { radius = value; } }

    [SerializeField] private Material material; // Red
    public Material Material => material;

    [SerializeField] SphereCollider attackarea;
    public SphereCollider AttackArea {get { return attackarea; } set {  attackarea = value; }}

    [SerializeField] private float Damage;

    public float coolDown = 1.5f;
    private float coolDownCounter;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();


    private void OnEnable()
    {
        AttackArea.radius = Radius;
        coolDownCounter = 0f;
    }

    private void Update()
    {
        if (coolDownCounter < coolDown)
            coolDownCounter += Time.deltaTime;

        if (coolDownCounter >= coolDown && enemiesInRange.Count > 0)
        {
            Attack();
            coolDownCounter = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null && enemiesInRange.Contains(enemy))
                enemiesInRange.Remove(enemy);
        }
    }
    public void Attack() 
    {
        foreach (IEnemy enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }
        }
        
    }
}
