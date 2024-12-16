using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Spawner Settings")]
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float timeBetweenWaves = 10f;

    private int waveNumber = 0;
    private float countdown = 2f;

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {   
        waveNumber++;

        // Spawn enemies
        for (int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.1f);
        }

    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, Waypoints.points[0].position, Quaternion.identity);
        EnemyManager.Instance.RegisterEnemy(enemy);
    }
}
