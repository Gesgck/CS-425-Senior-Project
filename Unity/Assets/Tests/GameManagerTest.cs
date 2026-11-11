using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameManagerTest
{
    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator EnemyIsNotDefetedBeforeKilledAndIsAfter()
    {
        // arrange
        GameObject gmObj = new GameObject("EnemyStateTest");
        gmObj.AddComponent<GameManager>();
        yield return null;

        string testEnemyID = "TestEnemy";

        Assert.IsFalse(GameManager.Instance.IsEnemyDefeated(testEnemyID));
        GameManager.Instance.DefeatEnemy(testEnemyID);
        Assert.IsTrue(GameManager.Instance.IsEnemyDefeated(testEnemyID));

        Object.Destroy(gmObj);
    }
}
