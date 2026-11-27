using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public PlayerStats stats;

    [HideInInspector] public int maxHp;
    [HideInInspector] public int hp;

    bool invincible;
    Renderer[] renderers;
    HitFlash hitFlash;

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
        hitFlash = GetComponent<HitFlash>();
    }

    public void TakeDamage(int dmg)
    {
        if (invincible) return;

        hp -= dmg;
        Debug.Log("HP: " + hp);

        SoundManager.Play(Sfx.Hit);
        FxManager.Spawn(Fx.Hit, transform.position);
        FxManager.SmallShake();
        FxManager.HitStop();

        if (hitFlash != null) hitFlash.Flash();

        if (hp <= 0)
        {
            GameOver();
            return;
        }

        StartCoroutine(Invincibility());
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
        Debug.Log("HP: " + hp);

        SoundManager.Play(Sfx.Heal);
        FxManager.Spawn(Fx.Pickup, transform.position);
    }

    IEnumerator Invincibility()
    {
        invincible = true;

        yield return new WaitForSecondsRealtime(FxManager.FlashTime);

        for (int i = 0; i < stats.invincibilityFlashes; i++)
        {
            foreach (var r in renderers)
                r.enabled = !r.enabled;

            yield return new WaitForSeconds(stats.flashInterval);
        }

        foreach (var r in renderers)
            r.enabled = true;

        invincible = false;
    }

    void GameOver()
    {
        SoundManager.Play(Sfx.Lose);
        FxManager.Spawn(Fx.Death, transform.position);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}