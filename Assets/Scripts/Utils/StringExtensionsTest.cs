using TMPro;
using UnityEngine;

public static class StringExtensionsTest
{
    public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
    public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
}
public class MyScript : MonoBehaviour
{
    TextMeshProUGUI SomeTMProText;

    void Start()
    {
        SomeTMProText.SetText($"" +
            $"{"H".AddColor(Color.red)}" +
            $"{"E".AddColor(Color.blue)}" +
            $"{"L".AddColor(Color.green)}" +
            $"{"L".AddColor(Color.white)}" +
            $"{"O".AddColor(Color.yellow)}");
    }
}