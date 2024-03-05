using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LevelCountdownController : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 10;
    public static Action OnLevelTimeEnd;
    [SerializeField] private TextMeshProUGUI countdownText;

    void Start()
    {
        print("Level Countdown Started");
        countdownText = GetComponent<TextMeshProUGUI>();

        CountdownAsync();

        //await Task.Delay(TimeSpan.FromSeconds(timeRemaining), destroyCancellationToken);

        //OnLevelTimeEnd?.Invoke();
    }

    async void CountdownAsync()
    {
        for (int i = (int)timeRemaining; i >= 1 && !destroyCancellationToken.IsCancellationRequested; i--)
        {
            countdownText.text = TimeSpan.FromSeconds(i).ToString(@"mm\:ss");

            if (i * 10 <= Math.Max(10, timeRemaining * .2f * 10)) countdownText.color = Color.red; // Change color to red when 20% of time remaining            
            
            await Task.Delay(Input.GetKey(KeyCode.T) ? 100 : 1000);            
        }

        countdownText.text = "Time End!";
        OnLevelTimeEnd?.Invoke();
    }
}
