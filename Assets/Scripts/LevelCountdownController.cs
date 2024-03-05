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
    

    void Start()
    {
        print("Level Countdown Started");
        countdownText = GetComponent<TextMeshProUGUI>();
        
        timeRemainingDefault = timeRemaining;

        Elevator.OnReachedStop += ResetCountdow;

        CountdownAsync();

        //await Task.Delay(TimeSpan.FromSeconds(timeRemaining), destroyCancellationToken);

        //OnLevelTimeEnd?.Invoke();
    }

    async void CountdownAsync()
    {        
        for (; timeRemaining >= 1 && !destroyCancellationToken.IsCancellationRequested;  timeRemaining--)
        {
            countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");

            if (timeRemaining * 10 <= Math.Max(10, timeRemaining * .2f * 10)) countdownText.color = Color.red; // Change color to red when 20% of time remaining            
            
            await Task.Delay(Input.GetKey(KeyCode.T) ? 100 : 1000);            
        }

        countdownText.text = "Time End!";
        OnLevelTimeEnd?.Invoke();
    }

    async void ResetCountdow(int stop)
    {
        if (stop == 1)
        {
            timeRemaining = timeRemainingDefault;
            countdownText.color = Color.green;
            await Task.Delay(1000);
            countdownText.color = Color.white;
        }        
    }
}
