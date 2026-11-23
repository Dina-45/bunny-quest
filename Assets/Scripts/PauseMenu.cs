using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] string menuSceneName = "MainMenu";

    [Header("Audio")]
    [SerializeField] AudioMixerSnapshot normalSnapshot;
    [SerializeField] AudioMixerSnapshot pausedSnapshot;
    [SerializeField] float snapshotFade = 0.3f;     // плавный переход, без щелчка

    void Start()
    {
        Resume(); // every level starts unpaused
    }

    void OnDestroy()
    {
        Time.timeScale = 1f; // never leave the game frozen
        SetSnapshot(normalSnapshot);   // и музыку не оставляем приглушённой
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        SetSnapshot(pausedSnapshot);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        SetSnapshot(normalSnapshot);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SetSnapshot(normalSnapshot);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SetSnapshot(normalSnapshot);
        SceneManager.LoadScene(menuSceneName);
    }

    void SetSnapshot(AudioMixerSnapshot s)
    {
        if (s != null) s.TransitionTo(snapshotFade);
    }
}