using UnityEngine;

[CreateAssetMenu(fileName = "NewBuffSO", menuName = "Game/Buff")]
public class BuffSO : ScriptableObject
{
    public string buffName;
    public BuffType type;
    public float duration;
    public float tickRate; // Per second
    public float value;
    public bool canStack;    
}
