using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LevelCountdownController : MonoBehaviour
{
    [SerializeField] private int timeRemaining = 10;
    private int defaultRemainingTime = 0;
    public static Action OnLevelTimeEnd;
    public static Action OnLevelTimeReloadStart;
    public static Action OnLevelTimeReloadEnd;

    [SerializeField] private TextMeshProUGUI countdownText;

    private Color defaultColor;

    private void OnEnable()
    {
        Elevator.OnReachedStop += ResetCountdow;        
        GameManager.OnWin += () => { inBase = true; Restart(); };
    }

    private void OnDisable()
    {
        Elevator.OnReachedStop -= ResetCountdow;        
        GameManager.OnWin -= () => { inBase = true; Restart(); };
    }

    void Start()
    {
        print("Level Countdown Started");

        defaultRemainingTime = timeRemaining;

        countdownText = GetComponent<TextMeshProUGUI>();
        defaultColor = countdownText.color;
        countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");

        toxicUIAnims = transform.parent.GetComponentsInChildren<Animator>();
        toxicUIAnims[0].enabled = false;
        toxicUIAnims[1].enabled = false;

        CountdownAsync();
    }

    private bool canCountdown => !inBase && timeRemaining >= 1 && !destroyCancellationToken.IsCancellationRequested;
    async void CountdownAsync()
    {
        for (; canCountdown; timeRemaining--)
        {
            countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");

            // Change color to red when 20% of time remaining            
            if (timeRemaining <= Math.Max(10, defaultRemainingTime * .2f)) countdownText.color = Color.red;

#if UNITY_EDITOR
            await Task.Delay(Input.GetKey(KeyCode.T) ? 100 : 1000);
#else
             await Task.Delay(1000);
#endif
        }

        if (!inBase && !destroyCancellationToken.IsCancellationRequested)
        {
            countdownText.fontSize = 42;
            countdownText.text = "Time End!";
            OnLevelTimeEnd?.Invoke();
        }
    }

    private Animator[] toxicUIAnims = new Animator[2];
    async void ToxicUIAnim()
    {
        toxicUIAnims[0].enabled = true;
        toxicUIAnims[1].enabled = true;

        await Task.Delay(3000);

        toxicUIAnims[0].enabled = false;
        toxicUIAnims[1].enabled = false;
    }

    private bool inBase = true;
    async void ResetCountdow(int stop)
    {
        if (stop != 0)
        {
            inBase = false;
            if (timeRemaining == defaultRemainingTime)
            {
                CountdownAsync();
                ToxicUIAnim();
            }
        }
        else
        {
            inBase = true;
            countdownText.color = Color.green;
            OnLevelTimeReloadStart.Invoke();
            int speed = 200;

            while (timeRemaining != defaultRemainingTime)
            {
                timeRemaining++;
                countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");
                await Task.Delay(Math.Max(1, speed -= 5));
            }

            await Task.Delay(500);
            countdownText.color = defaultColor;
            timeRemaining = defaultRemainingTime;
            OnLevelTimeReloadEnd.Invoke();         
        }
    }

    public async void Restart()
    {
        inBase = true;
        countdownText.fontSize = 42;
        countdownText.color = Color.red;

        for (int i = 10; i >= 0; i--)
        {
            countdownText.text = "Quit in\n" + i;
            await Task.Delay(1000);
        }

        Application.Quit();       
    }
}
