using UnityEngine;

[CreateAssetMenu(fileName = "NewRingEffectSO", menuName = "Game/RingEffect")]
public class RingEffectSO : ScriptableObject
{
    public string ringEffectName;
    public RingEffectType effectType;
    public ValueType valueType;

    public string stringValue;
    public int intValue;
    public float floatValue;
    public bool boolValue;
    public ProjectileSO projectileValue;
    public BuffSO buffValue;
    
    public enum ValueType
    {
        String,
        Int,
        Float,
        Bool,
        ProjectileSO,
        BuffSO
    }
    public bool isAdjacencyBonus;
}
