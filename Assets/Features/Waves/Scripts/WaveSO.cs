using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveSO", menuName = "Game/Wave")]
public class WaveSO : ScriptableObject
{
    public SubWaveSO[] subWaves;
}

