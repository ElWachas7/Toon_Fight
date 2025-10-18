using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    float Distance { get; set; } //determina quien esta mas cerca por ende a quien atacar (ahora mismo no funciona)
    Transform transform { get; } //expone la posicion para que la flecha sepa donde ir
    float EnemyHealth { get; set; }
    public void TakeDamage(float damage);
}
