using StarterAssets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Chest : Interactable
{
    #region VARIABLES
    [SerializeField] private KeyData.KeyType key;
    private bool hasKey => player.CurrentKey && player.CurrentKey.KeyType == key;
    private bool isOpen;
    private Transform handle;
    private float angle = 0;

    [Inject]
    private FirstPersonController player;

    private Item item;

    //PopUp
    private Vector3 popUpPosition => transform.position + Vector3.up / 2;
    private Color popUpColor => KeyData.KeyColors[key];
    #endregion

    #region INTERACTABLE METHODS
    public override void OnFocus()
    {
        if (angle != handle.localEulerAngles.x)
        {
            HUD.Instance.HidePopUp();
            return;
        }

        if (hasKey && !isOpen)
            HUD.Instance.ShowPopUp(popUpPosition, "Open ", key.ToString(), " Chest", popUpColor);

        else if (!isOpen)
            HUD.Instance.ShowPopUp(popUpPosition, "You need a ", key.ToString(), " Key", popUpColor);
    }

    public override void OnInteract()
    {
        //if (isOpen) Close();
        /*else*/
        if (hasKey) Open();
        HUD.Instance.HidePopUp();
    }

    public override void OnLoseFocus()
    {
        HUD.Instance.HidePopUp();
    }

    #endregion

    public override void OnEnable()
    {
        handle = transform.GetChild(1);
        angle = handle.localEulerAngles.x;

        base.OnEnable();

        if (item = GetComponentInChildren<Item>()) item.GetComponent<Collider>().enabled = false;
    }

    async void Open()
    {
        while (angle != -120)
        {
            angle = Mathf.Lerp(angle, -120, 0.05f);
            handle.localEulerAngles = new Vector3(angle, 0, 0);

            if (angle < -119f)
            {
                angle = -120;
                isOpen = true;

                if (item) item.GetComponent<Collider>().enabled = true;
                GetComponent<Collider>().enabled = false;

                HUD.Instance.HidePopUp();
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
