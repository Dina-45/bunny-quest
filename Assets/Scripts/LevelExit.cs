
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

        if (GameManager.Instance.coins >= GameManager.Instance.totalCoins)
        {
            isExiting = true;
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
            source.volume = 1f;
            source.Play();

            yield return new WaitForSeconds(winClip.length);

            Destroy(audioObject);
        }

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
