using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour , IWeapon
{
    [SerializeField] private float radius; //2.4
    public float Radius => radius;
    [SerializeField] private Material material; // Blue
    public Material Material => material;
    public void Attack()
    {
        Debug.Log("USsing lance");
    }
}
