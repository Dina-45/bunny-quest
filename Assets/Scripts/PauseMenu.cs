
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] GameObject pausePanel;
    [SerializeField] string menuSceneName = "MainMenu";

    [Header("Audio")]
    [SerializeField] AudioMixerSnapshot normalSnapshot;
    [SerializeField] AudioMixerSnapshot pausedSnapshot;
    [SerializeField] float snapshotFade = 0.3f;

    void Start()
    {
        Resume();
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SetSnapshot(normalSnapshot);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        SetSnapshot(pausedSnapshot);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        SetSnapshot(normalSnapshot);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SetSnapshot(normalSnapshot);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SetSnapshot(normalSnapshot);
        SceneManager.LoadScene(menuSceneName);
    }

    void SetSnapshot(AudioMixerSnapshot s)
    {
        if (s != null) s.TransitionTo(snapshotFade);
    }
}
