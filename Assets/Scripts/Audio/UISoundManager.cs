using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance; 

    public AudioSource sfxSource;     

    [Header("UI Sounds")]
    public AudioClip buttonClickClip; 

   
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickClip);
    }

   
}
