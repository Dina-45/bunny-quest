using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Play(Sfx.Lose);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}