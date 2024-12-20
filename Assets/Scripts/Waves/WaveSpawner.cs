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
    private Wave[] waves;
    [SerializeField]
    private float timeBetweenWaves = 10f;

    private int waveIndex = 0;
    private float countdown = 2f;

    public UnityEvent WavesCompleted;
    public UnityEvent WaveStarted;
    public UnityEvent WaveEnded;

    private bool waveInProgress = false;

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            WaveStarted.Invoke();
        }

        if ( EnemyManager.Instance.NoEnemiesRemaining() && !waveInProgress)
        {
            WaveEnded.Invoke();
            countdown -= Time.deltaTime;
        }
        
        if (waveIndex == waves.Length && EnemyManager.Instance.NoEnemiesRemaining() && !waveInProgress)
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

    IEnumerator SpawnWave()
    {   
        waveInProgress = true;
        // Spawn enemies
        for (int i = 0; i < waves[waveIndex].subWaves.Count() ; i++)
        {
            StartCoroutine(SpawnSubWave(waves[waveIndex].subWaves[i]));
        }
        waveIndex++;
        waveInProgress = false;
        yield return null;
    }

    IEnumerator SpawnSubWave(SubWave subWave)
    {
        yield return new WaitForSeconds(subWave.delay);
        for (int i = 0; i < subWave.count; i++)
        {
            SpawnEnemy(subWave.enemyPrefab);
            yield return new WaitForSeconds(1f / subWave.rate);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab is null");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, Waypoints.points[0].position, Quaternion.identity);
    }
}
