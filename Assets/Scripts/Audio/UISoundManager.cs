using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance; 

    public AudioSource sfxSource;     

    [Header("UI Sounds")]
    public AudioClip buttonClickClip; 

    [Header("Character Sounds")]
    public AudioClip moveClip;
    public AudioClip jumpClip;
    public AudioClip swimClip;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickClip);
    }

    public void PlayMove()
    {
        if (moveClip != null) sfxSource.PlayOneShot(moveClip);
    }

    public void PlayJump()
    {
        if (jumpClip != null) sfxSource.PlayOneShot(jumpClip);
    }

    public void PlaySwim()
    {
        if (swimClip != null) sfxSource.PlayOneShot(swimClip);
    }
}
