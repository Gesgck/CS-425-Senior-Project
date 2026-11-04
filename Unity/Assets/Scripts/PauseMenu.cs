using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private InputActionReference pauseActionReference;
    public static bool isPaused = false;

    private void OnEnable()
    {
        if (pauseActionReference != null)
        {
            pauseActionReference.action.Enable();
            pauseActionReference.action.performed += OnPause;
        }
    }

    private void OnDisable()
    {
        if (pauseActionReference != null)
        {
            pauseActionReference.action.performed -= OnPause;
            pauseActionReference.action.Disable();
        }
    }
    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
