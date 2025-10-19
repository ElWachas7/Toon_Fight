using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowNArrow : MonoBehaviour , IWeapon
{
    [SerializeField] private float radius; //8
    public float Radius { get { return radius; } set { radius = value; } }
    [SerializeField] private Material material; // Green
    public Material Material => material;
    [SerializeField] SphereCollider attackarea;
    public SphereCollider AttackArea { get { return attackarea; } set { attackarea = value; } }


    [SerializeField] private ArrowPool arrowPool;
    [SerializeField] private int Damage;
    [SerializeField] private float projectileSpeed;
    public float coolDown = 1.5f;
    private float coolDownCounter;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();

    private void OnEnable() // se mantiene
    {
        AttackArea.radius = Radius;
        coolDownCounter = 0f;
    }
    private void Update() // se mantiene
    {
        if (enemiesInRange.Count == 0) return;

        IEnemy objetivo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (IEnemy enemigo in enemiesInRange)
        {
            //float distancia = Vector3.Distance(transform.position, enemigo.transform.position);
            if (enemigo.Distance < menorDistancia)
            {
                menorDistancia = enemigo.Distance;
                objetivo = enemigo;
            }
        }

        Cooldown(objetivo);
    }

    private void OnTriggerEnter(Collider other) // añade enemigos a una lista
    {
        if (other.CompareTag("Enemy"))
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy) && enemy.EnemyHealth > 0)
                enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other) //elemina el enemigo de la lista si se va de rango
    {
        if (other.CompareTag("Enemy"))
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null && enemiesInRange.Contains(enemy))
                enemiesInRange.Remove(enemy);
        }
    }
    public void Attack(IEnemy enemy)
    {
        Arrow arrow = arrowPool.GetArrow();
        arrow.transform.position = transform.position;
        arrow.Shoot(enemy, Damage, projectileSpeed);
        if (enemy.EnemyHealth <= 0) 
        {
            enemiesInRange.Remove(enemy);
        }
    }

    public void Cooldown(IEnemy objetivo) 
    {
        if (coolDownCounter < coolDown)
            coolDownCounter += Time.deltaTime;

        if (coolDownCounter >= coolDown && enemiesInRange.Count > 0)
        {
            Attack(objetivo);
            coolDownCounter = 0f;
        }
    }
}
