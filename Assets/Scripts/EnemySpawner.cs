using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float timeBetweenWaves = 5f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();


    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Start() {
        StartWave();
        StartCoroutine(StartWave());
    }

    private void Awake() {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;   

        if ((timeSinceLastSpawn >= (1f / enemiesPerSecond)) && (enemiesLeftToSpawn > 0)){
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0){
            EndWave();
        }


    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();    
    }

    void EndWave(){
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy() {
        GameObject prefabToSpawn = enemyPrefabs[0];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave() {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void EnemyDestroyed() {
        enemiesAlive--;
    }
}
