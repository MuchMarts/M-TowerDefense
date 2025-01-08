using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Spawner Settings")]
    [SerializeField]
    private WaveSO[] waves;
    [SerializeField]
    private float timeBetweenWaves = 10f;

    private int waveIndex = 0;
    private float countdown = 2f;
    private int subWavesBeingSpawned = 0;
    public UnityEvent WavesCompleted;
    public UnityEvent WaveStarted;
    public UnityEvent WaveEnded;

    void Awake()
    {
        Debug.Log("WaveSpawner Awake");
    }

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            WaveStarted.Invoke();
        }

        if ( EnemyManager.Instance.NoEnemiesRemaining() && HaveAllSubWavesFinished())
        {
            WaveEnded.Invoke();
            countdown -= Time.deltaTime;
        }
        
        if (waveIndex == waves.Length && EnemyManager.Instance.NoEnemiesRemaining() && HaveAllSubWavesFinished())
        {
            WavesCompleted.Invoke();
            Debug.Log("All waves completed");
        }
    }

    public float GetWaveNumber()
    {
        return waveIndex;
    }

    public float GetTimeToNextWave()
    {
        if (countdown < 0)
        {
            return 0;
        }
        return countdown;
    }

    private bool HaveAllSubWavesFinished()
    {
        return subWavesBeingSpawned == 0;
    }

    IEnumerator SpawnWave()
    {   
        subWavesBeingSpawned = waves[waveIndex].subWaves.Count();
        
        for (int i = 0; i < waves[waveIndex].subWaves.Count(); i++)
        {
            yield return StartCoroutine(SpawnSubWave(waves[waveIndex].subWaves[i]));
        }
        waveIndex++;
    }

    IEnumerator SpawnSubWave(SubWaveSO subWave)
    {
        for (int i = 0; i < subWave.enemyGroups.Count(); i++)
        {
            yield return StartCoroutine(SpawnEnemyGroup(subWave.enemyGroups[i])); 
        }
        subWavesBeingSpawned--;
    }

    IEnumerator SpawnEnemyGroup(EnemyGroupSO enemyGroup)
    {
        yield return new WaitForSeconds(enemyGroup.delay);
        for (int i = 0; i < enemyGroup.count; i++)
        {
            SpawnEnemy(enemyGroup.enemySO);
            yield return new WaitForSeconds(1f / enemyGroup.rate);
        }
    }

    void SpawnEnemy(EnemySO enemySO)
    {
        if (enemySO == null)
        {
            Debug.LogError("Enemy Prefab is null");
            return;
        }

        GameObject enemy = GameObjectPoolManager.Instance.GetObject(enemySO, enemySO.prefab, Waypoints.points[0].position, Quaternion.identity);
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.SetEnemyData(enemySO);
        enemyComponent.ResetEnemyData();
        EnemyManager.Instance.RegisterEnemy(enemy);
    }
}
