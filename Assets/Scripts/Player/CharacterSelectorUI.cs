using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectorUI : MonoBehaviour
{
    public void SelectCharacter(int index)
    {
        CharacterSelectionManager.Instance.selectedCharacterIndex = index;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerMovement"); // Replace with your gameplay scene name
    }
}
