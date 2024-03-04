using StarterAssets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Key : Interactable
{
    #region VARIABLES

    public Color KeyColor;
    public KeyData.KeyType Type => keyType;
    [SerializeField] private KeyData.KeyType keyType;

    private FirstPersonController player;
    private bool isPickedUp;
    [HideInInspector] public bool CanDropping = true;

    #endregion

    #region OVERRIDEN METHODS

    public override void OnFocus()
    {
        base.OnFocus();

        if (!isPickedUp)
            HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2
                , "E to pick up ",
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
        }
        else
        {
            //player.DropKey();
            //isPickedUp = false;
        }
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        HUD.Instance.HidePopUp();
    }
    private void Update()
    {
        if (isPickedUp && CanDropping) Drop();
    }

    public override void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        interactableRenderer = GetComponentInChildren<Renderer>();
        materialHandler = interactableRenderer.materials.ToList();
    }

    #endregion

    async void Drop()
    {
        float posY = transform.position.y;
        float rotZ = transform.localEulerAngles.z;

        CanDropping = false;

        await Task.Delay(300, destroyCancellationToken);

        if (Input.GetKey(KeyCode.E))
        {
            for (int i = 0; i < 50; i++)
            {
                if (!Input.GetKey(KeyCode.E)) break;
                transform.position += Vector3.up * 0.001f;
                transform.Rotate(Vector3.forward * Random.RandomRange(-2f, 2f));
                await Task.Delay(10, destroyCancellationToken);
            }
            if (Input.GetKey(KeyCode.E))
            {
                if (player.CurrentKey) player.DropKey();
                isPickedUp = false;
            }

            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotZ);
            CanDropping = true;
        }
        CanDropping = true;
    }
}

public class KeyData
{
    public static Dictionary<KeyType, Color> KeyColors = new Dictionary<KeyType, Color>()
    {
        {KeyType.Red, Color.red },
        {KeyType.Green, Color.green },
        {KeyType.Blue, Color.blue },
        {KeyType.Purple, new Color(0.5f, 0, 0.5f) },
        {KeyType.Yellow, Color.yellow },
        {KeyType.Orange, new Color(1, 0.5f, 0) },
        {KeyType.White, Color.white },
        {KeyType.Black, Color.black },
        {KeyType.Brown, new Color(0.5f, 0.25f, 0) },
        {KeyType.Pink, new Color(1, 0.5f, 0.5f) },
        {KeyType.Cyan, Color.cyan },
        {KeyType.Lime, new Color(0.5f, 1, 0.5f) },
        {KeyType.Maroon, new Color(0.5f, 0, 0) },
        {KeyType.Olive, new Color(0.5f, 0.5f, 0) },
        {KeyType.Teal, new Color(0, 0.5f, 0.5f) },
        {KeyType.Navy, new Color(0, 0, 0.5f) },
        {KeyType.Beige, new Color(0.5f, 0.5f, 0.25f) },
        {KeyType.Peach, new Color(1, 0.5f, 0.25f) },
        {KeyType.Lavender, new Color(0.5f, 0.25f, 0.5f) },
        {KeyType.Coral, new Color(1, 0.5f, 0.25f) },
        {KeyType.Mint, new Color(0.5f, 1, 0.5f) },
        {KeyType.Indigo, new Color(0.25f, 0, 0.5f) },
        {KeyType.Amber, new Color(1, 0.75f, 0) },
        {KeyType.Azure, new Color(0, 0.5f, 1) },
        {KeyType.Lilac, new Color(0.5f, 0, 0.5f) },
        {KeyType.Gold, new Color(1, 0.85f, 0) },
        {KeyType.Silver, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Bronze, new Color(0.8f, 0.5f, 0.2f) },
        {KeyType.Platinum, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Diamond, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Ruby, new Color(0.75f, 0.25f, 0.25f) },
        {KeyType.Sapphire, new Color(0.25f, 0.25f, 0.75f) },
        {KeyType.Emerald, new Color(0.25f, 0.75f, 0.25f) },
        {KeyType.Topaz, new Color(0.75f, 0.75f, 0.25f) },
        {KeyType.Amethyst, new Color(0.5f, 0, 0.5f) },
        {KeyType.Opal, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Pearl, new Color(0.75f, 0.75f, 0.75f) },
        {KeyType.Garnet, new Color(0.75f, 0.25f, 0.25f) },
    };

    public enum KeyType
    {
        Red,
        Blue,
        Green,
        Purple,
        Yellow,
        Orange,
        White,
        Black,
        Brown,
        Pink,
        Cyan,
        Lime,
        Maroon,
        Olive,
        Teal,
        Navy,
        Beige,
        Peach,
        Lavender,
        Coral,
        Mint,
        Indigo,
        Amber,
        Azure,
        Lilac,
        Gold,
        Silver,
        Bronze,
        Platinum,
        Diamond,
        Ruby,
        Sapphire,
        Emerald,
        Topaz,
        Amethyst,
        Opal,
        Pearl,
        Garnet,
        Agate,
        Aquamarine,
        Peridot,
        Citrine,
        Onyx,
        Jasper,
        Malachite,
        LapisLazuli,
        Turquoise,
        Moonstone,
        Sunstone,
        Bloodstone,
    }
}