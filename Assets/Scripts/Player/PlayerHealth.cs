using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;                       
    public Image[] heartImages;                 
    public Sprite fullHeart;                    
    public Sprite brokenHeart;                  

    private Animator animator;                   
    //public GameObject gameOverPanel;            

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateHearts();
        /*if (gameOverPanel != null)
            gameOverPanel.SetActive(false);*/
    }

    public void TakeDamage()
    {
        if (isDead) return;

        lives--;
        UpdateHearts();

        if (lives <= 0)
        {
            Die();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < lives)
                heartImages[i].sprite = fullHeart;    // alive → full heart
            else
                heartImages[i].sprite = brokenHeart;  // lost → broken
        }
    }

    void Die()
    {
        isDead = true;
        animator.Play("Dead"); 
        Invoke("ShowGameOver", 1.5f); 
    }

    void ShowGameOver()
    {
       /* if (gameOverPanel != null)
            gameOverPanel.SetActive(true);*/
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))  
        {
            
            TakeDamage();
        
            Destroy(other.gameObject);
        }
    }
}
