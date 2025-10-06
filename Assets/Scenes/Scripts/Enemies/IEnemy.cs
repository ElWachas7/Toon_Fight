using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    string Name { get; }
    float Distance { get; set; }
}
