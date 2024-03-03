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
    private FirstPersonController player;

    #region Overriden Methods
    public override void OnFocus()
    {
        if (hasKey && !isOpen) HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2 + Vector3.left,
            "E to open",
            key.ToString(), " Door",
            KeyData.KeyColors[key]);

        else if (!isOpen) HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2 + Vector3.left,
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
        HUD.Instance.HidePopUp();
    }

    #endregion

    public override void OnEnable()
    {
        handle = transform;//.GetChild(0);
        handle.localEulerAngles = Vector3.zero;
        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        base.OnEnable();
    }

    async void Open()
    {
        while (angle != -120)
        {
            angle = Mathf.Lerp(angle, -120, 0.05f);
            handle.localEulerAngles = new Vector3(0, angle, 0);//handle.localEulerAngles += Vector3.down;

            if (angle < -119f)
            {
                angle = -120;
                isOpen = true;
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }

    async void Close()
    {
        while (angle != 0)
        {
            angle = Mathf.Lerp(angle, 0, 0.05f);
            handle.localEulerAngles = new Vector3(0, angle, 0);//handle.localEulerAngles += Vector3.down;

            if (angle > -1)
            {
                angle = 0;
                isOpen = false;
            }
            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
