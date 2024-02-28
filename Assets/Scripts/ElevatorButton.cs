using UnityEngine;
using UnityEngine.UI;

public class ElevatorButton : Interactable
{
    [SerializeField] private Elevator elevator;
    [SerializeField] private byte index;

    [Header("Color")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color pressedColor;
    private bool isPressed = false;

    public override void OnFocus()
    {
        if (!isPressed) GetComponent<Image>().color = hoverColor;
    }

    public override void OnInteract()
    {
        elevator.MoveTo((int)index);
    }

    public override void OnLoseFocus()
    {
        if (!isPressed) GetComponent<Image>().color = defaultColor;
    }

    private void OnEnable()
    {
        Elevator.OnReachedStop += SetIsNotPressed;
        Elevator.OnReached += SetAsPressed;
        gameObject.layer = 7;
    }

    private void OnDisable()
    {
        Elevator.OnReached -= SetAsPressed;
        Elevator.OnReachedStop -= SetIsNotPressed;
    }

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
            GetComponent<Image>().color = pressedColor;
        }
    }
}
