using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionUI : MonoBehaviour
{
    public GameObject[] characterPanels;  
    private int currentIndex = 0;

    void Start()
    {
        ShowPanel(currentIndex);
    }

    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= characterPanels.Length)
            currentIndex = 0;  
        ShowPanel(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = characterPanels.Length - 1;  
        ShowPanel(currentIndex);
    }

    void ShowPanel(int index)
    {
        
        for (int i = 0; i < characterPanels.Length; i++)
            characterPanels[i].SetActive(i == index);
    }

    public void BuyButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
