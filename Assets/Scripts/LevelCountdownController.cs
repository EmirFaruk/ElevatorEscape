using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelCountdownController : MonoBehaviour
{
    #region VARIABLES
    public static Action OnLevelTimeEnd;
    public static Action OnLevelTimeReloadStart;
    public static Action OnLevelTimeReloadEnd;

    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Transform toxicUIPanel;

    [SerializeField] private int timeRemaining = 10;
    private int defaultRemainingTime = 0;

    private Color defaultColor;

    private Animator[] toxicUIAnims = new Animator[2];

    private bool inSafe = true;
    private bool canCountdown => !inSafe && timeRemaining >= 1;
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
        StartCoroutine(CountdownCoroutine());
        SetActiveToxicUIPanel(false);
    }
    #endregion

    #region METHODS

    #region Initialize
    private void InitializeCountdown()
    {
        defaultRemainingTime = timeRemaining;

        // countdownText = GetComponent<TextMeshProUGUI>();
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
            StartCoroutine(CountdownCoroutine());
            StartCoroutine(ToxicUIAnimCoroutine());
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        while (canCountdown)
        {
            timeRemaining--;
            UpdateCountdownDisplay();

            if (IsTimeCritical())
            {
                // Change color to red when 20% of time remaining
                countdownText.color = Color.red;
            }

#if UNITY_EDITOR
            yield return new WaitForSeconds(Input.GetKey(KeyCode.T) ? 0.1f : 1f);
#else
            yield return new WaitForSeconds(1f);
#endif
        }

        if (!inSafe)
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
        StartCoroutine(ToxicUIAnimCoroutine());
        Invoke(nameof(TriggerLevelEndTime), 1);
        Invoke(nameof(Restart), 1);
    }

    private void TriggerLevelEndTime()
    {
        OnLevelTimeEnd?.Invoke();
    }

    private IEnumerator ReloadLevelTimeCoroutine()
    {
        inSafe = true;
        countdownText.color = Color.green;
        OnLevelTimeReloadStart.Invoke();
        int speed = 200;

        while (timeRemaining != defaultRemainingTime)
        {
            timeRemaining++;
            UpdateCountdownDisplay();
            yield return new WaitForSeconds(Math.Max(0.01f, (float)speed / 1000));
            speed -= 5;
        }

        yield return new WaitForSeconds(0.5f);
        countdownText.color = defaultColor;
        timeRemaining = defaultRemainingTime;
        OnLevelTimeReloadEnd.Invoke();
        SetActiveToxicUIPanel(false);
    }

    private void SetEnabilityToxicUIAnimations(bool isEnable)
    {
        toxicUIAnims[0].enabled = isEnable;
        toxicUIAnims[1].enabled = isEnable;
    }

    private void SetActiveToxicUIPanel(bool hasActive)
    {
        toxicUIPanel.gameObject.SetActive(hasActive);
    }

    private IEnumerator ToxicUIAnimCoroutine()
    {
        SetEnabilityToxicUIAnimations(true);

        yield return new WaitForSeconds(3);

        SetEnabilityToxicUIAnimations(false);
    }

    private void ResetCountdow(int stop)
    {
        if (stop != 0)
        {
            SetActiveToxicUIPanel(true);
            StartCountdown();
        }
        else
            StartCoroutine(ReloadLevelTimeCoroutine());
    }

    public void Restart()
    {
        inSafe = true;

        PrepareForQuit();

        StartCoroutine(QuitCountdownCoroutine());
    }

    private IEnumerator QuitCountdownCoroutine()
    {
        for (int i = 10; i >= 0; i--)
        {
            countdownText.text = "Quit in\n" + i;
            yield return new WaitForSeconds(1);
        }

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
