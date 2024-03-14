using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer soundMixer;

    public void SetMasterVolume(float volume)
    {
        soundMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        soundMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        soundMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20f);
    }
}
