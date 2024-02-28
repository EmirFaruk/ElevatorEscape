using System.Threading.Tasks;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private Transform[] stops;
    [SerializeField] private Transform stopsParent;
    [SerializeField] private Transform door;
    [SerializeField] private float speed = 1f;

    [Header("Auido")]
    [SerializeField] private AudioClip buttonClickSoundActive;
    [SerializeField] private AudioClip buttonClickSoundPassive;

    private AudioSource audioSource;

    private bool hasReachedStop = true;
    private bool doorIsOpen = false;

    #endregion

    void Start()
    {
        if (stopsParent != null)
            for (byte i = 0; i < stopsParent.childCount; i++) stops[i] = stopsParent.GetChild(i);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

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

        //Buton sesi oynat
        PlayButtonClickSound(true);

        //Kapiyi kapat
        MoveDoor(true);

        //Kapanin kapanmasini bekle
        await Task.Delay(1000, destroyCancellationToken);
        doorIsOpen = false;

        //Player'i asansore al yoksa duser ya da titrer (player kodunun icinde olacak burasi)
        GameObject.FindWithTag("Player").transform.parent = transform;
        GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = false;

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

                //Player'i serbest birak (player kodunun icinde olacak burasi)
                GameObject.FindWithTag("Player").transform.parent = null;
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = true;
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
        Vector3 targetPosition = door.position;
        targetPosition.x += isOpen ? 1.25f : -1.25f;
        Vector3 direction = isOpen ? Vector3.right : Vector3.left;

        while (Mathf.Abs(door.transform.position.x - targetPosition.x) > .1f)
        {
            door.transform.position += speed * direction * Time.deltaTime;//Vector3.MoveTowards(door.transform.position, targetPosition, speed * Time.deltaTime);

            await Task.Delay(10, destroyCancellationToken);
        }
    }
}
