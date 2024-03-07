using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
public class SoundData : ScriptableObject
{
    public AudioClip[] Musics;

    public enum SoundEnum
    {
        Door,

    }
}