using StarterAssets;
using System.Threading.Tasks;
using UnityEngine;

public class Chest : Interactable
{
    #region VARIABLES
    [SerializeField] private KeyData.KeyType key;
    private bool hasKey => player.CurrentKey && player.CurrentKey.Type == key;
    private bool isOpen;
    private Transform handle;
    private float angle = 0;
    private FirstPersonController player;
    private Item item;
    #endregion

    #region Overriden Methods
    public override void OnFocus()
    {
        if (player.CurrentKey) player.CurrentKey.CanDrop = false;

        if (angle != handle.localEulerAngles.x) { HUD.Instance.HidePopUp(); return; }

        if (hasKey && !isOpen) HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2,
            "Open ",
            key.ToString(), " Chest",
            KeyData.KeyColors[key]);

        else if (!isOpen) HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2,
            "You need a ",
            key.ToString(), " Key",
            KeyData.KeyColors[key]);
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
        if (player.CurrentKey) player.CurrentKey.CanDrop = true;

        HUD.Instance.HidePopUp();
    }

    #endregion

    public override void OnEnable()
    {
        handle = transform.GetChild(1);
        angle = handle.localEulerAngles.x;

        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        base.OnEnable();

        item = GetComponentInChildren<Item>();
        if (item) item.GetComponent<Collider>().enabled = false;
    }

    async void Open()
    {
        while (angle != -120)
        {
            angle = Mathf.Lerp(angle, -120, 0.05f);
            handle.localEulerAngles = new Vector3(angle, 0, 0);//handle.localEulerAngles += Vector3.down;

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

    async void Close()
    {
        while (angle != 0)
        {
            angle = Mathf.Lerp(angle, 0, 0.05f);
            handle.localEulerAngles = new Vector3(0, 0, angle);//handle.localEulerAngles += Vector3.down;

            if (angle > -1)
            {
                angle = 0;
                isOpen = false;
            }
            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
