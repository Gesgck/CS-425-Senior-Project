using UnityEngine;
using UnityEngine.SceneManagement;
public class startMenuController : MonoBehaviour
{
    public void onPlayClick()
    {
        SceneManager.LoadScene("SampleScene");
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
        //SceneManager.LoadScene("SettingsScene");
    }
}
