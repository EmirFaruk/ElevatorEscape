using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LevelCountdownController : MonoBehaviour
{
    [SerializeField] private int timeRemaining = 10;
    private int timeRemainingDefault = 0;
    public static Action OnLevelTimeEnd;
    [SerializeField] private TextMeshProUGUI countdownText;

    Elevator elevator;

    void Start()
    {
        print("Level Countdown Started");
        countdownText = GetComponent<TextMeshProUGUI>();

        timeRemainingDefault = timeRemaining;
        countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");

        Elevator.OnReachedStop += ResetCountdow;

        CountdownAsync();

        //await Task.Delay(TimeSpan.FromSeconds(timeRemaining), destroyCancellationToken);

        //OnLevelTimeEnd?.Invoke();
    }

    async void CountdownAsync()
    {
        bool condition = !inBase && timeRemaining >= 1 && !destroyCancellationToken.IsCancellationRequested;
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
        if (stop != 1)
        {
            inBase = false;
            if (timeRemaining == timeRemainingDefault) CountdownAsync();
        }
        else
        {
            inBase = true;
            countdownText.color = Color.green;
            await Task.Delay(1000);
            countdownText.color = Color.white;
            timeRemaining = timeRemainingDefault;
            countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");
        }
    }
}
