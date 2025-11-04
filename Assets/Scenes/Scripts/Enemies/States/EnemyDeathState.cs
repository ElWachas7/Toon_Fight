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
        Debug.Log("Entering Death State");
        base.Enter();
    }
    public override void Execute()
    {
        base.Execute();
        Die();
    }
    public void Die()
    {

        Debug.Log("NPC has died.");
        _user.NotifyDeath(); // Avisa al GameManager
        _user.gameObject.SetActive(false);

    }
    public override void Exit()
    {
        Debug.Log("Exiting Death State");
    }
}
