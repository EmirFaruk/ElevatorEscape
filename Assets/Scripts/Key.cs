using StarterAssets;
using System.Threading.Tasks;
using UnityEngine;

public class KeyItem : Interactable
{
    #region VARIABLES
    public KeyData.KeyType KeyType => keyType;
    [SerializeField] private KeyData.KeyType keyType;

    private FirstPersonController player;
    private bool isPickedUp;
    private bool canDrop = true;

    //PopUp
    private Vector3 popUpPosition => transform.position + Vector3.up / 2;
    private string popUpHueText => keyType.ToString();
    private Color popUpColor => KeyData.KeyColors[keyType];
    #endregion

    #region INTERACTABLE METHODS    

    public override void OnFocus()
    {
        base.OnFocus();

        if (!isPickedUp)
            HUD.Instance.ShowPopUp(popUpPosition, "Pick up ", popUpHueText, " Key", popUpColor);
    }

    public override void OnInteract()
    {
        if (!player.CurrentKey)
        {
            player.HoldKey(this);
            isPickedUp = true;

            //set model object's layer as HandCamera layer    
            transform.GetChild(0).gameObject.layer = 8;

            ToggleKeyLight();
        }
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        HUD.Instance.HidePopUp();
    }

    #endregion

    #region UNITY EVENTS FUNCTIONS

    private void Update()
    {
        if (isPickedUp && canDrop) DropWithoutDelay();
    }

    public async override void OnEnable()
    {
        await Task.Delay(1000);

        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        transform.GetChild(0).GetComponent<Renderer>().materials = KeyData.Instance.KeyMaterials[(int)keyType].materials;

        base.OnEnable();
    }

    #endregion

    // Hold and Drop Key
    private async void Drop()
    {
        float posY = transform.position.y;
        float rotZ = transform.localEulerAngles.z;

        canDrop = false;

        await Task.Delay(300);

        if (Input.GetKey(KeyCode.G))
        {
            for (int i = 0; i < 50 && !destroyCancellationToken.IsCancellationRequested; i++)
            {
                if (!Input.GetKey(KeyCode.G)) break;
                transform.position += Vector3.up * 0.001f;
                transform.Rotate(Vector3.forward * Random.RandomRange(-2f, 2f));
                await Task.Delay(10, destroyCancellationToken);
            }

            if (Input.GetKey(KeyCode.G))
            {
                if (player.CurrentKey) player.DropKey();
                isPickedUp = false;
                transform.GetChild(0).gameObject.layer = default;//set model object's layer as Default layer

                //await Task.Delay(1000);

            }

            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotZ);
            ToggleKeyLight();
            canDrop = true;
        }
        canDrop = true;
    }

    private void DropWithoutDelay()
    {
        float posY = transform.position.y;
        float rotZ = transform.localEulerAngles.z;

        canDrop = false;

        if (Input.GetKey(KeyCode.G))
        {
            if (player.CurrentKey) player.DropKey();
            isPickedUp = false;
            transform.GetChild(0).gameObject.layer = default;//set model object's layer as Default layer

            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotZ);
            ToggleKeyLight();
            canDrop = true;
        }
        canDrop = true;
    }

    private async void ToggleKeyLight()
    {
        var light = GetComponentInChildren<Light>();
        if (light && light.enabled)
        {
            light.enabled = false;
        }
        else if (light)
        {
            await Task.Delay(500);
            light.enabled = true;
        }
    }
}