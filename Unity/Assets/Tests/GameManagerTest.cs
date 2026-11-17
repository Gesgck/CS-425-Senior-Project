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
        GameManager gm = gmObj.AddComponent<GameManager>();
        yield return null;

        string testEnemyID = "TestEnemy";

        Assert.IsFalse(gm.IsEnemyDefeated(testEnemyID));
        gm.DefeatEnemy(testEnemyID);
        Assert.IsTrue(gm.IsEnemyDefeated(testEnemyID));
    }
}
