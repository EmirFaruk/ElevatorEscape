using StarterAssets;
using System.Threading.Tasks;
using UnityEngine;

public class KeyItem : Interactable
{
    #region VARIABLES

    public KeyData.KeyType Type => keyType;
    [SerializeField] private KeyData.KeyType keyType;

    private FirstPersonController player;
    private bool isPickedUp;
    [HideInInspector] public bool CanDrop = true;

    #endregion

    #region OVERRIDEN METHODS

    public override void OnFocus()
    {
        base.OnFocus();

        if (!isPickedUp)
            HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2
                , "Pick up ",
                keyType.ToString(),
                " Key",
                KeyData.KeyColors[keyType]);
    }

    public override void OnInteract()
    {
        if (!player.CurrentKey)
        {
            player.HoldKey(this);
            isPickedUp = true;
            transform.GetChild(0).gameObject.layer = 8;//set model object's layer as HandCamera layer    

            ToggleKeyLight();
        }
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        HUD.Instance.HidePopUp();
    }
    private void Update()
    {
        if (isPickedUp && CanDrop && !destroyCancellationToken.IsCancellationRequested) DropWithoutDelay();
    }

    public async override void OnEnable()
    {
        if (destroyCancellationToken.IsCancellationRequested) return;
        await Task.Delay(1000, destroyCancellationToken);

        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        transform.GetChild(0).GetComponent<Renderer>().materials = KeyData.Instance.KeyMaterials[(int)keyType].materials;

        base.OnEnable();

        //interactableRenderer = GetComponentInChildren<Renderer>();
        //materialHandler = interactableRenderer.materials.ToList();
    }

    #endregion

    async void Drop()
    {
        float posY = transform.position.y;
        float rotZ = transform.localEulerAngles.z;

        CanDrop = false;

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
            CanDrop = true;
        }
        CanDrop = true;
    }

    void DropWithoutDelay()
    {
        float posY = transform.position.y;
        float rotZ = transform.localEulerAngles.z;

        CanDrop = false;

        if (Input.GetKey(KeyCode.G))
        {
            if (player.CurrentKey) player.DropKey();
            isPickedUp = false;
            transform.GetChild(0).gameObject.layer = default;//set model object's layer as Default layer

            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotZ);
            ToggleKeyLight();
            CanDrop = true;
        }
        CanDrop = true;
    }

    async void ToggleKeyLight()
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