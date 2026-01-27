using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip gameplayBGM;
    public AudioClip deathBGM;

    [Header("SFX")]
    public AudioClip jumpSFX;
    public AudioClip hitSFX;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayGameplayBGM();
    }

    // ===== BGM =====
    public void PlayGameplayBGM()
    {
        bgmSource.clip = gameplayBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayDeathBGM()
    {
        bgmSource.clip = deathBGM;
        bgmSource.loop = false;
        bgmSource.Play();
    }

    // ===== SFX =====
    public void PlayJumpSFX()
    {
        sfxSource.PlayOneShot(jumpSFX);
    }

    public void PlayHitSFX()
    {
        sfxSource.PlayOneShot(hitSFX);
    }
}
