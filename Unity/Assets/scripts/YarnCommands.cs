using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class YarnSceneCommands : MonoBehaviour
{
    [YarnCommand("LoadScene")]
    public static void ChangeScene()
    {
        SceneManager.LoadScene(2);
    }
}