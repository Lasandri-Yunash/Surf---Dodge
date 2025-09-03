/*// CharacterManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    
    public static int selectedCharacterIndex = 0;

    public GameObject[] characterPrefabs;

    public string playerTag = "Player";

    
    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
       
    }

    private void Start()
    {
        
        if (SceneManager.GetActiveScene().name == "PlayerMovement")
        {
            ReplaceExistingPlayerInThisScene();
        }
    }

    private void ReplaceExistingPlayerInThisScene()
    {
        if (characterPrefabs == null || characterPrefabs.Length == 0) return;

        GameObject existing = GameObject.FindWithTag(playerTag);
        if (existing == null)
        {
        
            Character existingChar = FindObjectOfType<Character>();
            if (existingChar != null) existing = existingChar.gameObject;
        }

        
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        Transform parent = null;

        if (existing != null)
        {
            pos = existing.transform.position;
            rot = existing.transform.rotation;
            parent = existing.transform.parent;
        }

        
        if (existing != null) Destroy(existing);

        
        int i = Mathf.Clamp(selectedCharacterIndex, 0, characterPrefabs.Length - 1);
        GameObject newPlayer = Instantiate(characterPrefabs[i], pos, rot, parent);

    
        if (!string.IsNullOrEmpty(playerTag)) newPlayer.tag = playerTag;

        var cam = Camera.main;
        if (cam != null)
        {
            var camIntro = cam.GetComponent<CameraIntro>();
            if (camIntro != null)
            {
                camIntro.targetPosition = newPlayer.transform;
            }
        }
    }
}
*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public TMP_Text nameTxt;

    public Transform previewPoint; 

    private int selectedOption = 0;
    private GameObject currentModel;

    private void Start()
    {
        
        previewPoint.position = new Vector3(80f, 80f, -160f);

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


    public void NextOption()
    {
        selectedOption++;
        if (selectedOption >= characterDB.CharacterCount)
        {
            selectedOption = 0;
        }
        UpdateCharacter(selectedOption);
        save();
    }

    public void BackOption()
    {
        selectedOption--;
        if (selectedOption < 0)
        {
            selectedOption = characterDB.CharacterCount - 1;
        }
        UpdateCharacter(selectedOption);

        save();
    }

    private void UpdateCharacter(int selectedOption)
    {
        
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        
        CharacterC character = characterDB.GetCharacter(selectedOption);

        
        nameTxt.text = character.characterName;

        
        currentModel = Instantiate(character.characterPrefab, previewPoint.position, previewPoint.rotation);
        currentModel.transform.SetParent(previewPoint);

        
        currentModel.transform.localScale = Vector3.one * 80f;

        
        currentModel.transform.localPosition = new Vector3(0f, 0f, 0f);

        
        Rigidbody rb = currentModel.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }



    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    private void save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }

    public void BuyButton()
    {
        SceneManager.LoadScene("PlayerMovement");
    }
}

