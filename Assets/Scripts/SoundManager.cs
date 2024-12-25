using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource menuAudioSource; // Assign your audio source for the menu sound.

    private void Awake()
    {
        // Ensure there's only one instance of SoundManager and it persists between scenes.
        if (FindObjectsOfType<SoundManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMenuSound(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMenuSound(scene.name);
    }

    private void PlayMenuSound(string sceneName)
    {
        if (sceneName == "MainScene" || sceneName == "GameScene")
        {
            if (!menuAudioSource.isPlaying)
            {
                menuAudioSource.Play();
            }
        }
        else
        {
            menuAudioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
