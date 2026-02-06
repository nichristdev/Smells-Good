using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    bool GamePaused;
    public GameObject pauseMenu, settingsMenu, androidLayout;
    InputManager inputManager;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start() {
        if (inputManager.inputMode == InputMode.Windows)
            Cursor.visible = false;
    }

    private void Update() {
        if (inputManager.MainInput.Main.Back.WasPressedThisFrame ()){
            GamePaused = !GamePaused;
            pauseMenu.SetActive (GamePaused);
            settingsMenu.SetActive (false);

            if (inputManager.inputMode == InputMode.Android)
                androidLayout.SetActive(!GamePaused);
            else
            {
                Cursor.visible = GamePaused;
                androidLayout.SetActive(false);
            }
        }

        Time.timeScale = GamePaused ? 0f : 1f;
    }

    public void Pause (){
        GamePaused = true;
        pauseMenu.SetActive (true);

        androidLayout.SetActive (false);
        AudioManager.Instance.PauseAllSound();
    }

    public void Resume (){
        GamePaused = false;
        pauseMenu.SetActive (false);
        settingsMenu.SetActive (false);
        AudioManager.Instance.ResumeAllSound();

        if (inputManager.inputMode == InputMode.Android)
            androidLayout.SetActive (true);
        else Cursor.visible = GamePaused;
    }

    public void QuitGame (){
        Application.Quit ();
    }
}