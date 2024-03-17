using StarterAssets;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Door : Interactable
{
    #region VARIABLES
    [SerializeField] private KeyData.KeyType key;
    [Inject] private FirstPersonController player;
    private Transform handle;
    private float angle;
    private bool isOpen;
    private bool canRotate = true;

    private bool playerHasKey => player.CurrentKey && player.CurrentKey.KeyType == key;


    //PopUp    
    private Vector3 popUpPosition => GetComponent<Collider>().bounds.center + Vector3.up / 2;
    private string popUpHueText => key.ToString();
    private Color popUpColor => KeyData.KeyColors[key];
    #endregion

    #region Interactable Methods
    public override void OnFocus()
    {
        if (playerHasKey && !isOpen)
            HUD.ShowPopUp(popUpPosition, "Open ", popUpHueText, " Door", popUpColor);

        else if (!isOpen)
            HUD.ShowPopUp(popUpPosition, "You need a ", popUpHueText, " Key", popUpColor);
    }

    public override void OnInteract()
    {
        // Close the Door
        if (isOpen && playerHasKey && canRotate) RotateHandle(handle.localEulerAngles.y - 120);

        // Open the Door
        else if (playerHasKey && canRotate) RotateHandle(handle.localEulerAngles.y + 120);

        // Locked Door
        else AudioManager.OnSFXCall?.Invoke(SoundData.SoundEnum.LockedDoor);
    }

    public override void OnLoseFocus()
    {
        HUD.HidePopUp();
    }

    #endregion

    public override void OnEnable()
    {
        handle = transform;
        angle = handle.localEulerAngles.y;

        base.OnEnable();
    }

    private async Task PlayDoorOpenSounds()
    {
        if (!isOpen)
        {
            AudioManager.OnSFXCall?.Invoke(SoundData.SoundEnum.Unlock);
            await Task.Delay(400);
        }
        AudioManager.OnSFXCall?.Invoke(SoundData.SoundEnum.DoorOpening);
    }

    async void RotateHandle(float targetAngle)
    {
        canRotate = false;

        await PlayDoorOpenSounds();

        while (!destroyCancellationToken.IsCancellationRequested)
        {
            angle = Mathf.Lerp(angle, targetAngle, 0.04f);
            handle.localEulerAngles = new Vector3(0, angle, 0);

            if (Mathf.Abs(angle - targetAngle) < 1)
            {
                angle = targetAngle;
                isOpen = !isOpen;
                canRotate = true;
                return;
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
