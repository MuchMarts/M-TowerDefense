using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerStatsManager : MonoBehaviour
{
    public static UI_PlayerStatsManager instance;
    public GameObject playerStatsParent;

    public GameObject health;
    public GameObject points;
    public GameObject score; 

    public GameObject player;
    private PlayerManager playerManager;

    private void Awake()
    {
        instance = this;
        playerManager = player.GetComponent<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("PlayerManager not found");
        }
    }

    public void UpdateHealth()
    {
        health.GetComponent<TextMeshProUGUI>().text = "Health: " + playerManager.GetHealth();
    }

    public void UpdatePoints()
    {
        points.GetComponent<TextMeshProUGUI>().text = "Points: " + playerManager.GetPoints();
    }

    public void UpdateScore()
    {
        score.GetComponent<TextMeshProUGUI>().text = "Score: " + playerManager.GetScore();
    }
    
}
