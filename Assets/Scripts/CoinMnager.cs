using UnityEngine;
using UnityEngine.UI;

public class CoinMnager : MonoBehaviour
{
    public Text coinText;
    private int coins;

    void Start()
    {
        // Load saved coin value (default is 0 if not saved yet)
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
       
    }

    void UpdateUI()
    {
        coinText.text = coins.ToString();
    }
}
