using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadSceneAsync("TestLevel");
        
    }

    public void ResumeGame()
    {
        PauseMenu.isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenu.pauseMenu.SetActive(false);
    }

    public void LoadGameMenu()
    {
        SceneManager.LoadSceneAsync("LoadGameMenu");
    }

    public void OptionsMenu()
    {
        SceneManager.LoadSceneAsync("OptionsMenu");
    }

    public void AudioOptions()
    {
        SceneManager.LoadSceneAsync("AudioOptionsMenu");
    }

    public void VideoOptions()
    {
        SceneManager.LoadSceneAsync("VideoOptionsMenu");
    }

    public void ControlsMenu()
    {
        SceneManager.LoadSceneAsync("ControlsMenu");
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        PauseMenu.isPaused = false;
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
