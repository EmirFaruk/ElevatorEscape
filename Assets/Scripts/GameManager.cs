using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        LevelCountdownController.OnLevelTimeEnd += OnLevelTimeEnd;
    }

    private void OnDisable()
    {
        LevelCountdownController.OnLevelTimeEnd -= OnLevelTimeEnd;
    }

    private void OnLevelTimeEnd()
    {
        print("Level Time Ended");
        UnityEditor.EditorApplication.isPaused = true;
    }
}
