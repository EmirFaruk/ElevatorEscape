using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public async void RestartLevel()
    {
        bool isRestart = false;
        while (!isRestart && !destroyCancellationToken.IsCancellationRequested)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                isRestart = true;
            }
            await Task.Delay(5);
        }
        isRestart = true;
    }
}
