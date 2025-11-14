using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private float radius; // 1.6
    public float Radius { get { return radius; } set { radius = value; } }
    [SerializeField] SphereCollider attackarea;
    public SphereCollider AttackArea {get { return attackarea; } set {  attackarea = value; }}

    [SerializeField] private float Damage;

    public float coolDown;
    private float coolDownCounter;
    public float CoolDownCounter => coolDownCounter;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();


    private void OnEnable()
    {
        AttackArea.radius = Radius;
        coolDownCounter = coolDown;
    }

    private void Update()
    {
        if (enemiesInRange.Count == 0)
        {
            coolDownCounter = coolDown; // esto deberia cambiarse por movimiento, pero todavia no hay combate
            return; 
        }
        
        coolDownCounter -= Time.deltaTime;
        if (coolDownCounter <= 0)
        {
            Attack();
            coolDownCounter = coolDown;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy) && enemy.EnemyHealth > 0)
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
            if (enemy != null && enemy.EnemyHealth > 0)
            {
                enemy.TakeDamage(Damage);
            }
        } 
    }
}
