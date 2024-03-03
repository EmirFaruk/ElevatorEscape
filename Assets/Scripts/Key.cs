using StarterAssets;
using System.Linq;
using UnityEngine;

public class Key : Interactable
{
    public KeyType Type => keyType;
    [SerializeField] private KeyType keyType;

    private FirstPersonController player;
    private bool isPickedUp;
    [SerializeField] private Color color;

    public override void OnFocus()
    {
        base.OnFocus();

        if (!isPickedUp)
            HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2
                , "E to pick up ",
                keyType.ToString(),
                " Key",
                color);
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
            player.DropKey();
            isPickedUp = false;
        }
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        HUD.Instance.HidePopUp();
    }

    public override void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        interactableRenderer = GetComponentInChildren<Renderer>();
        materialHandler = interactableRenderer.materials.ToList();
    }

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
