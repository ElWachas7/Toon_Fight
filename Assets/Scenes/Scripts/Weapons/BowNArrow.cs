using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowNArrow : MonoBehaviour , IWeapon
{
    [SerializeField] private float radius; //8
    public float Radius => radius;
    [SerializeField] private Material material; // Green
    public Material Material => material;
    public void Attack() 
    {
        Debug.Log("Shooting an arrow");
    }
}
