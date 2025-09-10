using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public TMP_Text nameTxt;
    public TMP_Text costButtonText; 


    public Transform previewPoint;
    public GameObject playButton;
    public GameObject coinIcon;


    private int selectedOption = 0;
    private GameObject currentModel;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();


        previewPoint.position = new Vector3(12f, -50f, -155f);

        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
            costButtonText.text = "SELECT";
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

        if (IsCharacterPurchased(selectedOption))
        {
            costButtonText.text = "SELECT";
            playButton.SetActive(true);
            coinIcon.SetActive(false);


        }
        else
        {
            costButtonText.text = character.characterCost.ToString();

            playButton.SetActive(false); 

        }

        if (character.previewPrefab != null)
        {
            currentModel = Instantiate(character.previewPrefab, previewPoint.position, previewPoint.rotation);
            currentModel.transform.SetParent(previewPoint);

            currentModel.transform.localScale = Vector3.one * 80f;
            currentModel.transform.localPosition = Vector3.zero;

            Rigidbody rb = currentModel.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
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
        
            CharacterC character = characterDB.GetCharacter(selectedOption);

            if (!IsCharacterPurchased(selectedOption) && PlayerScore.score >= character.characterCost)
            {
                PlayerScore.score -= character.characterCost;
                PlayerPrefs.SetInt("Coins", PlayerScore.score);
                PlayerPrefs.SetInt("CharacterPurchased" + selectedOption, 1); // mark as purchased
                PlayerPrefs.Save();

                costButtonText.text = "SELECT";
            audioManager.PlaySFX(audioManager.purchesMusic);

            playButton.SetActive(true);
            coinIcon.SetActive(false);

            /*Time.timeScale = 1f;
            SceneManager.LoadScene("PlayerMovement");*/
        }
            else if (IsCharacterPurchased(selectedOption))
            {
                
                Time.timeScale = 1f;
                //SceneManager.LoadScene("PlayerMovement");
            }
            else
            {
                Debug.Log("Not enough coins to buy this character!");
            }
        }

    public void OnPlayButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PlayerMovement");
    }


    private bool IsCharacterPurchased(int index)
    {
        // For example, save purchased states in PlayerPrefs with keys like "CharacterPurchased0", "CharacterPurchased1"...
        return PlayerPrefs.GetInt("CharacterPurchased" + index, 0) == 1;
    }

}

