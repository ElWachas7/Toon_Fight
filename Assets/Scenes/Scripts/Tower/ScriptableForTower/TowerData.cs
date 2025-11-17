using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData", order = 1)]
public class TowerData : ScriptableObject
{
    public int damage;
    public float projectileSpeed;
    public float range;
    public float cooldown;
    public int exp;
    public int level;
}
