using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioClip slowStartSFX;
    public AudioClip slowEndSFX;

    public void StopBGM()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            StopBGM();
        }
        else if (scene.name == "Gameplay") 
        {
            PlayGameplayBGM();
        }
    }

    // ===== BGM =====
    public void PlayGameplayBGM()
    {
        if (bgmSource == null || gameplayBGM == null) return;

        if (bgmSource.clip == gameplayBGM && bgmSource.isPlaying) return;

        bgmSource.Stop();
        bgmSource.clip = gameplayBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayDeathBGM()
    {
        if (bgmSource == null || deathBGM == null) return;

        bgmSource.Stop();
        bgmSource.clip = deathBGM;
        bgmSource.loop = false;
        bgmSource.Play();
    }

    // ===== SFX =====
    public void PlayJumpSFX()
    {
        if (sfxSource == null || jumpSFX == null) return;
        sfxSource.PlayOneShot(jumpSFX);
    }

    public void PlayHitSFX()
    {
        if (sfxSource == null || hitSFX == null) return;
        sfxSource.PlayOneShot(hitSFX);
    }

    public void PlaySlowStart()
    {
        if (sfxSource == null || slowStartSFX == null) return;
        sfxSource.PlayOneShot(slowStartSFX);
    }

    public void PlaySlowEnd()
    {
        if (sfxSource == null || slowEndSFX == null) return;
        sfxSource.PlayOneShot(slowEndSFX);
    }
}
