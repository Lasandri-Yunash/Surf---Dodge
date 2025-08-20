using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;   
    public AudioClip backgroundMusic; 

    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
}
