using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
public class SoundData : ScriptableObject
{
    public AudioClip[] Musics;
    [Space]
    public AudioClip[] SFX;

    [FormerlySerializedAs(oldName: "Clips")]
    public AudioClips[] Clips;
    [Serializable]
    public struct AudioClips
    {
        public string name;
        public AudioClip Clip;
    }

    #region Editor Code
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Clips.Length != Enum.GetNames(typeof(SoundEnum)).Length)
        {
            //Array.Resize(ref Clips, 0);
            Clips = new AudioClips[Enum.GetNames(typeof(SoundEnum)).Length];
            Debug.Log("Asigned Key Materials Lenght");
        }
        if (Clips[0].name != ((SoundEnum)0).ToString())
            for (int i = 0; i < Enum.GetNames(typeof(SoundEnum)).Length; i++)
                Clips[i].name = ((SoundEnum)i).ToString();
    }
#endif
    #endregion

    public enum SoundEnum
    {
        Unlock = 0,
        LockedDoor = 1,
        DoorOpening = 2,
        ElevatorMovement = 3,
        ElevatorDoorMovement = 4,
        ElevatorButtonPressedActive = 5,
        ElevatorButtonPressedPassive = 6,
        ZombieBreathing = 7,
        ZombieAttack1 = 8,
        ZombieAttack2 = 9,
        ZombieAttack3 = 10,
        ZombieAttack4 = 11,
    }

    public AudioClip GetSFXClip(SoundEnum clip)
    {
        return Clips[((int)clip)].Clip;
        //return SFX[((int)clip)];
    }
}