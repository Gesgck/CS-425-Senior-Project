using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuController : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene(1);
    }
    public void OnQuitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit(); // Quit the application
    }

    public void OnSettingsClick()
    {
        //SceneManager.LoadScene("SettingsScene");
    }
}


public class SettingsMenuController : MonoBehaviour
{
    public void OnBackClick()
    {
        SceneManager.LoadScene(0); // Load the main menu scene
    }
}
