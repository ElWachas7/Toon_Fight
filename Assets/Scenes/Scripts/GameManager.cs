using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("General Settings")]
    public float spawnDelay = 1.5f;                 // Tiempo entre spawns dentro de una oleada
    public float waveDelay = 5f;                    // Tiempo entre oleadas
    public int startingEnemies = 5;                 // Enemigos en la primera oleada
    public int maxWaves = 5;                        // Máximo de oleadas
    [Range(0.05f, 1.0f)]
    public float spawnDelayDecreasePerWave = 0.1f;  //  Cuánto se reduce el tiempo de spawn por oleada (0.1 = 10%)

    [Header("Difficulty Settings")]
    [Range(1.0f, 2.0f)]
    public float enemyGrowthRate = 1.2f;            //  Multiplicador del aumento de enemigos por ronda (1.1 = +10%)
    [Range(0f, 0.5f)]
    public float strongEnemyChanceIncrement = 0.1f; //  Aumento de la chance de enemigos fuertes por oleada (0.1 = +10%)

    [Header("Spawn & End Points")]
    [SerializeField] private Transform _enemySpawn;
    public Node spawnPoint1;
    public Node spawnPoint2;
    public Node endPoint;
    public Node endPoint2;

    [Header("Enemies")]
    public EnemyController enemyPrefab;             // Enemigo débil
    public EnemyController enemyPrefab2;            // Enemigo fuerte

    [Header("Waves")]
    private int currentWave = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool spawningWave = false;

    [Header("Economy")]
    public int Money = 0;
    public int Exp = 0;
    [SerializeField] private TextMeshProUGUI textoMoney;


    [NonSerialized]public static GameManager gameManagerSingleton;
    private void Awake()
    {
        if(gameManagerSingleton == null) 
        {
            gameManagerSingleton = this;
        }
        else if (gameManagerSingleton != this) 
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Update()
    {
        textoMoney.text = Money.ToString();
    }
    private void Start()
    {
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        while (currentWave < maxWaves)
        {
            currentWave++;

            //  Aumenta la cantidad de enemigos según el multiplicador elegido
            int enemiesThisWave = Mathf.RoundToInt(startingEnemies * Mathf.Pow(enemyGrowthRate, currentWave - 1));

            Debug.Log($"---- Iniciando Oleada {currentWave}/{maxWaves} con {enemiesThisWave} enemigos ----");

            yield return StartCoroutine(SpawnWave(enemiesThisWave));

            // Esperar hasta que todos los enemigos mueran
            yield return StartCoroutine(WaitForEnemiesToDie());

            if (currentWave < maxWaves)
            {
                Debug.Log($"Oleada {currentWave} completada. Próxima en {waveDelay} segundos...");
                yield return new WaitForSeconds(waveDelay);

                //  Reduce el tiempo entre spawns para hacer el ritmo más rápido
                spawnDelay = Mathf.Max(0.2f, spawnDelay * (1f - spawnDelayDecreasePerWave));
            }
        }

        Debug.Log("¡Todas las oleadas completadas!");
        OnAllWavesCompleted();
    }

    private IEnumerator SpawnWave(int count)
    {
        spawningWave = true;
        activeEnemies.Clear();

        for (int i = 0; i < count; i++)
        {
            EnemyController prefabToSpawn;

            //  Probabilidad de enemigos fuertes crece por oleada
            float strongChance = Mathf.Clamp01(strongEnemyChanceIncrement * (currentWave - 1));
            if (currentWave >= 2 && UnityEngine.Random.value < strongChance)
                prefabToSpawn = enemyPrefab2;
            else
                prefabToSpawn = enemyPrefab;

            EnemyController enemy = Instantiate(prefabToSpawn, _enemySpawn.position, Quaternion.identity);
            activeEnemies.Add(enemy.gameObject);

            
            
            enemy.OnEnemyDied += OnEnemyDied;

            yield return new WaitForSeconds(spawnDelay);
        }

        spawningWave = false;
    }

    private IEnumerator WaitForEnemiesToDie()
    {
        while (activeEnemies.Count > 0)
        {
            activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);
            yield return null;
        }
    }

    private void OnEnemyDied(EnemyController enemy)
    {
        if (activeEnemies.Contains(enemy.gameObject))
        {
            AddMoney(enemy);
            activeEnemies.Remove(enemy.gameObject);
        }
        
        
    }

    private void OnAllWavesCompleted()
    {
        Debug.Log(" Juego completado: todas las rondas superadas.");
    }

    private void AddMoney(EnemyController enemy)
    {
        if (enemy.type == "Goblin")
        {
            Money += 10;
        }
        else if (enemy.type == "Gordogoblin")
        {
            Money += 20;
        }
    }

    
}