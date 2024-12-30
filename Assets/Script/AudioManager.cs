using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        HandleAudioListeners();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.isPlaying)
        {
            Debug.Log($"Music is already playing: {musicSource.clip.name}. Stopping it first.");
            StopMusic();
        }

        musicSource.clip = clip;
        musicSource.Play();
        Debug.Log("Playing music: " + clip.name);
    }

    public void StopMusic()
    {
        // musicSource.Stop();

        if (musicSource.isPlaying)
        {
            Debug.Log("Stopping current music: " + musicSource.clip.name);
            musicSource.Stop();
        }
        else
        {
            Debug.Log("No music is playing to stop.");
        }

        
    }

    private void HandleAudioListeners()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();

        if (listeners.Length > 1)
        {
            foreach (var listener in listeners)
            {
                if (listener.gameObject != Camera.main.gameObject)
                {
                    listener.enabled = false;
                }
            }
        }
        else if (listeners.Length == 0)
        {
            Debug.LogError("No Audio Listener found! Please ensure there is at least one active Audio Listener in the scene.");
        }
    }

    public void DebugActiveAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio.isPlaying)
            {
                Debug.Log($"Active AudioSource: {audio.clip.name} on GameObject: {audio.gameObject.name}");
            }
        }
    }
    
}

