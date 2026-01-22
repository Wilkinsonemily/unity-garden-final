using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 0.25f;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;

    [Header("Music")]
    public AudioClip backgroundMusic;

    private AudioSource musicSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
        if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f;
    }

    void Start()
    {
        if (backgroundMusic != null)
            PlayMusic(backgroundMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void SetMusicVolume(float v)
    {
        musicVolume = Mathf.Clamp01(v);
        if (musicSource != null) musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float v)
    {
        sfxVolume = Mathf.Clamp01(v);
    }

    public void PlaySFX(AudioClip clip, float volumeMul = 1f)
    {
        if (clip == null) return;

        GameObject go = new GameObject("SFX_" + clip.name);
        AudioSource src = go.AddComponent<AudioSource>();
        src.spatialBlend = 0f; // 2D
        src.playOnAwake = false;
        src.loop = false;
        src.volume = sfxVolume * Mathf.Clamp01(volumeMul);
        src.clip = clip;

        src.Play();
        Destroy(go, clip.length + 0.1f);
    }
}
