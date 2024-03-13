using UnityEngine;

public class Battery : Interactable
{
    public override void OnFocus()
    {
        base.OnFocus();
        HUD.ShowPopUp(transform.position + Vector3.up / 3, "Pick up ", "Battery", "", Color.white);
    }

    public override void OnInteract()
    {
        HUD.HidePopUp();
        HUD.SetItemAmount(1);
        Destroy(gameObject);
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        HUD.HidePopUp();
    }
}
