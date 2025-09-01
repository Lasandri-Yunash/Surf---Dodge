using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public TMP_Text scoreText;   
    private int score = 0;

    void Start()
    {
        UpdateScoreText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            score += 1;
            UpdateScoreText();

            Destroy(other.gameObject); 
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
