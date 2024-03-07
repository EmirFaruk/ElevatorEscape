using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ExitDoor : Interactable
{
    [SerializeField] private Transform itemsParent;
    [SerializeField] private Material itemMaterial;
    private readonly List<Transform> items = new();
    private Collider doorCollider => GetComponent<Collider>();

    private void Start()
    {
        foreach (Transform item in itemsParent) items.Add(item);

        handle = transform;
        angle = defaultAngleY = handle.transform.localEulerAngles.y;
    }

    public override void OnFocus()
    {
        HUD.Instance.ShowPopUp(doorCollider.bounds.center + Vector3.up / 2, "Collect ", "Items", " to Exit", Color.cyan);
    }

    public override void OnInteract()
    {
        if (HUD.Instance.GetItemAmount > 0 && items.Count > 0)
        {
            HUD.Instance.HidePopUp();

            //Item materyalini degistir
            items.Last().GetComponent<MeshRenderer>().material = itemMaterial;

            //Itemi listeden kaldir
            items.Remove(items.Last());

            //Item sayisini azalt
            HUD.Instance.SetItemAmount(-1);

            if (items.Count <= 0) OpenDoor();
        }
    }

    public override void OnLoseFocus()
    {
        HUD.Instance.HidePopUp();
    }

    float angle = 0;
    float defaultAngleY = 0;
    private Transform handle;

    private async void OpenDoor()
    {
        await Task.Delay(1000);

        while (!destroyCancellationToken.IsCancellationRequested && angle != defaultAngleY + 120)
        {
            angle = Mathf.Lerp(angle, defaultAngleY + 120, 0.05f);
            handle.localEulerAngles = new Vector3(0, angle, 0);//handle.localEulerAngles += Vector3.down;

            if (angle > defaultAngleY + 119)
            {
                angle = defaultAngleY + 120;
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
