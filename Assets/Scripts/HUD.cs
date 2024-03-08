using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    #region Item
    [SerializeField] private TextMeshProUGUI itemAmountTmp;

    private int itemAmount;
    public int GetItemAmount => itemAmount;
    public void SetItemAmount(int data) { itemAmount += data; itemAmountTmp.SetText(itemAmount.ToString()); }
    #endregion

    //Outline
    [SerializeField] private Shader outlineShader;
    public Shader OutlineShader => outlineShader;

    //PopUp
    [SerializeField] private Canvas popUp;
    private TextMeshProUGUI tmp;

    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private GameObject winPanel;

    #region SoundManager

    public AudioClip Unlock;
    public AudioClip LockedDoor;
    public AudioClip DoorOpening;

    #endregion

    private void Awake()
    {
        if (Instance == null) Instance = this;

        Time.timeScale = 1;

        popUp = Instantiate(popUp);
        popUp.gameObject.SetActive(false);
        tmp = popUp.GetComponentInChildren<TextMeshProUGUI>();

        foreach (Transform child in transform)
            if (child.CompareTag("Death")) deathPanel = child.gameObject;
    }

    private void Start()
    {
        fadePanel.SetActive(true);
    }

    private void OnEnable()
    {
        PlayerHealth.OnDeath += () => { deathPanel.SetActive(true); Time.timeScale = 0; };
        ExitDoor.OnWin += () => { winPanel.SetActive(true); Time.timeScale = 0; };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) ActivateTakeDamageEffect();
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
