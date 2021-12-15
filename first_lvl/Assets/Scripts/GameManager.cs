using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("--- Game Variables ---")]
    // Needed variables ***************************
    // Array of enemies
    [SerializeField] List<GameObject> enemiesList;
    public List<GameObject> EnemiesList
    {
        get => enemiesList;
        set => enemiesList = value;
    }

    // Spawn point object
    [SerializeField] GameObject spawnPoint;
    public GameObject SpawnPoint
    {
        get => spawnPoint;
        set => spawnPoint = value;
    }

    // Enemy walking end point
    [SerializeField] GameObject endPoint;
    public GameObject EndPoint
    {
        get => endPoint;
        set => endPoint = value;
    }

    // Max enemies same time on screen
    [SerializeField] int maxEnemiesOnScreen;
    public int MaxEnemiesOnScreen
    {
        get => maxEnemiesOnScreen;
        set => maxEnemiesOnScreen = value;
    }

    // Total enemies on this level
    [SerializeField] int totalEnemies;
    public int TotalEnemies
    {
        get => totalEnemies;
        set => totalEnemies = value;
    }

    // Enemies count in one wave
    [SerializeField] int enemiesPerWave;
    public int EnemiesPerWave
    {
        get => enemiesPerWave;
        set => enemiesPerWave = value;
    }

    // Spawn enemy delay
    [SerializeField] float spawnDelay = .4f;
    public float SpawnDelay
    {
        get => spawnDelay;
        set => spawnDelay = value;
    }

    // Time to next wave
    [SerializeField] float nextWaveTime = 60f;
    public float NextWaveTime
    {
        get => nextWaveTime;
        set => nextWaveTime = value;
    }

    // Game pause variable
    [SerializeField] bool isPaused;
    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }

    [SerializeField] bool isGameOver;
    public bool IsGameOver
    {
        get => isGameOver;
        set => isGameOver = value;
    }

    // Enemies count in one wave
    [SerializeField] int lives;
    public int Lives
    {
        get => lives;
        set => lives = value;
    }

    List<GameObject> currentEnemiesList = new List<GameObject>();
    public List<GameObject> CurrentEnemiesList
    {
        get => currentEnemiesList;
        set => currentEnemiesList = value;
    }
    [SerializeField] int totalEnemySpawned;
    [SerializeField] int waveCount;


    [Header("--- Game Panels ---")]
    // List of created enemies in scene
    //public List<Enemy> EnemyList = new List<Enemy>();
    // ********************************************
    public GameObject OptionsPanel; 

    
    float localCounter = 0f;

    // Singleton Game Manager *************************
    public static GameManager instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // ************************************************

    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Enemy spawn courutine
    IEnumerator Spawn()
    {
        // Check level variables and conditions
        if (enemiesPerWave > 0 && totalEnemySpawned < totalEnemies && !isPaused && !IsGameOver)
        {
            waveCount++;

            if ((totalEnemies - totalEnemySpawned) >= enemiesPerWave)
            {
                // Start instantiate enemies (wave)
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    // Check current enemies count 
                    if (currentEnemiesList.Count <= enemiesPerWave && localCounter == 0)
                    {
                        // Instantiate new enemy
                        GameObject newEnemy = Instantiate(enemiesList[0], spawnPoint.transform.position, Quaternion.identity);
                        newEnemy.AddComponent<NavAgent>().WaypointNetwork = GameObject.FindGameObjectWithTag("WaypointNetwork").GetComponent<AIWaypointNetwork>();
                        currentEnemiesList.Add(newEnemy);

                        totalEnemySpawned++;

                        // Wait delay time and repeat
                        yield return new WaitForSeconds(spawnDelay);
                    }
                }
            }
            else
            {
                int enemyAfter = totalEnemies - totalEnemySpawned;
                // Start instantiate enemies (wave)
                for (int i = 0; i < enemyAfter; i++)
                {
                        // Instantiate new enemy
                        GameObject newEnemy = Instantiate(enemiesList[0], spawnPoint.transform.position, Quaternion.identity);
                        newEnemy.AddComponent<NavAgent>().WaypointNetwork = GameObject.FindGameObjectWithTag("WaypointNetwork").GetComponent<AIWaypointNetwork>();
                        currentEnemiesList.Add(newEnemy);

                        totalEnemySpawned++;

                        // Wait delay time and repeat
                        yield return new WaitForSeconds(spawnDelay);
                }
            }
            localCounter = nextWaveTime;
            yield return new WaitForSeconds(nextWaveTime);
            localCounter = 0;
            StartCoroutine(Spawn());
        }
    }
}
