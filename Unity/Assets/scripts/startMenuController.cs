using UnityEngine;
using UnityEngine.SceneManagement;
public class startMenuController : MonoBehaviour
{
    public void onStartClick()
    {
       // SceneManagement.LoadScene("MainGameScene");
    }

    public void onQuitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit(); // Quit the application
    }

    public void onSettingsClick()
    {
        //SceneManagement.LoadScene("SettingsScene");
    }
}
