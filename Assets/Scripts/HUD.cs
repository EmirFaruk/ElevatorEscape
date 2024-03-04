using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    #region Item
    [SerializeField] private TextMeshProUGUI itemAmountTmp;

    private int itemAmount;
    public int GetItemAmount() => itemAmount;
    public void SetItemAmount(int data) { itemAmount += data; itemAmountTmp.SetText(itemAmount.ToString()); }
    #endregion

    [SerializeField] private Canvas popUp;
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        tmp = popUp.GetComponentInChildren<TextMeshProUGUI>();

    }

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
}
