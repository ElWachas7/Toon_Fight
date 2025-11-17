using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowPool : MonoBehaviour
{
    [SerializeField] private Arrow arrowPrefab;
     ObjectPool<Arrow> arrowPool;

    public void Awake()
    {
        arrowPool = new ObjectPool<Arrow>(CreateItem, Get, Release, Delete, false, 10, 50 );
    }

    public Arrow CreateItem() 
    {
        Arrow arrow = Instantiate(arrowPrefab);
        arrow.gameObject.SetActive( false );
        arrow.pool = arrowPool;
        return arrow;
    }

    public void Get(Arrow arrow) 
    {
        arrow.gameObject.SetActive ( true );
    }
    public void Release(Arrow arrow) 
    {
        arrow.gameObject.SetActive(false);
    }

    public void Delete(Arrow arrow)
    {
        Destroy(arrow.gameObject);
    }

    //a function for the tower
    public Arrow GetArrow()
    {
        return arrowPool.Get();
    }
}
