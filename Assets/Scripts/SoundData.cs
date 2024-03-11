using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
public class SoundData : ScriptableObject
{
    public AudioClip[] Musics;
    [Space]
    public AudioClip[] SFX;

    public enum SoundEnum
    {
        Unlock,
        LockedDoor,
        DoorOpening
    }

    public AudioClip GetSFXClip(SoundEnum clip)
    {
        return SFX[((int)clip)];
    }
}