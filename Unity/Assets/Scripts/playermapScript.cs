using UnityEngine;
using UnityEngine.InputSystem;

public class playermapScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // If the GameManager has a saved position stored, move the player there
        if (GameManager.Instance != null && GameManager.Instance.lastPosition != Vector2.zero)
        {
            transform.position = GameManager.Instance.lastPosition;

            // Reset position
            GameManager.Instance.lastPosition = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
