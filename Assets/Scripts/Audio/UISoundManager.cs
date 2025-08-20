using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public AudioSource sfxSource;     
    public AudioClip buttonClickClip; 

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickClip);
    }
}
