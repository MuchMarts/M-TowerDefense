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
        TextMeshProUGUI textObject = health.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        textObject.text = "Health: " + playerManager.GetHealth();
    }

    public void UpdatePoints()
    {
        TextMeshProUGUI textObject = points.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        textObject.text = "Points: " + playerManager.GetPoints();
    }

    public void UpdateScore()
    {
        TextMeshProUGUI textObject = score.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        textObject.text = "Score: " + playerManager.GetScore();
    }
    
}
