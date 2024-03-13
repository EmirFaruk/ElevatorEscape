using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class HUD : MonoBehaviour
{
    #region VARIABLES
    #region Singleton
    private static HUD instance;
    public static HUD Instance => instance;
    #endregion

    #region Item
    [SerializeField] private TextMeshProUGUI itemAmountTmp;

    private int itemAmount;
    public int GetItemAmount => itemAmount;
    public void SetItemAmount(int data) { itemAmount += data; itemAmountTmp.SetText(itemAmount.ToString()); }
    #endregion

    #region OutlineShader // nerede olmalý?
    [SerializeField] private Shader outlineShader;
    public Shader OutlineShader => outlineShader;
    #endregion

    #region PopUp
    [Header("PopUp")]
    [SerializeField] private Canvas popUp;
    private TextMeshProUGUI tmp;
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

    [Inject] ZenjectGetter zenjectGetter;
    #endregion

    private void Awake()
    {
        instance = this;

        Time.timeScale = 1;

        popUp = Instantiate(popUp);
        popUp.gameObject.SetActive(false);
        tmp = popUp.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        deathPanel.SetActive(false);
        winPanel.SetActive(false);
        fadePanel.SetActive(true);
        restartButton.transform.parent.gameObject.SetActive(false);
        quitButton.transform.parent.gameObject.SetActive(false);
        tabMenu.SetActive(false);
        TabMenuToggle();

        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        quitButton.onClick.AddListener(() =>
#if UNITY_EDITOR        
        UnityEditor.EditorApplication.isPlaying = false);
#else 
        Application.Quit());
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) TabMenuToggle();
    }

    void TabMenuToggle()
    {
        tabMenu.SetActive(!tabMenu.activeInHierarchy);
        SetEnabilityButtons(tabMenu.activeInHierarchy);
        Cursor.visible = tabMenu.activeInHierarchy;
        Cursor.lockState = tabMenu.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void SetEnabilityButtons(bool isActive)
    {
        restartButton.transform.parent.gameObject.SetActive(isActive);
        quitButton.transform.parent.gameObject.SetActive(isActive);
    }

    private void OnEnable()
    {
        LevelCountdownController.OnLevelTimeEnd += () => EndGame(false);
        PlayerHealth.OnDeath += () => EndGame(false);
        ExitDoor.OnWin += () => EndGame(true);
    }

    private void OnDisable()
    {
        LevelCountdownController.OnLevelTimeEnd -= () => EndGame(false);
        PlayerHealth.OnDeath -= () => EndGame(false);
        ExitDoor.OnWin -= () => EndGame(true);
    }

    void EndGame(bool isWin)
    {
        if (isWin) winPanel.SetActive(true);
        else deathPanel.SetActive(true);

        SetEnabilityButtons(true);

        Time.timeScale = 0;
    }


    #region PopUp

    public void ShowPopUp(Vector3 position, string messageBase, string hue, string end, Color color)
    {
        popUp.transform.position = position;
        popUp.transform.LookAt(Camera.main.transform);

        tmp.SetText(messageBase + $"{hue.AddColor(color)}" + end);

        popUp.gameObject.SetActive(true);
    }
    public void HidePopUp()
    {
        popUp.gameObject.SetActive(false);
    }
    #endregion

    #region Take Damage Effect

    [SerializeField] private GameObject takeDamageRef;
    private Animator takeDamageAnim => takeDamageRef.GetComponent<Animator>();
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

    public void DeactivateTakeDamageEffect()
    {
        takeDamageRef.SetActive(false);
        isPlayTakeDamageEffect = false;
    }

    #endregion
}
