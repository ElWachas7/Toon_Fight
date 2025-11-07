using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EnemyDeathState<T> : State<T>
{
    public EnemyController _user;
    public EnemyDeathState(EnemyController user)
    {
        _user = user;
    }
    public override void Enter()
    {
        
        base.Enter();
        Die();
    }
    public override void Execute()
    {

    }
    public void Die()
    {

        _user._rb.velocity = Vector3.zero; // Detiene el movimiento del enemigo
        _user.NotifyDeath(); // Avisa al GameManager

        
        
    }
    public override void Exit()
    {
       
    }
}
