using System;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    public static RingManager Instance;
    public List<RingSO> AllRingSO;

    internal GameObject GetRingPrefab(int ringID)
    {
        return AllRingSO[ringID].ringPrefab;
    }

    void Awake()
    {
        Instance = this;
    }

}
