using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCountdownController : MonoBehaviour
{
    [SerializeField] private int timeRemaining = 10;
    private int timeRemainingDefault = 0;
    public static Action OnLevelTimeEnd;

    [SerializeField] private TextMeshProUGUI countdownText;

    private Color defaultColor;

    void Start()
    {
        print("Level Countdown Started");
        countdownText = GetComponent<TextMeshProUGUI>();

        timeRemainingDefault = timeRemaining;
        countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");

        defaultColor = countdownText.color;

        Elevator.OnReachedStop += ResetCountdow;

        CountdownAsync();

        //await Task.Delay(TimeSpan.FromSeconds(timeRemaining), destroyCancellationToken);

        //OnLevelTimeEnd?.Invoke();

        PlayerHealth.OnDeath += () => { inBase = true; Restart(); };
        ExitDoor.OnWin += () => { inBase = true; Restart(); };
    }

    async void CountdownAsync()
    {
        for (; !inBase && timeRemaining >= 1 && !destroyCancellationToken.IsCancellationRequested; timeRemaining--)
        {
            countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");

            if (timeRemaining * 10 <= Math.Max(10, timeRemaining * .2f * 10)) countdownText.color = Color.red; // Change color to red when 20% of time remaining            

            await Task.Delay(Input.GetKey(KeyCode.T) ? 100 : 1000);
        }

        if (!inBase)
        {
            countdownText.text = "Time End!";
            OnLevelTimeEnd?.Invoke();
        }
    }

    bool inBase = true;
    async void ResetCountdow(int stop)
    {
        if (stop != 0)
        {
            inBase = false;
            if (timeRemaining == timeRemainingDefault) CountdownAsync();
        }
        else
        {
            inBase = true;
            countdownText.color = Color.green;

            while (timeRemaining != timeRemainingDefault)
            {
                timeRemaining++;
                countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");
                await Task.Delay(100);
            }

            await Task.Delay(500);
            countdownText.color = defaultColor;
            timeRemaining = timeRemainingDefault;
            //countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");*/
        }
    }

    async void Restart()
    {
        bool isRestart = false;
        while (!isRestart && !destroyCancellationToken.IsCancellationRequested)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                isRestart = true;
            }
            await Task.Delay(10);
        }
        isRestart = true;
    }
}
