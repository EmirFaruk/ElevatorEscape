using StarterAssets;
using System;
using System.Threading.Tasks;
using UnityEngine;

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

    [Header("Auido")]
    [SerializeField] private AudioClip buttonClickSoundActive;
    [SerializeField] private AudioClip buttonClickSoundPassive;

    private AudioSource audioSource;

    private bool hasReachedStop = true;
    private bool doorIsOpen = false;


    [Header("Color")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color pressedColor;

    private FirstPersonController player;
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

        player = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();

        MoveDoor(false);
    }

    #endregion

    public async void MoveTo(int stopIndex)
    {
        if (stopIndex < 0 || stopIndex >= stops.Length)
        {
            Debug.LogError("Invalid stop index");
            return;
        }

        //Hareket halindeyse
        if (!hasReachedStop)
        {
            PlayButtonClickSound(hasReachedStop);
            return;
        }

        //Hedef pozisyonu belirle
        Vector3 targetPosition = transform.position;
        targetPosition.y = stops[stopIndex].position.y;

        //Bulundugu kat ise
        if (hasReachedStop = transform.position.y == targetPosition.y)
        {
            PlayButtonClickSound(false);
            return;
        }

        OnReached?.Invoke(stopIndex);

        //Buton sesi oynat
        PlayButtonClickSound(true);

        //Kapiyi kapat
        await Task.Delay(700, destroyCancellationToken);
        MoveDoor(true);

        //Kapanin kapanmasini bekle
        await Task.Delay(1700, destroyCancellationToken);
        doorIsOpen = false;

        //Player'i asansore al yoksa duser ya da titrer (player kodunun icinde olacak burasi)
        player.transform.parent = transform;
        player.GetComponent<CharacterController>().enabled = false;

        //Asansoru hareket ettir
        while (!hasReachedStop)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            hasReachedStop = transform.position.y == targetPosition.y;

            if (hasReachedStop)
            {
                //Kapiyi ac
                MoveDoor(false);

                await Task.Delay(1000, destroyCancellationToken);

                doorIsOpen = true;

                OnReachedStop?.Invoke(stopIndex);

                //Player'i serbest birak (player kodunun icinde olacak burasi)
                player.transform.parent = null;
                player.GetComponent<CharacterController>().enabled = true;
            }

            await Task.Delay(10, destroyCancellationToken);
        }
    }

    public void PlayButtonClickSound(bool isActive)
    {
        audioSource.PlayOneShot(isActive ? buttonClickSoundActive : buttonClickSoundPassive);
    }

    async void MoveDoor(bool isOpen)
    {                        
        Vector3 targetPositionDoor1 = doors[0].position;
        Vector3 targetPostionDoor2 = doors[1].position;
        targetPositionDoor1.x += isOpen ? doorOffset : -doorOffset;
        targetPostionDoor2.x += isOpen ? -doorOffset : doorOffset;
        Vector3 direction = isOpen ? Vector3.right : Vector3.left;


        while (Mathf.Abs(doors[0].transform.position.x - targetPositionDoor1.x) > .1f)
        {
            doors[0].transform.position += doorSpeed * direction * Time.deltaTime;//Vector3.MoveTowards(door.transform.position, targetPosition, speed * Time.deltaTime);
            doors[1].transform.position += doorSpeed * -direction * Time.deltaTime;

            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
