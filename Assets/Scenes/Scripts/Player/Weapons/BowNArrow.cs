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

    private void OnEnable()
    {
        AttackArea.radius = Radius;
    }
    public void Attack() 
    {
        Debug.Log("Shooting an arrow");
    }
}
