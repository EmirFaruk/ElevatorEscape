using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    [Inject]
    ZenjectGetter zenjectGetter;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) zenjectGetter.firstPersonController.DoSomething();
        if (Input.GetKeyDown(KeyCode.R)) zenjectGetter.gameManager.RestartLevel();
    }
}
