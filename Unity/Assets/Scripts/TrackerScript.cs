using UnityEngine;

public class TrackerScript : MonoBehaviour
{
    [SerializeField] private string enemyID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Safety check if ID was left blank in the Inspector
        if (string.IsNullOrEmpty(enemyID))
        {
            Debug.LogWarning($"Enemy {gameObject.name} has no Unique ID assigned!", this);
            return;
        }

        // Ask the Singleton GameManager if this enemy was already killed
        if (GameManager.Instance != null && GameManager.Instance.IsEnemyDefeated(enemyID))
        {
            // If it was defeated, destroy it immediately on scene load
            Destroy(gameObject);
        }
    }
    public void Die()
    {
        if (GameManager.Instance != null && !string.IsNullOrEmpty(enemyID))
        {
            GameManager.Instance.DefeatEnemy(enemyID);
        }

        // Add any death effects, loot drops, or animations here
        Destroy(gameObject);
    }
}
