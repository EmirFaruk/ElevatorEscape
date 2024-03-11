using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;
    private static GameManager instance;

    private AudioManager audioManager => GetComponent<AudioManager>();
    public AudioManager AudioManager => audioManager;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        LevelCountdownController.OnLevelTimeEnd += OnLevelTimeEnd;
    }

    private void OnDisable()
    {
        LevelCountdownController.OnLevelTimeEnd -= OnLevelTimeEnd;
    }

    private void OnLevelTimeEnd()
    {
        print("Level Time Ended");
        Time.timeScale = 0.0f;
    }
}
