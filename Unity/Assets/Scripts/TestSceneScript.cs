using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneScript: MonoBehaviour
{
    public void ReturnToPreviousScene()
    {
        // If GameManager recorded a previous scene, go back to it
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.previousSceneName))
        {
            SceneManager.LoadScene(GameManager.Instance.previousSceneName);
        }
        else
        {
            // Fallback default scene if launched directly
            SceneManager.LoadScene("MapScene");
        }
    }
}