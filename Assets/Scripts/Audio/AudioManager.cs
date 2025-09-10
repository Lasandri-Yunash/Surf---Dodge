using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;   // For background music
    public AudioSource SFXSource;     // For sound effects

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip coinMusic;
    public AudioClip turnsideMusic;
    public AudioClip purchesMusic;
    public AudioClip buttonClickClip;

    private bool musicOn = true;
    private bool sfxOn = true;

    private void Awake()
    {
        // Add AudioSources if not assigned
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        if (SFXSource == null)
            SFXSource = gameObject.AddComponent<AudioSource>();

        // Set musicSource to loop for background music
        musicSource.loop = true;
    }
    private void Start()
    {
        if (!musicSource.gameObject.activeInHierarchy)
        {
            musicSource.gameObject.SetActive(true);
        }

        // Assign background music clip and start playing
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Play a general sound effect clip via SFX audio source.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (sfxOn && clip != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Play button click sound effect.
    /// </summary>
    public void PlayButtonClick()
    {
        PlaySFX(buttonClickClip);
    }

    /// <summary>
    /// Toggles background music on/off.
    /// </summary>
    public void ToggleMusic()
    {
        musicOn = !musicOn;
        Debug.Log("Music toggled: " + musicOn);

        if (musicSource != null)
        {
            if (!musicSource.enabled)
            {
                musicSource.enabled = true; // Enable if disabled
            }

            if (musicOn)
            {
                if (!musicSource.isPlaying)
                    musicSource.Play();
                else
                    musicSource.UnPause();
            }
            else
            {
                musicSource.Pause();
            }
        }
        else
        {
            Debug.LogWarning("musicSource is null!");
        }
    }


    /// <summary>
    /// Toggles sound effects on/off.
    /// </summary>
    public void ToggleSFX()
    {
        sfxOn = !sfxOn;
        if (!sfxOn && SFXSource != null)
        {
            SFXSource.Stop();
        }
        Debug.Log("SFX toggled: " + sfxOn);
    }

    /// <summary>
    /// Set music explicitly on or off.
    /// </summary>
    public void SetMusic(bool on)
    {
        musicOn = on;
        if (musicOn)
            musicSource.UnPause();
        else
            musicSource.Pause();
    }

    /// <summary>
    /// Set sound effects explicitly on or off.
    /// </summary>
    public void SetSFX(bool on)
    {
        sfxOn = on;
        if (!sfxOn && SFXSource != null)
            SFXSource.Stop();
    }
}
