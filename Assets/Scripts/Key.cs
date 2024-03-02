using StarterAssets;
using System.Linq;
using UnityEngine;

public class Key : Interactable
{
    public byte ID => id;
    [SerializeField] private byte id;

    private FirstPersonController player;

    public override void OnFocus()
    {
        base.OnFocus();
    }

    public override void OnInteract()
    {
        if (!player.CurrentKey) player.HoldKey(this);
        else player.DropKey();
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
    }

    public override void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        interactableRenderer = GetComponentInChildren<Renderer>();
        materialHandler = interactableRenderer.materials.ToList();
    }
}
