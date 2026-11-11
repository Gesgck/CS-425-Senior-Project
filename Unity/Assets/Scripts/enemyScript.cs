using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyScript : MonoBehaviour
{
    [SerializeField] private string uniqueEnemyID;

    private void Start()
    {
        // If this enemy was already defeated previously, delete it on spawn
        if (GameManager.Instance != null && GameManager.Instance.IsEnemyDefeated(uniqueEnemyID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                // Save player's current position
                GameManager.Instance.lastPosition = collision.transform.position;

                // Mark enemy as defeated
                GameManager.Instance.DefeatEnemy(uniqueEnemyID);
            }

            // Trigger the scene transition
            GameManager.Instance.ChangeScene("CardScene");
        }
    }
}
