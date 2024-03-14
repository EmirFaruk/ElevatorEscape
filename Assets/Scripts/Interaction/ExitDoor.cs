using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ExitDoor : Interactable
{
    [SerializeField] private Transform door;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private Transform itemsParent2;
    //[SerializeField] private Material itemMaterial;
    private readonly List<Transform> items = new();
    private readonly List<Transform> items2 = new();
    private Collider doorCollider => GetComponent<Collider>();
    public static Action OnWin;

    private void Start()
    {
        FindChild(itemsParent, items);
        FindChild(itemsParent2, items2);

        handle[0] = door.GetChild(0);
        handle[1] = door.GetChild(1);
        angle = defaultAngleY = handle[0].transform.localEulerAngles.y;
    }

    public override void OnFocus()
    {
        HUD.ShowPopUp(doorCollider.bounds.center + Vector3.up / 2, "Collect ", "Batterries", " to Exit", Color.cyan);
    }

    public override void OnInteract()
    {
        if (HUD.GetItemAmount > 0 && items.Count > 0)
        {
            HUD.HidePopUp();

            //Item materyalini degistir
            //items.Last().GetComponent<MeshRenderer>().material = itemMaterial;

            //Item objesini aktif et
            items.First().gameObject.SetActive(true);
            items2.First().gameObject.SetActive(true);

            //Itemi listeden kaldir
            items.Remove(items.First());
            items2.Remove(items2.First());

            //Item sayisini azalt
            HUD.SetItemAmount(-1);

            if (items.Count <= 0) OpenDoor();
        }
    }

    public override void OnLoseFocus()
    {
        HUD.HidePopUp();
    }

    float angle = 0;
    float defaultAngleY;
    private Transform[] handle = new Transform[2];

    private async void OpenDoor()
    {
        await Task.Delay(1000);

        while (!destroyCancellationToken.IsCancellationRequested && angle != defaultAngleY + 120)
        {
            angle = Mathf.Lerp(angle, defaultAngleY + 120, 0.05f);
            handle[0].localEulerAngles = new Vector3(0, -angle, 0);
            handle[1].localEulerAngles = new Vector3(0, angle + 180, 0);

            if (angle > defaultAngleY + 119)
            {
                angle = defaultAngleY + 120;
            }

            await Task.Delay(10);
        }

        await Task.Delay(1000);
        OnWin?.Invoke();
    }

    void FindChild(Transform target, List<Transform> itemList)
    {
        foreach (Transform item in target)
        {
            if (item.gameObject.activeInHierarchy)
            {
                item.GetChild(0).gameObject.SetActive(false);
                itemList.Add(item.GetChild(0));
            }
        }
    }
}
