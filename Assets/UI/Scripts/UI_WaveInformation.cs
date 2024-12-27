using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WaveInformation : MonoBehaviour
{
    public static UI_WaveInformation instance;

    public GameObject waveNumber;
    public GameObject timeToNextWave;

    public GameObject gameManager;
    private WaveSpawner waveSpawner;

    private void Awake()
    {
        instance = this;
        waveSpawner = gameManager.GetComponent<WaveSpawner>();
    }

    public void UpdateWaveNumber()
    {
        waveNumber.GetComponent<TextMeshProUGUI>().text = "Wave: " + waveSpawner.GetWaveNumber();
    }

    public void UpdateTimeToNextWave()
    {
        timeToNextWave.GetComponent<TextMeshProUGUI>().text = "Time to next wave: " + string.Format("{0:00.00}", waveSpawner.GetTimeToNextWave());

    }

}
