using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCharacterManager : MonoBehaviour
{
    public GameObject[] characters; // drag all playable character prefabs here in inspector

    void Start()
    {
        int index = CharacterSelectionManager.Instance.selectedCharacterIndex;

        
        foreach (var character in characters)
        {
            character.SetActive(false);
        }

        
        if (index >= 0 && index < characters.Length)
        {
            characters[index].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Invalid character index selected");
        }
    }
}
