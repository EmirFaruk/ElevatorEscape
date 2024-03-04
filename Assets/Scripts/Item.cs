using UnityEngine;

public class Item : Interactable
{
    public override void OnFocus()
    {
        base.OnFocus();
        HUD.Instance.ShowPopUp(transform.position + Vector3.up / 2, "E to pick up ", "Item", "", Color.white);
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
