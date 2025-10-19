using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float spawnDelay = 2f;
    public int maxEnemies = 10;
    public int enemiesCount;
    public Node spawnPoint1;
    public Node spawnPoint2;
    public Node endPoint;
    public EnemyController enemyPrefab;
    
    [SerializeField] private Transform _enemySpawn;

    private GameManager instance;

    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        CorutineController.Instance.StartCoroutine(WaitAndSpawn(spawnDelay));
    }
    


    IEnumerator WaitAndSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(enemiesCount < maxEnemies)
        {
            SpawnEnemies();
        }
    }
    private void SpawnEnemies()
    {

        Instantiate(enemyPrefab, _enemySpawn.position, Quaternion.identity);
        enemiesCount++;
        CorutineController.Instance.StartCoroutine(WaitAndSpawn(spawnDelay));
        
    }
}
