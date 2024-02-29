using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Key : Interactable
{
    public byte ID => id;
    [SerializeField] private byte id;
    
    private FirstPersonController player;
    public Shader shadera;

    private void OnValidate()
    {
      // if(shadera) print("shader : " + shadera);
    }

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
        base.OnEnable();
    }
}
