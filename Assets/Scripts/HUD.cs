using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IHUD
{
    #region VARIABLES

    #region Battery Item
    [SerializeField] private TextMeshProUGUI batteryItemAmountTmp;

    private int batteryItemAmount;
    public int GetBatteryItemAmount => batteryItemAmount;

    private Animator batteryItemAnim;
    #endregion

    #region PopUp
    [Header("PopUp")]
    [SerializeField] private Canvas popUp;
    private TextMeshProUGUI tmpPopUp;
    public Shader OutlineShader => outlineShader;
    [SerializeField] private Shader outlineShader;
    #endregion

    #region Panels
    [Header("Panels")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject tabMenu;
    #endregion

    #region Buttons
    [SerializeField] private Button quitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button soundSettingsButton;
    #endregion

    #endregion

    #region UNITY EVENT FUNCTIONS
    private void Awake()
    {
        SetPopUpReference();
        takeDamageAnim = takeDamageRef.GetComponent<Animator>();
    }

    private void SetPopUpReference()
    {
        popUp = Instantiate(popUp);
        popUp.gameObject.SetActive(false);
        tmpPopUp = popUp.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LevelCountdownController.OnLevelTimeEnd += () => EndGame(false);
        GameManager.OnWin += () => EndGame(true);
    }

    private void Start()
    {
        InitializeGame();
        SetButtonsAction();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleTabMenu();
    }
    #endregion

    #region METHODS

    #region Initialize
    private void InitializeGame()
    {
        deathPanel.SetActive(false);
        winPanel.SetActive(false);
        SetButtonsVisibility(false);
        fadePanel.SetActive(true);
        tabMenu.SetActive(false);
        ToggleTabMenu();
        batteryItemAnim = batteryItemAmountTmp.GetComponentInParent<Animator>();
        batteryItemAnim.enabled = false;
    }
    #endregion

    #region Button Methods
    private void SetButtonsAction()
    {
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        quitButton.onClick.AddListener(() =>
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false);
#else
                Application.Quit());
#endif
    }

    private void SetButtonsVisibility(bool isActive)
    {
        restartButton.gameObject.SetActive(isActive);
        quitButton.gameObject.SetActive(isActive);
        soundSettingsButton.gameObject.SetActive(isActive);
    }
    #endregion

    #region Tab Menu
    private void ToggleTabMenu()
    {
        bool isActive = !tabMenu.activeInHierarchy;
        tabMenu.SetActive(isActive);
        SetButtonsVisibility(isActive);
        SetCursorVisibility(isActive);
    }
    #endregion

    #region Cursor
    private void SetCursorVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
    #endregion

    #region EndGame
    public void EndGame(bool isWin)
    {
        if (isWin) winPanel.SetActive(true);
        else deathPanel.SetActive(true);

        SetCursorVisibility(true);
        SetButtonsVisibility(true);

        Time.timeScale = 0;
    }
    #endregion

    #region PopUp

    public void ShowPopUp(Vector3 position, string messageBase, string hue, string end, Color color)
    {
        popUp.transform.position = position;
        popUp.transform.LookAt(Camera.main.transform);

        tmpPopUp.SetText(messageBase + $"{hue.AddColor(color)}" + end);

        popUp.gameObject.SetActive(true);
    }

    public void HidePopUp()
    {
        popUp.gameObject.SetActive(false);
    }
    #endregion

    #region Take Damage Effect

    [SerializeField] private GameObject takeDamageRef;
    private Animator takeDamageAnim;// => takeDamageRef.GetComponent<Animator>();
    private bool isPlayTakeDamageEffect;

    public async void ActivateTakeDamageEffect()
    {
        if (!isPlayTakeDamageEffect)
        {
            isPlayTakeDamageEffect = true;
            takeDamageRef.SetActive(true);
            takeDamageAnim.Play("TakeDamage");

            await Task.Delay((int)takeDamageAnim.runtimeAnimatorController.animationClips[0].length * 1000);

            takeDamageRef.SetActive(false);
            isPlayTakeDamageEffect = false;
        }
    }

    #endregion

    #region Battery Item
    public void SetItemAmount(int data)
    {
        batteryItemAmountTmp.SetText((batteryItemAmount += data).ToString());
        ActivateBatteryItemEffect();
    }

    private async void ActivateBatteryItemEffect()
    {
        batteryItemAnim.enabled = true;

        await Task.Delay(2000);

        batteryItemAnim.enabled = false;
    }
    #endregion

    #endregion
}

public interface IHUD
{
    int GetBatteryItemAmount { get; }
    void SetItemAmount(int data);
    void ShowPopUp(Vector3 position, string messageBase, string hue, string end, Color color);
    void HidePopUp();
    void ActivateTakeDamageEffect();
}

/*
public interface IItem
{
    int GetItemAmount { get; }
    void SetItemAmount(int data);
}

public interface IPopUp
{
    void ShowPopUp(Vector3 position, string messageBase, string hue, string end, Color color);
    void HidePopUp();
}

public interface IDamageEffect
{
    void ActivateTakeDamageEffect();
}*/