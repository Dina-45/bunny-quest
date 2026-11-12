using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public PlayerStats stats;
    public AudioClip hurtClip;
    public AudioClip healClip;

    [HideInInspector] public int maxHp;
    [HideInInspector] public int hp;

    bool invincible;
    Renderer[] renderers;
    AudioSource src;

    void Awake()
    {
        if (stats == null)
        {
            Debug.LogError("Health: assign PlayerStats on " + name, this);
            return;
        }
        maxHp = stats.maxHealth;
        hp = maxHp;
    }

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        src = GetComponent<AudioSource>();
    }

    public void TakeDamage(int dmg)
    {
        if (invincible) return;
        hp -= dmg;
        Debug.Log("HP: " + hp);
        if (src != null && hurtClip != null) src.PlayOneShot(hurtClip);
        if (hp <= 0) { GameOver(); return; }
        StartCoroutine(Invincibility());
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
        Debug.Log("HP: " + hp);
        if (src != null && healClip != null) src.PlayOneShot(healClip);
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < stats.invincibilityFlashes; i++)
        {
            foreach (var r in renderers) r.enabled = !r.enabled;
            yield return new WaitForSeconds(stats.flashInterval);
        }
        foreach (var r in renderers) r.enabled = true;
        invincible = false;
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}