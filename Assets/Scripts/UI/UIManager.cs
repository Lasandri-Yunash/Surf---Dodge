using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject infoPanel;

    private bool isPaused = false;

    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        PauseGame();
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        ResumeGame();
    }

    public void OpenInfo()
    {
        if (infoPanel != null) infoPanel.SetActive(true);
        PauseGame();
    }

    public void CloseInfo()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
        ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("PlayerMovement");
    }
}
