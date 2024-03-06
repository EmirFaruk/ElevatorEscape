using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundData soundData;

    private AudioSource musicAudioSource;

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
