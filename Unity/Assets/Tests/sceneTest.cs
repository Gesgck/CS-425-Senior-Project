using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneTest
{
    [UnityTest]
    public IEnumerator CheckForActiveSceneSwitchSuccess()
    {

        SceneManager.LoadScene(0);
        yield return null;
        Scene activeScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(1);
        yield return null;
        Scene activeScene2 = SceneManager.GetActiveScene();

        Assert.AreNotEqual(activeScene, activeScene2);
        Assert.AreEqual(SceneManager.GetActiveScene(), activeScene2);

    }

}
