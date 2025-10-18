using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{ 
    void Attack();
    SphereCollider AttackArea { get; set; }
    float Radius { get; set;}
    Material Material { get; }
}
