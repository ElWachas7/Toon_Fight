using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_hitDetection : MonoBehaviour
{
    [SerializeField] public Transform shootingPoint;
    [SerializeField] private StonePool stonePool;
    [SerializeField] private TowerManager towerManager;
    private float coolDownCounter = 0;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();

    public void OnEnable()
    {
        if (stonePool == null)
            stonePool = FindObjectOfType<StonePool>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("entró " + other.name);
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null && enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }
    private void Update()
    {
        //Una linea re falopa de chatgpt
        enemiesInRange.RemoveAll(enemy => enemy == null || !((MonoBehaviour)enemy).gameObject.activeInHierarchy || enemy.EnemyHealth <= 0);
        //aca tengo que modiciar este scritp, solo obtiene el enemigo en rango y los guarda en una lista
        //deberia hacer un proceso de eleccion con la distancia y puntos recorridos de cada enemigo
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
        coolDown(objetivo);
    }
    private void Atacar(IEnemy enemigo)
    {
        Stone stone = stonePool.GetStone();
        stone.transform.position = shootingPoint.position;
        stone.Shoot(enemigo, towerManager.StoneData.damage, towerManager.StoneData.projectileSpeed);
    }
    private void coolDown(IEnemy objetivo)
    {
        if (objetivo != null)
        {
            coolDownCounter += Time.deltaTime;
            if (coolDownCounter >= towerManager.StoneData.cooldown)
            {
                Atacar(objetivo);
                coolDownCounter = 0;
            }
        }
        else
        {
            coolDownCounter = 0;
        }
    }

}
