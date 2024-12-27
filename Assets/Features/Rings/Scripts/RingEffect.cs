public class RingEffect
{
    public string effectName;
    public RingEffectType type;
    public object value;

    public void SetValues(RingEffectSO effect)
    {
        effectName = effect.effectName;
        type = effect.type;
        
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
            case RingEffectSO.ValueType.ScriptableObject:
                value = effect.scriptableObject;
                break;
            case RingEffectSO.ValueType.BuffSO:
                value = effect.buffValue;
                break;
        }
    }
}