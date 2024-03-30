using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    #region VARIABLES
    public SoundData SoundData => soundData;
    [SerializeField] private SoundData soundData;
    [SerializeField] private AudioMixer soundMixer;

    private AudioSource musicAudioSource;
    private AudioSource zombieAudioSource;
    public static AudioSource ZombieAudioSource;

    public static Action<SoundData.SoundEnum> OnSFXCall = _ => { };
    public static Action<AudioSource, SoundData.SoundEnum> OnAudioSourceSet = (_, _) => { };
    #endregion

    #region UNITY EVENT FUNCTIONS
    private void OnEnable()
    {
        OnSFXCall += PlaySFX;
        OnAudioSourceSet += SetAudioSourceClip;
    }

    private void OnDisable()
    {
        OnSFXCall -= PlaySFX;
        OnAudioSourceSet -= SetAudioSourceClip;
    }

    private void Awake()
    {
        musicAudioSource = Instantiate(new GameObject("MusicAudioSource"), transform).AddComponent<AudioSource>();
        zombieAudioSource = Instantiate(new GameObject("ZombieAudioSource"), transform).AddComponent<AudioSource>();
        ZombieAudioSource = zombieAudioSource;
    }

    void Start()
    {
        SetAudioSourceAttributes(musicAudioSource, true, "Music");
        PlayRandomMusic(null);

        SetAudioSourceAttributes(zombieAudioSource, false, "Sfx");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) PlayRandomMusic(musicAudioSource.clip);
    }
    #endregion

    private void SetAudioSourceAttributes(AudioSource audioSource, bool loop, string group)
    {
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = soundMixer.FindMatchingGroups(group)[0];
    }

    #region SFX Methods

    public async void PlaySFX(SoundData.SoundEnum sfxClip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = soundMixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = soundData.GetSFXClip(sfxClip);
        audioSource.Play();

        await Task.Delay(Math.Max(1000, ((int)audioSource.clip.length) * 1000));
        DestroyImmediate(audioSource);
    }

    public async void SetAudioSourceClip(AudioSource audioSource, SoundData.SoundEnum sfxClip)
    {

        audioSource.outputAudioMixerGroup = soundMixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = soundData.GetSFXClip(sfxClip);
        audioSource.Play();

        await Task.Delay(Math.Max(1000, ((int)audioSource.clip.length) * 1000));
        if (destroyCancellationToken.IsCancellationRequested) return;
        audioSource.clip = null;
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
