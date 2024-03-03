using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    [SerializeField] private Canvas popUp;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ShowPopUp(Vector3 position, string message)
    {
        popUp.transform.position = position;
        popUp.transform.LookAt(Camera.main.transform);
        popUp.GetComponentInChildren<TextMeshProUGUI>().text = message;
        popUp.gameObject.SetActive(true);

        /*string a = ($"" +
            $"{"H".AddColor(Color.red)}" +
            $"{"E".AddColor(Color.blue)}" +
            $"{"L".AddColor(Color.green)}" +
            $"{"L".AddColor(Color.white)}" +
            $"{"O".AddColor(Color.yellow)}");*/
    }
    public void HidePopUp()
    {
        popUp.gameObject.SetActive(false);
    }
}
