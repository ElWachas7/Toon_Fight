using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private float radius; // 1.6
    public float Radius => radius;

    [SerializeField] private Material material; // Red
    public Material Material => material;
    public void Attack() 
    {
        Debug.Log("Using Sword");
    }
}
