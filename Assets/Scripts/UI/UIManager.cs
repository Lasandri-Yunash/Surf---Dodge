using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject infoPanel;
    public GameObject gameoverPanel;

    private bool isPaused = false;

    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);
        if (gameoverPanel != null) gameoverPanel.SetActive(false);

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
        Time.timeScale = 1f; 

        SceneManager.LoadScene("PlayerMovement");

    }

    public void ShopButton()
    {
        //SceneManager.LoadScene("CharacterSelection");
        Time.timeScale = 1f;
        SceneManager.LoadScene("CharacterChoose");
    }
    public void CloseShopButton()
    {
        SceneManager.LoadScene("StartScene");
    }
    
    public void gameOver()
    {
        if (gameoverPanel != null) gameoverPanel.SetActive(true);
        PauseGame();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
