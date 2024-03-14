using UnityEngine;

public class Battery : Interactable
{
    private const string messageBase = "Pick up ";
    private const string hue = "Battery";
    private const string end = "";
    private static readonly Color hueColor = Color.white;

    /*public Battery(Vector3 popUpPosition, string popUpBase, string popUpHue, string popUpEnd, Color hueColor)
        : base(popUpPosition, popUpBase, popUpHue, popUpEnd, hueColor) { }*/

    public override void Awake()
    {
        base.Awake();
        Init(transform.position + Vector3.up / 3, messageBase, hue, end, hueColor);
    }

    public override void OnFocus()
    {
        base.OnFocus();
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
    }
}
