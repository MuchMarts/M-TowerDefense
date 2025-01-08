using UnityEngine;

[CreateAssetMenu(fileName = "NewRingSO", menuName = "Game/Ring")]
public class RingSO : ScriptableObject
{
    public string ringName;
    public int cost;
    public float adjacencyBonus;
    public RingEffectSO[] effects;
    public GameObject prefab;
}
