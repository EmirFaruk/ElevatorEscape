using StarterAssets;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

public class Elevator : MonoBehaviour
{
    #region FIELDS
    public static Action<int> OnReachedStop;
    public static Action<int> OnReached;

    [SerializeField] private float speed = 3f;

    [Header("Stop")]
    [SerializeField] private Transform stopsParent;
    [SerializeField] private Transform[] stops;

    [Header("Door")]
    [SerializeField] private float doorSpeed = 1f;
    [SerializeField] private float doorOffset = .75f;
    [SerializeField] private Transform[] doors;

    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI tmpFloor;

    private bool hasReachedStop = true;
    private bool doorIsOpen = false;
    private AudioSource audioSource;

    [Header("Color")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color pressedColor;    

    bool buttonPressed = false;

    private bool timeReported = false;

    [Inject] ZenjectGetter zenjectGetter;
    #endregion

    #region PROPERTIES
    public Color DefaultColor => defaultColor;
    public Color HoverColor => hoverColor;
    public Color PressedColor => pressedColor;
    #endregion

    #region UNITY EVENT FUNCTIONS

    void Start()
    {
        // Set Stops with Stops Parent
        stops = new Transform[stopsParent.childCount];
        if (stopsParent != null)
            for (byte i = 0; i < stopsParent.childCount; i++) stops[i] = stopsParent.GetChild(i);

        audioSource = gameObject.AddComponent<AudioSource>();
        
        tmpFloor.text = "0";

        MoveDoor(false);
    }

    private void OnEnable()
    {
        LevelCountdownController.OnLevelTimeReloadStart += () => timeReported = true;
        LevelCountdownController.OnLevelTimeReloadEnd += () => timeReported = false;
    }

    private void OnDisable()
    {

    }

    #endregion


    public async void MoveTo(int stopIndex, bool isCallButton)
    {
        if (stopIndex < 0 || stopIndex >= stops.Length)
        {
            Debug.LogError("Invalid stop index");
            return;
        }

        //Hareket halindeyse ve kapilar acilmamissa ve zaman yeniden doluyorsa
        if (buttonPressed || timeReported)
        {
            PlayButtonClickSound(false);
            return;
        }

        //Hedef pozisyonu belirle
        Vector3 targetPosition = transform.position;
        targetPosition.y = stops[stopIndex].position.y;

        //Bulundugu kat ise
        if (hasReachedStop = transform.position.y == targetPosition.y)
        {
            PlayButtonClickSound(false);
            buttonPressed = false;
            return;
        }

        OnReached?.Invoke(stopIndex);

        //Buton sesi oynat
        PlayButtonClickSound(true);
        buttonPressed = true;

        //Player'i asansore al yoksa duser ya da titrer (player kodunun icinde olacak burasi)
        if (!isCallButton)
        {
            zenjectGetter.FirstPersonController.transform.parent = transform;
            zenjectGetter.FirstPersonController.GetComponent<CharacterController>().enabled = false;
        }

        //Biraz bekle
        await Task.Delay(700, destroyCancellationToken);

        //Kapiyi kapat
        MoveDoor(true);

        //Kapanin kapanmasini bekle
        await Task.Delay(1700);

        doorIsOpen = false;

        
        AudioManager.OnAudioSourceSet?.Invoke(audioSource, SoundData.SoundEnum.ElevatorMovement);
        //audioSource.PlayOneShot(movementSound);

        //Asansoru hareket ettir
        while (!destroyCancellationToken.IsCancellationRequested && !hasReachedStop)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            hasReachedStop = transform.position.y == targetPosition.y;

            for (int i = stopIndex; i > 0; i--) 
            {
                if (stops[i].position.y <= transform.position.y)
                {
                    tmpFloor.text = i.ToString();
                    break;
                }
            }

            if (hasReachedStop)
            {
                //Player'i serbest birak (player kodunun icinde olacak burasi)
                if (!isCallButton)
                {
                    zenjectGetter.FirstPersonController.transform.parent = null;
                    zenjectGetter.FirstPersonController.GetComponent<CharacterController>().enabled = true;
                }

                audioSource.Stop();
                tmpFloor.text = stopIndex.ToString();

                //Kapiyi ac
                MoveDoor(false);

                await Task.Delay(1000);

                doorIsOpen = true;
                buttonPressed = false;
                OnReachedStop?.Invoke(stopIndex);
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }

    public void PlayButtonClickSound(bool isActive)
    {
        AudioManager.OnSFXCall(isActive ? SoundData.SoundEnum.ElevatorButtonPressedActive : SoundData.SoundEnum.ElevatorButtonPressedPassive);
    }

    async void MoveDoor(bool isOpen)
    {
        Vector3 targetPositionDoor1 = doors[0].position;
        Vector3 targetPostionDoor2 = doors[1].position;
        targetPositionDoor1.x += isOpen ? doorOffset : -doorOffset;
        targetPostionDoor2.x += isOpen ? -doorOffset : doorOffset;
        Vector3 direction = isOpen ? Vector3.right : Vector3.left;

        AudioManager.OnSFXCall(SoundData.SoundEnum.ElevatorDoorMovement);

        while (!destroyCancellationToken.IsCancellationRequested && Mathf.Abs(doors[0].transform.position.x - targetPositionDoor1.x) > .1f)
        {
            doors[0].transform.position += doorSpeed * direction * Time.deltaTime;//Vector3.MoveTowards(door.transform.position, targetPosition, speed * Time.deltaTime);
            doors[1].transform.position += doorSpeed * -direction * Time.deltaTime;

            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
