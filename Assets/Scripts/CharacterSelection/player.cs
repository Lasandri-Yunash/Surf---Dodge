using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public CharacterDatabase characterDB;


    public Transform previewPoint; 

    private int selectedOption = 0;
    private GameObject currentModel;

    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }
        UpdateCharacter(selectedOption);
    }


    private void UpdateCharacter(int selectedOption)
    {
        
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        
        CharacterC character = characterDB.GetCharacter(selectedOption);

        

        
        currentModel = Instantiate(character.characterPrefab, previewPoint.position, previewPoint.rotation);
        currentModel.transform.SetParent(previewPoint);


    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}