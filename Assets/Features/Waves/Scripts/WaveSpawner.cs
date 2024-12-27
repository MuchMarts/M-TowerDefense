using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
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
    private float countdown = 0f;

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
        return waves[waveIndex].subWaves.All(subWave => subWave.isFinished);
    }

    IEnumerator SpawnWave()
    {   
        // Spawn enemies
        for (int i = 0; i < waves[waveIndex].subWaves.Count() ; i++)
        {
            StartCoroutine(SpawnSubWave(waves[waveIndex].subWaves[i]));
        }
        waveIndex++;
        yield return null;
    }

    IEnumerator SpawnSubWave(SubWaveSO subWave)
    {
        yield return new WaitForSeconds(subWave.delay);
        for (int i = 0; i < subWave.count; i++)
        {
            SpawnEnemy(subWave.enemySO);
            yield return new WaitForSeconds(1f / subWave.rate);
        }
        subWave.isFinished = true;
    }

    void SpawnEnemy(EnemySO enemySO)
    {
        if (enemySO == null)
        {
            Debug.LogError("Enemy Prefab is null");
            return;
        }

        GameObject enemy = GameObjectPoolManager.Instance.GetObject(enemySO, enemySO.prefab, Waypoints.points[0].position, Quaternion.identity);
        enemy.GetComponent<Enemy>().SetEnemyData(enemySO);
        enemy.GetComponent<Enemy>().ResetEnemyData();
    }
}
