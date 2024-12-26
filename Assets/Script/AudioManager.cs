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
        Debug.Log("Playing music: " + clip.name);

        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
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
    
}

