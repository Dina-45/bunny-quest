using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

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
            ProgressTracker.CompleteLevel(unlocks);
            FxManager.Spawn(Fx.Win, transform.position);
            StartCoroutine(WinAndLoadNextLevel());
        }
    }

    IEnumerator WinAndLoadNextLevel()
    {
        yield return new WaitForSeconds(SoundManager.Play(Sfx.Win));

        LevelData level = GameManager.Instance.level;
        if (level != null && !string.IsNullOrEmpty(level.nextSceneName))
            SceneManager.LoadScene(level.nextSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}