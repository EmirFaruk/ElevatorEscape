using System.Threading.Tasks;
using UnityEngine;

public class Door : Interactable
{
    private bool hasKey;
    private bool isOpen;
    private Transform handle;
    private float angle = 0;

    #region Overriden Methods

    public override void OnFocus()
    {
        print("foc");
    }

    public override void OnInteract()
    {
        if (hasKey) Open();
        if (!isOpen) Open();
        else Close();
    }

    public override void OnLoseFocus()
    {

    }

    #endregion

    private void OnEnable()
    {
        handle = transform;//.GetChild(0);
        handle.localEulerAngles = Vector3.zero;
    }

    async void Open()
    {
        while (angle != -120)
        {
            angle = Mathf.Lerp(angle, -120, 0.05f);
            handle.localEulerAngles = new Vector3(0, angle, 0);//handle.localEulerAngles += Vector3.down;

            if (angle < -119f)
            {
                angle = -120;
                isOpen = true;
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }

    async void Close()
    {
        print("close");
        while (angle != 0)
        {
            angle = Mathf.Lerp(angle, 0, 0.05f);
            handle.localEulerAngles = new Vector3(0, angle, 0);//handle.localEulerAngles += Vector3.down;

            if (angle > -1)
            {
                angle = 0;
                isOpen = false;
            }
            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
