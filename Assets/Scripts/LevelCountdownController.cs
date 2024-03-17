using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LevelCountdownController : MonoBehaviour
{
    #region VARIABLES
    public static Action OnLevelTimeEnd;
    public static Action OnLevelTimeReloadStart;
    public static Action OnLevelTimeReloadEnd;

    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private int timeRemaining = 10;
    private int defaultRemainingTime = 0;

    private Color defaultColor;

    private Animator[] toxicUIAnims = new Animator[2];

    private bool inSafe = true;
    private bool canCountdown => !inSafe && timeRemaining >= 1 && !destroyCancellationToken.IsCancellationRequested;
    #endregion

    #region UNITY EVENT FUNCTIONS
    private void OnEnable()
    {
        Elevator.OnReachedStop += ResetCountdow;
        GameManager.OnWin += Restart;
    }

    private void OnDisable()
    {
        Elevator.OnReachedStop -= ResetCountdow;
        GameManager.OnWin -= Restart;
    }

    void Start()
    {
        InitializeCountdown();
        CountdownAsync();
    }
    #endregion

    #region METHODS

    #region Initialize
    private void InitializeCountdown()
    {
        defaultRemainingTime = timeRemaining;

        countdownText = GetComponent<TextMeshProUGUI>();
        defaultColor = countdownText.color;
        UpdateCountdownDisplay();

        toxicUIAnims = transform.parent.GetComponentsInChildren<Animator>();
        SetEnabilityToxicUIAnimations(false);
    }
    #endregion

    private void StartCountdown()
    {
        inSafe = false;

        if (timeRemaining == defaultRemainingTime)
        {
            CountdownAsync();
            ToxicUIAnim();
        }
    }

    async void CountdownAsync()
    {
        for (; canCountdown; timeRemaining--)
        {
            UpdateCountdownDisplay();

            if (IsTimeCritical())
            {
                // Change color to red when 20% of time remaining
                countdownText.color = Color.red;
            }

#if UNITY_EDITOR
            await Task.Delay(Input.GetKey(KeyCode.T) ? 100 : 1000);
#else
            await Task.Delay(1000);
#endif
        }

        if (!inSafe && !destroyCancellationToken.IsCancellationRequested)
            EndLevelTime();
    }

    private void UpdateCountdownDisplay()
    {
        countdownText.text = TimeSpan.FromSeconds(timeRemaining).ToString(@"mm\:ss");
    }

    private bool IsTimeCritical()
    {
        return timeRemaining <= Math.Max(10, defaultRemainingTime * .2f);
    }

    private void EndLevelTime()
    {
        countdownText.fontSize = 42;
        countdownText.text = "Time End!";
        ToxicUIAnim();
        Invoke(nameof(TriggerLevelEndTime), 1);
        Invoke(nameof(Restart), 1);
    }

    private void TriggerLevelEndTime()
    {
        OnLevelTimeEnd?.Invoke();
    }

    private async Task ReloadLevelTime()
    {
        inSafe = true;
        countdownText.color = Color.green;
        OnLevelTimeReloadStart.Invoke();
        int speed = 200;

        while (timeRemaining != defaultRemainingTime)
        {
            timeRemaining++;
            UpdateCountdownDisplay();
            await Task.Delay(Math.Max(1, speed -= 5));
        }

        await Task.Delay(500);
        countdownText.color = defaultColor;
        timeRemaining = defaultRemainingTime;
        OnLevelTimeReloadEnd.Invoke();
    }

    private void SetEnabilityToxicUIAnimations(bool isEnable)
    {
        toxicUIAnims[0].enabled = isEnable;
        toxicUIAnims[1].enabled = isEnable;
    }

    async void ToxicUIAnim()
    {
        SetEnabilityToxicUIAnimations(true);

        await Task.Delay(3000);

        SetEnabilityToxicUIAnimations(false);
    }

    async void ResetCountdow(int stop)
    {
        if (stop != 0) StartCountdown();
        else await ReloadLevelTime();
    }

    public async void Restart()
    {
        inSafe = true;

        PrepareForQuit();

        for (int i = 10; i >= 0; i--, await Task.Delay(1000))
            countdownText.text = "Quit in\n" + i;

        Application.Quit();
    }

    private void PrepareForQuit()
    {
        inSafe = true;
        countdownText.fontSize = 42;
        countdownText.color = Color.red;
    }

    #endregion
}