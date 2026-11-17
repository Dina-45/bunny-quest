using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public AudioClip winClip;

    bool isExiting;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (isExiting) return;

        GameManager gm = GameManager.Instance;
        if (gm.coins >= gm.totalCoins)
        {
            isExiting = true;
            int unlocks = gm.level != null ? gm.level.unlocksLevel : 0;
            ProgressTracker.CompleteLevel(unlocks);   // сохраняем до звука победы
            StartCoroutine(WinAndLoadNextLevel());
        }
    }

    IEnumerator WinAndLoadNextLevel()
    {
        if (winClip != null)
        {
            GameObject audioObject = new GameObject("WinSound");
            AudioSource source = audioObject.AddComponent<AudioSource>();
            source.clip = winClip;
            source.Play();
            yield return new WaitForSeconds(winClip.length);
            Destroy(audioObject);
        }

        LevelData level = GameManager.Instance.level;
        if (level != null && !string.IsNullOrEmpty(level.nextSceneName))
            SceneManager.LoadScene(level.nextSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}