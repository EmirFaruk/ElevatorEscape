using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private void Awake()
    {
        this.gameObject.layer = 7; // interaction layer;
    }


    public abstract void OnInteract();
    public abstract void OnFocus();

    public abstract void OnLoseFocus();
}
