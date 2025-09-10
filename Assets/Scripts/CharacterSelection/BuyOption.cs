using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyOption : MonoBehaviour
{
    public int characterCost;           // Set this to the character's cost
    public TMP_Text costText;           // Reference to the button's cost text
    public PlayerScore playerScore;     // Reference to the PlayerScore script to access the coins

    public void OnBuyButtonClicked()
    {
        if (PlayerScore.score >= characterCost)
        {
            
            PlayerScore.score -= characterCost;

            
            PlayerPrefs.SetInt("Coins", PlayerScore.score);
            PlayerPrefs.Save();

            
           // playerScore.UpdateScoreText();

            
            costText.text = "SELECT";

            
            GetComponent<Button>().interactable = false;
        }
        else
        {
            Debug.Log("Not enough coins to purchase.");
        }
    }
}
