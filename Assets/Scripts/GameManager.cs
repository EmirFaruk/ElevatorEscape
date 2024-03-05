using UnityEngine;
using UnityEditor;

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

#if UNITY_EDITOR
        EditorApplication.isPaused = true;
#else
        Time.timeScale = 0.0f;
#endif
    }
}
