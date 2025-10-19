using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyReachedBase<T> : State<T>
{
    public EnemyController _user;
    public EnemyReachedBase(EnemyController user)
    {
        _user = user;
    }
    public override void Enter()
    {
        //Debug.Log("Entering Reached Base State");
        base.Enter();
    }
    public override void Execute()
    {
        base.Execute();
        ReachBase();
    }
    public void ReachBase()
    {
        //Debug.Log("NPC has reached the base.");
        // Implement logic for when the enemy reaches the base
        _user.gameObject.SetActive(false);
    }
    public override void Exit()
    {
        Debug.Log("Exiting Reached Base State");
    }
}
