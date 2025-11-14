using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StonePool : MonoBehaviour
{
    [SerializeField] private Stone stonePrefab;
    ObjectPool<Stone> stonePool;

    public void Awake()
    {
        stonePool = new ObjectPool<Stone>(CreateItem, Get, Release, Delete, false, 10, 50);
    }

    public Stone CreateItem()
    {
        Stone stone = Instantiate(stonePrefab);
        stone.gameObject.SetActive(false);
        stone.pool = stonePool;
        return stone;
    }

    public void Get(Stone stone)
    {
        stone.gameObject.SetActive(true);
    }
    public void Release(Stone stone)
    {
        stone.gameObject.SetActive(false);
    }

    public void Delete(Stone stone)
    {
        Destroy(stone.gameObject);
    }

    //a function for the tower
    public Stone GetArrow()
    {
        return stonePool.Get();
    }
}
