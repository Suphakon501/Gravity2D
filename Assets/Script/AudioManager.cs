using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM")]
    public AudioSource bgmSource;
    public AudioClip gameBGM;
    public AudioClip deathBGM;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip jumpSFX;
    public AudioClip hurtSFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void PlayGameBGM()
    {
        bgmSource.clip = gameBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayDeathBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = deathBGM;
        bgmSource.loop = false;
        bgmSource.Play();
    }

    
    public void PlayJumpSFX()
    {
        sfxSource.PlayOneShot(jumpSFX);
    }

    public void PlayHurtSFX()
    {
        sfxSource.PlayOneShot(hurtSFX);
    }
}
