using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public string type;
    public int speed;
    public int rotationSpeed;
    public int damage;
    public int attackDelay;
    public int range;
}
