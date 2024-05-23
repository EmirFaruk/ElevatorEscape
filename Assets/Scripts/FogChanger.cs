using UnityEngine;

public class FogChanger : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Color toxicAirColor;
    [SerializeField]
    private Color basementAirColor;
    // Update is called once per frame

    [SerializeField]
    private float transitionSpeed = .5f; // Geçiþ hýzý
    private Color targetColor;

    private void Update()
    {
        if (player.transform.position.y <= 1.5f)
            targetColor = basementAirColor;
        else targetColor = toxicAirColor;

        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetColor, transitionSpeed * Time.deltaTime);
    }

}
