using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IEnemy
{
    public float distance;
    public float Distance {get {return distance;} set { distance = value;}}

    [SerializeField] public float enemyhealth;
    public float EnemyHealth { get {return enemyhealth; } set { enemyhealth = value;}}

    public void TakeDamage(float damage) 
    {
        EnemyHealth -= damage;

        //if(EnemyHealth <= 0) 
        //{
        //    gameObject.SetActive(false);
        //}
    }

}
