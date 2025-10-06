using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    private List<IEnemy> enemiesInRange = new List<IEnemy>();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entró " + other.name);
        if (other.CompareTag("Enemy"))
        {
            IEnemy enemy = other.GetComponent<IEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Algo salio");
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

        if (objetivo != null)
        {
            Atacar(objetivo);
        }
    }

    private void Atacar(IEnemy enemigo)
    {
        // Tu lógica de ataque: disparar, reducir vida, etc.
        Debug.Log("Atacando a " + enemigo.Name);
    }

}
