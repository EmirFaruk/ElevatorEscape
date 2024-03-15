using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ExitDoor : Interactable
{
    #region VARIABLES
    [SerializeField] private Transform door;
    [SerializeField] private Transform batteryItemsGateParent;
    [SerializeField] private Transform batteryItemsPowerBoxParent;
    
    private readonly List<Transform> gateBatteryItems = new();
    private readonly List<Transform> powerBoxBatteryItems = new();

    private float angle = 0;
    private float defaultAngleY;
    private Transform[] handle = new Transform[2];

    private Collider doorCollider => GetComponent<Collider>();
    #endregion

    private void Start()
    {
        SetBatteryItems(batteryItemsGateParent, gateBatteryItems);
        SetBatteryItems(batteryItemsPowerBoxParent, powerBoxBatteryItems);

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
        if (HUD.GetItemAmount > 0 && gateBatteryItems.Count > 0)
        {
            HUD.HidePopUp();

            //Item objesini aktif et
            gateBatteryItems.First().gameObject.SetActive(true);
            powerBoxBatteryItems.First().gameObject.SetActive(true);

            //Itemi listeden kaldir
            gateBatteryItems.Remove(gateBatteryItems.First());
            powerBoxBatteryItems.Remove(powerBoxBatteryItems.First());

            //Item sayisini azalt
            HUD.SetItemAmount(-1);

            if (gateBatteryItems.Count <= 0) OpenDoor();
        }
    }

    public override void OnLoseFocus()
    {
        HUD.HidePopUp();
    }    

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
        
        GameManager.OnWin?.Invoke();        
    }

    void SetBatteryItems(Transform target, List<Transform> itemList)
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
