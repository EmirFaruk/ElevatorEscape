using StarterAssets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Door : Interactable
{
    #region VARIABLES
    [SerializeField] private KeyData.KeyType key;

    private bool hasKey => player.CurrentKey && player.CurrentKey.KeyType == key;
    private bool isOpen;
    private bool canRotate = true;

    private Transform handle;
    private float angle;

    //PopUp    
    private Vector3 popUpPosition => GetComponent<Collider>().bounds.center + Vector3.up / 2;
    private string popUpHueText => key.ToString();
    private Color popUpColor => KeyData.KeyColors[key];

    [Inject]
    private FirstPersonController player;
    #endregion

    #region Interactable Methods
    public override void OnFocus()
    {
        if (hasKey && !isOpen)
            HUD.Instance.ShowPopUp(popUpPosition, "Open ", popUpHueText, " Door", popUpColor);
        else if (!isOpen)
            HUD.Instance.ShowPopUp(popUpPosition, "You need a ", popUpHueText, " Key", popUpColor);
    }

    public override void OnInteract()
    {
        // Close the Door
        if (isOpen && hasKey && canRotate) RotateHandle(handle.localEulerAngles.y - 120);
        // Open the Door
        else if (hasKey && canRotate) RotateHandle(handle.localEulerAngles.y + 120);
        // Locked Door
        else AudioManager.OnSFXCall?.Invoke(SoundData.SoundEnum.LockedDoor);
    }

    public override void OnLoseFocus()
    {
        HUD.Instance.HidePopUp();
    }

    #endregion

    public override void OnEnable()
    {
        handle = transform;
        angle = handle.eulerAngles.y;

        base.OnEnable();
    }

    async void RotateHandle(float targetAngle)
    {
        canRotate = false;

        if (!isOpen)
        {
            AudioManager.OnSFXCall?.Invoke(SoundData.SoundEnum.Unlock);
            await Task.Delay(100);
        }
        AudioManager.OnSFXCall?.Invoke(SoundData.SoundEnum.DoorOpening);

        while (!destroyCancellationToken.IsCancellationRequested && angle != targetAngle)
        {
            angle = Mathf.Lerp(angle, targetAngle, 0.04f);
            handle.localEulerAngles = new Vector3(0, angle, 0);

            if (Mathf.Abs(angle - targetAngle) < 1)
            {
                angle = targetAngle;
                isOpen = !isOpen;
                canRotate = true;
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
