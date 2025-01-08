using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    public static RingManager Instance;
    public List<RingSO> allRingSO;

    void Awake()
    {
        Instance = this;
    }

}
