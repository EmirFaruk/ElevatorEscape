using StarterAssets;
using System.Threading.Tasks;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private KeyData.KeyType key;
    private bool hasKey => player.CurrentKey && player.CurrentKey.Type == key;
    private bool isOpen;
    private Transform handle;
    private float angle = 0;
    private float defaultAngleY = 0;
    private FirstPersonController player;

    #region Overriden Methods
    public override void OnFocus()
    {
        if (player.CurrentKey) player.CurrentKey.CanDropping = false;

        if (hasKey && !isOpen) HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2 + Vector3.forward,
            "E to open",
            key.ToString(), " Door",
            KeyData.KeyColors[key]);

        else if (!isOpen) HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2 + Vector3.forward,
            "You need a ",
            key.ToString(), " Key",
            KeyData.KeyColors[key]);
    }

    public override void OnInteract()
    {
        if (isOpen) Close();
        else if (hasKey) Open();
    }

    public override void OnLoseFocus()
    {
        if (player.CurrentKey) player.CurrentKey.CanDropping = true;

        HUD.Instance.HidePopUp();
    }

    #endregion

    public override void OnEnable()
    {
        handle = transform;//.GetChild(0);
        angle = defaultAngleY = handle.localEulerAngles.y;
        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        base.OnEnable();
    }

    async void Open()
    {
        while (!destroyCancellationToken.IsCancellationRequested && angle != defaultAngleY + 120)
        {
            angle = Mathf.Lerp(angle, defaultAngleY + 120, 0.05f);
            handle.localEulerAngles = new Vector3(0, angle, 0);//handle.localEulerAngles += Vector3.down;

            if (angle > defaultAngleY + 119)
            {
                angle = defaultAngleY + 120;
                isOpen = true;
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }

    async void Close()
    {
        while (!destroyCancellationToken.IsCancellationRequested && angle != defaultAngleY)
        {
            angle = Mathf.Lerp(angle, defaultAngleY, 0.05f);
            handle.localEulerAngles = new Vector3(0, angle, 0);//handle.localEulerAngles += Vector3.down;

            if (Mathf.Abs(angle - defaultAngleY) < 1)
            {
                angle = defaultAngleY;
                isOpen = false;
            }
            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
