using UnityEngine;

[CreateAssetMenu(fileName = "NewRingEffectSO", menuName = "Game/RingEffect")]
public class RingEffectSO : ScriptableObject
{
    public string effectName;
    public RingEffectType type;
    public ValueType valueType;

    public string stringValue;
    public int intValue;
    public float floatValue;
    public bool boolValue;
    public ScriptableObject scriptableObject;
    public BuffSO buffValue;
    
    public enum ValueType
    {
        String,
        Int,
        Float,
        Bool,
        ScriptableObject,
        BuffSO
    }
    public bool isAdjacencyBonus;
}
