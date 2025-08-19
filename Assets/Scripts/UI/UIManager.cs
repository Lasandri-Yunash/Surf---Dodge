using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject infoPanel;

    void Start()
    {
        
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);
    }

    // Settings
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // Info
    public void OpenInfo()
    {
        if (infoPanel != null) infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        if (infoPanel != null) infoPanel.SetActive(false);
    }
}
