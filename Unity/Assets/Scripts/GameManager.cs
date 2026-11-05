using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Persistent State")]
    public Vector2 lastPosition;
    public string previousSceneName;
    private HashSet<string> defeatedEnemies = new HashSet<string>();

    private void Awake()
    {
        //Enforce Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate copies
            return;
        }

        Instance = this;

        // Prevent this object from being destroyed
        DontDestroyOnLoad(gameObject);
    }

    // Method to handle scene transitions cleanly
    public void ChangeScene(string targetSceneName)
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(targetSceneName);
    }

    // Check if an enemy has already been beaten
    public bool IsEnemyDefeated(string enemyID)
    {
        return defeatedEnemies.Contains(enemyID);
    }

    // Mark an enemy as defeated
    public void DefeatEnemy(string enemyID)
    {
        if (!string.IsNullOrEmpty(enemyID) && !defeatedEnemies.Contains(enemyID))
        {
            defeatedEnemies.Add(enemyID);
        }
    }
}