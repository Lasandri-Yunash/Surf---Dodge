using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public static int score = 0; // persists across scenes
    private TMP_Text scoreText;
    AudioManager audioManager;

    

    void Awake()
    {
        // Load saved score
        score = PlayerPrefs.GetInt("Coins", 0);

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        // Find the TMP Text in the scene tagged "ScoreText"
        GameObject scoreObj = GameObject.FindGameObjectWithTag("ScoreText");
        if (scoreObj != null)
        {
            scoreText = scoreObj.GetComponent<TMP_Text>();
            UpdateScoreText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            // Increase score immediately
            score += 1;

            // Save the updated score
            PlayerPrefs.SetInt("Coins", score);
            PlayerPrefs.Save();

            // Update UI instantly
            UpdateScoreText();
            audioManager.PlaySFX(audioManager.coinMusic);

            // Destroy the coin
            Destroy(other.gameObject);
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}
