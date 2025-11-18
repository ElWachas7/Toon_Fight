using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Waves")]
    public GameObject waveUI;
    [SerializeField] private float waveUIShowTime = 3f;
    private int currentWave = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool spawningWave = false;
    public float spawnDelay = 1.5f;
    public float waveDelay = 5f;
    public int startingEnemies = 5;
    public int maxWaves = 5;
    [Range(0.05f, 1.0f)]
    public float spawnDelayDecreasePerWave = 0.1f;

    [Header("Tower Health")]
    public float towerHealth;
    public float towerCurrentHealth;
    public Image towerHealthUI;


    [Header("Difficulty Settings")]
    [Range(1.0f, 2.0f)]
    public float enemyGrowthRate = 1.2f;
    [Range(0f, 0.5f)]
    public float strongEnemyChanceIncrement = 0.1f;

    [Header("Spawn & End Points")]
    [SerializeField] private Transform _enemySpawn;
    public Node spawnPoint1;
    public Node spawnPoint2;
    public Node endPoint;
    public Node endPoint2;

    [Header("Enemies")]
    public EnemyController goblin;
    public EnemyController gordoGoblin;
    public EnemyController flyGob;

   

    [Header("Economy")]
    public int money = 0;
    public int exp = 0;
    [SerializeField] private TextMeshProUGUI textoMoney;

    [Header("TowerData")]
    public TowerData AT;
    public TowerData ST;
    [NonSerialized] public static GameManager gameManagerSingleton;
    public void SetTowerData() //Los SO no se resetean luego de cada partida por lo que hay que volver a su estado normal
    {
        //ArrowTower
        AT.damage = 20;
        AT.projectileSpeed = 6f;
        AT.range = 10f;
        AT.cooldown = 1.2f;
        AT.level = 1;
        //StoneTower
        ST.damage = 30;
        ST.projectileSpeed = 5f;
        ST.range = 9f;
        ST.cooldown = 1.7f;
        ST.level = 1;

    }
    private void Awake()
    {
        if (gameManagerSingleton == null)
            gameManagerSingleton = this;
        else if (gameManagerSingleton != this)
        {
            Destroy(gameObject);
            return;
        }
        SetTowerData();
    }

    private void Start()
    {
        if (waveUI != null)
            waveUI.SetActive(false); 
        towerCurrentHealth = towerHealth;
        StartCoroutine(StartWaves());
    }

    private void Update()
    {
        textoMoney.text = money.ToString();
    }

    private IEnumerator StartWaves()
    {
        while (currentWave < maxWaves)
        {
            currentWave++;

            yield return StartCoroutine(ShowWaveUI());

            int enemiesThisWave = Mathf.RoundToInt(startingEnemies * Mathf.Pow(enemyGrowthRate, currentWave - 1));
            Debug.Log($"---- Iniciando Oleada {currentWave}/{maxWaves} con {enemiesThisWave} enemigos ----");

            yield return StartCoroutine(SpawnWave(enemiesThisWave));
            yield return StartCoroutine(WaitForEnemiesToDie());

            if (currentWave < maxWaves)
            {
                Debug.Log($"Oleada {currentWave} completada. Próxima en {waveDelay} segundos...");
                yield return new WaitForSeconds(waveDelay);
                spawnDelay = Mathf.Max(0.2f, spawnDelay * (1f - spawnDelayDecreasePerWave));
            }
        }

        Debug.Log("¡Todas las oleadas completadas!");
        OnAllWavesCompleted();
    }

    private IEnumerator ShowWaveUI()
    {
        if (waveUI != null)
        {
            waveUI.SetActive(true);
            yield return new WaitForSeconds(waveUIShowTime);
            waveUI.SetActive(false);
        }
    }

    private IEnumerator SpawnWave(int count)
    {
        spawningWave = true;
        activeEnemies.Clear();

        for (int i = 0; i < count; i++)
        {
            EnemyController prefabToSpawn;

            float strongChance = Mathf.Clamp01(strongEnemyChanceIncrement * (currentWave - 1));
            if (currentWave >= 2 && UnityEngine.Random.value < strongChance)
                prefabToSpawn = gordoGoblin;
            else if (currentWave >= 3 && UnityEngine.Random.value < strongChance)
                prefabToSpawn = flyGob;
            else
                prefabToSpawn = goblin;

            EnemyController enemy = Instantiate(prefabToSpawn, _enemySpawn.position, Quaternion.identity);
            activeEnemies.Add(enemy.gameObject);
            enemy.OnEnemyComplete += OnEnemyComplete;
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

    private void OnEnemyComplete(EnemyController enemy)
    {
        towerCurrentHealth -= enemy.damage;
      
        towerHealthUI.fillAmount = Mathf.Clamp01(towerCurrentHealth / towerHealth);
        if (towerCurrentHealth <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }
    private void OnAllWavesCompleted()
    {
        SceneManager.LoadScene("WinScene");
    }

    private void AddMoney(EnemyController enemy)
    {
        if (enemy.type == "Goblin")
            money += 10;
        else if (enemy.type == "Gordogoblin")
            money += 20;
        else if (enemy.type == "FlyGob")
            money += 5;
    }
}