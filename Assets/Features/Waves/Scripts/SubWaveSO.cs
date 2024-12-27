using UnityEngine;

[CreateAssetMenu(fileName = "NewSubwaveSO", menuName = "Game/Wave/Subwave")]
public class SubWaveSO : ScriptableObject
{
    public EnemySO enemySO;
    public int count;
    public float rate;
    public float delay;
    public bool isFinished;       
}
