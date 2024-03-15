using StarterAssets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Chest : Interactable
{
    #region VARIABLES
    [SerializeField] private KeyData.KeyType requiredKeyType;
    
    [Inject] private FirstPersonController player;

    private bool playerHasKey => player.CurrentKey && player.CurrentKey.KeyType == requiredKeyType;
    private bool chestIsOpen;
    
    private Transform chestHandle;
    private float handleAngle = 0;
    
    private Battery batteryItem;

    //PopUp
    private Vector3 popUpPosition => transform.position + Vector3.up / 2;
    private Color popUpColor => KeyData.KeyColors[requiredKeyType];

    #endregion

    #region INTERACTABLE METHODS
    public override void OnFocus()
    {
        if (handleAngle != chestHandle.localEulerAngles.x)
        {
            HUD.HidePopUp();
            return;
        }

        UpdateOutline(true);

        if (playerHasKey && !chestIsOpen)
            HUD.ShowPopUp(popUpPosition, "Open ", requiredKeyType.ToString(), " Chest", popUpColor);

        else if (!chestIsOpen)
            HUD.ShowPopUp(popUpPosition, "You need a ", requiredKeyType.ToString(), " Key", popUpColor);
    }

    public override void OnInteract()
    {
        HUD.HidePopUp();
        if (playerHasKey) OpenChest();
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
    }

    #endregion

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        chestHandle = transform.GetChild(1);
        handleAngle = chestHandle.localEulerAngles.x;

        base.OnEnable();

        CheckIfBatteryIsPresent();
    }

    private void CheckIfBatteryIsPresent()
    {
        if (batteryItem = GetComponentInChildren<Battery>()) batteryItem.GetComponent<Collider>().enabled = false;
    }

    private async void OpenChest()
    {
        while (handleAngle != -120)
        {
            handleAngle = Mathf.Lerp(handleAngle, -120, 0.05f);
            chestHandle.localEulerAngles = new Vector3(handleAngle, 0, 0);

            if (handleAngle < -119f)
            {
                handleAngle = -120;
                chestIsOpen = true;

                if (batteryItem) batteryItem.GetComponent<Collider>().enabled = true;
                GetComponent<Collider>().enabled = false;

                HUD.HidePopUp();
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }
}