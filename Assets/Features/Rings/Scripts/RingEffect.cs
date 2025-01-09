public class RingEffect
{
    public string effectName;
    public RingEffectType type;
    public object value;
    public bool isAdjacencyBonus;
    public void SetValues(RingEffectSO effect)
    {
        effectName = effect.name;
        type = effect.effectType;
        isAdjacencyBonus = effect.isAdjacencyBonus;

        switch (effect.valueType)
        {
            case RingEffectSO.ValueType.String:
                value = effect.stringValue;
                break;
            case RingEffectSO.ValueType.Int:
                value = effect.intValue;
                break;
            case RingEffectSO.ValueType.Float:
                value = effect.floatValue;
                break;
            case RingEffectSO.ValueType.Bool:
                value = effect.boolValue;
                break;
            case RingEffectSO.ValueType.ProjectileSO:
                value = effect.projectileValue;
                break;
            case RingEffectSO.ValueType.BuffSO:
                value = effect.buffValue;
                break;
        }
    }
}