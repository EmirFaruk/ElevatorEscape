using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    public byte ID => id;
    [SerializeField] private byte id;
    
    private FirstPersonController player;

    public override void OnFocus()
    {
        
    }

    public override void OnInteract()
    {
        if (!player.CurrentKey) player.HoldKey(this);
        else player.DropKey();
    }

    public override void OnLoseFocus()
    {
        
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
    }
}
