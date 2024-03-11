using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public SoundData SoundData => soundData;
    [SerializeField] private SoundData soundData;

    private AudioSource musicAudioSource;

    public static Action<SoundData.SoundEnum> OnSFXCall;

    private void OnEnable()
    {
        OnSFXCall += PlaySFX;
    }

    private void OnDisable()
    {
        OnSFXCall -= PlaySFX;
    }

    void Start()
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
        PlayRandomMusic(musicAudioSource.clip);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) PlayRandomMusic(musicAudioSource.clip);
    }

    #region SFX Methods

    public async void PlaySFX(SoundData.SoundEnum sfxClip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundData.GetSFXClip(sfxClip);
        audioSource.Play();
        await Task.Delay(Math.Max(1000, ((int)audioSource.clip.length) * 1000));

        DestroyImmediate(audioSource);
    }

    #endregion

    #region Music Methods
    public AudioClip RandomMusic(AudioClip clip)
    {
        musicAudioSource.clip = soundData.Musics[Random.Range(0, soundData.Musics.Length)];
        return musicAudioSource.clip == clip ? RandomMusic(clip) : musicAudioSource.clip;
    }

    public void PlayRandomMusic(AudioClip clip)
    {
        while (clip == musicAudioSource.clip) musicAudioSource.clip = soundData.Musics[Random.Range(0, soundData.Musics.Length)];
        musicAudioSource.Play();
    }

    public void ChangeMusic(AudioClip clip) => musicAudioSource.clip = clip;
    #endregion
}
