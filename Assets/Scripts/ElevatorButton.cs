using UnityEngine;
using UnityEngine.UI;

public class ElevatorButton : Interactable
{
    #region FIELDS

    [SerializeField] private byte index;
    private Elevator elevator;

    private bool isPressed = false;

    private bool isCallButton;

    #endregion

    #region INTERACTABLE OVERRIDE METHODS
    public override void OnFocus()
    {
        if (!isPressed) GetComponent<Image>().color = elevator.HoverColor;
    }

    public override void OnInteract()
    {
        elevator.MoveTo((int)index, isCallButton);
    }

    public override void OnLoseFocus()
    {
        if (!isPressed) GetComponent<Image>().color = elevator.DefaultColor;
    }
    #endregion

    private void OnEnable()
    {
        isCallButton = GetComponentInParent<Elevator>() == null;
        elevator = GetComponentInParent<Elevator>()?? GameObject.FindWithTag("Elevator").GetComponent<Elevator>();
        Elevator.OnReachedStop += SetIsNotPressed;
        Elevator.OnReached += SetAsPressed;

        gameObject.layer = 7;

        GetComponent<Image>().color = elevator.DefaultColor;
    }

    private void OnDisable()
    {
        Elevator.OnReached -= SetAsPressed;
        Elevator.OnReachedStop -= SetIsNotPressed;
    }

    #region METHODS

    void SetIsNotPressed(int index)
    {
        if (this.index == index)
        {
            isPressed = false;
            OnLoseFocus();
        }
    }

    void SetAsPressed(int index)
    {
        if (this.index == index)
        {
            isPressed = true;
            GetComponent<Image>().color = elevator.PressedColor;
        }
    }

    #endregion
}
