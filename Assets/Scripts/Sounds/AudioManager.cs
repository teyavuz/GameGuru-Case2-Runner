using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource perfectNoteSource;
    [SerializeField] private AudioClip perfectNoteClip;
    [SerializeField] private float pitchStep = 0.1f;
    [SerializeField] private float maxPitch = 2f;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private AudioClip victoryMusicClip;
    [SerializeField] private AudioClip gameOverMusicClip;
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float victoryMusicVolume = 0.7f;
    [SerializeField] private float gameOverMusicVolume = 0.3f;

    private int perfectStreak = 0;

    private void Awake()
    {
        Instance = this;

        PlayBackgroundMusic();
    }

    public void PlayPerfectNote()
    {
        perfectStreak++;
        float pitch = Mathf.Min(1f + perfectStreak * pitchStep, maxPitch);

        perfectNoteSource.pitch = pitch;
        perfectNoteSource.PlayOneShot(perfectNoteClip);
    }

    public void ResetPerfectPitch()
    {
        perfectStreak = 0;
        perfectNoteSource.pitch = 1f;
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusicClip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayVictoryMusic()
    {
        musicSource.Stop();
        musicSource.clip = victoryMusicClip;
        musicSource.volume = victoryMusicVolume;
        musicSource.loop = false;
        musicSource.Play();
    }

    public void PlayGameOverMusic()
    {
        musicSource.Stop();
        musicSource.clip = gameOverMusicClip;
        musicSource.volume = gameOverMusicVolume;
        musicSource.loop = false;
        musicSource.Play();
    }
}
