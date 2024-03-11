using UnityEngine;

public class Item : Interactable
{
    public override void OnFocus()
    {
        base.OnFocus();
        HUD.Instance.ShowPopUp(transform.position + Vector3.up / 3, "Pick up ", "Battery", "", Color.white);
    }

    public override void OnInteract()
    {
        HUD.Instance.HidePopUp();
        HUD.Instance.SetItemAmount(1);
        Destroy(gameObject);
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        HUD.Instance.HidePopUp();
    }
}
