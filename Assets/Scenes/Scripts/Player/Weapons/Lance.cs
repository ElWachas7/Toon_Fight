using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour , IWeapon
{
    [SerializeField] private float radius; //2.4
    public float Radius { get { return radius; } set { radius = value; } }
    [SerializeField] SphereCollider attackarea;
    public SphereCollider AttackArea { get { return attackarea; } set { attackarea = value; } }

    public float coolDown = 1.5f;
    private float coolDownCounter;
    public float CoolDownCounter => coolDownCounter;

    private void OnEnable()
    {
        AttackArea.radius = Radius;
    }
    public void Attack()
    {
        Debug.Log("USsing lance");
    }
}
