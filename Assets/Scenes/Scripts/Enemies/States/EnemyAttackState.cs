using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState<T> : State<T>
{
    EnemyController _user;
    PlayerMovement _target;
    int _damage;
    float _attackDelay;

    public EnemyAttackState(EnemyController user, PlayerMovement target, int damage, float attackdelay)
    {
        _user = user;
        this._target = target;
        _damage = damage;
        _attackDelay = attackdelay;
    }

    public override void Enter()
    {
        Debug.Log("Entering Attack State");
        _user.speed = 0f; // Stop movement while attacking
        base.Enter();

        CorutineController.Instance.StartCoroutine(AttackDelay());
    }
    public override void Execute()
    {
        base.Execute();
        if (_target.currentHealth <= 0)
        {
            Debug.Log("Target is dead, exiting attack state");
            Exit();
            return;
        }

    }

    IEnumerator AttackDelay()
    {
 
        yield return new WaitForSeconds(_attackDelay); // Simulate attack delay
        Debug.Log("Attack Delay Finished");
        Attack();

    }
    public void Attack()
    {
        
        if (_target != null)
        {
            Debug.Log(_user.name + "Attacking target: " + _target.name);
            _target.GetComponent<PlayerMovement>()?.GotHit(_damage);
        }

    }
    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
        _user.speed = _user.enemyData.speed; // Reset speed after attack
        base.Exit();
    }
}
