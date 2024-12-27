using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uiContainer;
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth()
    {
        return;
    }

}
