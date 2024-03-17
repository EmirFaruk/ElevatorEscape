using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class HUD : MonoBehaviour, IHUD
{
    #region VARIABLES
    
    #region Item
    [SerializeField] private TextMeshProUGUI itemAmountTmp;

    private int itemAmount;
    public int GetItemAmount => itemAmount;    

    public void SetItemAmount(int data) => itemAmountTmp.SetText((itemAmount += data).ToString()); 
   
    #endregion    

    #region PopUp
    [Header("PopUp")]
    [SerializeField] private Canvas popUp;
    private TextMeshProUGUI tmpPopUp;
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
        SetButtonVisibility(false);
        fadePanel.SetActive(true);
        tabMenu.SetActive(false);
        ToggleTabMenu();
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

    private void SetButtonVisibility(bool isActive)
    {
        restartButton.gameObject.SetActive(isActive);
        quitButton.gameObject.SetActive(isActive);
    }
    #endregion

    #region Tab Menu
    private void ToggleTabMenu()
    {
        bool isActive = !tabMenu.activeInHierarchy;
        tabMenu.SetActive(isActive);
        SetButtonVisibility(isActive);
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
        SetButtonVisibility(true);

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

    #endregion
}

public interface IHUD
{
    int GetItemAmount { get; }    
    void SetItemAmount(int data);
    void ShowPopUp(Vector3 position, string messageBase, string hue, string end, Color color);
    void HidePopUp();
    void ActivateTakeDamageEffect();
}