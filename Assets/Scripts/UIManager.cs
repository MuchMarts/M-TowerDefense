using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
