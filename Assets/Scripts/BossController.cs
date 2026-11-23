
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public BossStats stats;
    public Slider hpBar;
    public GameObject fireballPrefab, warningPrefab;
    public Transform player;

    static readonly Color HitColor = new Color(1f, 0.5f, 0.5f, 1f);

    int hp;
    bool phase2, dead;
    SpriteRenderer sr;
    Animator anim;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (stats == null || hpBar == null || player == null ||
            fireballPrefab == null || warningPrefab == null)
        {
            Debug.LogError("BossController: Assign all required references in Inspector.", this);
            enabled = false;
            return;
        }

        hp = stats.maxHp;
        hpBar.maxValue = stats.maxHp;
        hpBar.value = hp;

        StartCoroutine(AttackLoop());
    }

    public void TakeDamage(int dmg)
    {
        if (dead || stats == null) return;

        hp = Mathf.Max(0, hp - dmg);
        hpBar.value = hp;

        SoundManager.Play(Sfx.Hit);

        StartCoroutine(Flash());

        if (!phase2 && hp <= Mathf.FloorToInt(stats.maxHp * stats.phase2HealthFraction))
        {
            phase2 = true;

            if (anim != null)
                anim.SetTrigger("Phase2");
        }

        if (hp <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Flash()
    {
        if (sr == null) yield break;

        sr.color = HitColor;
        yield return new WaitForSeconds(stats.hitFlashTime);
        sr.color = Color.white;
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(stats.startDelay);

        while (!dead)
        {
            if (!phase2)
            {
                yield return FireFan(stats.p1FanCount, stats.p1FanStep, stats.p1FanSpeed);
                yield return new WaitForSeconds(stats.p1PauseAfterFan);

                yield return Rain(stats.p1RainColumns);
                yield return new WaitForSeconds(stats.p1PauseAfterRain);
            }
            else
            {
                for (int i = 0; i < stats.p2Volleys && !dead; i++)
                {
                    yield return FireFan(stats.p2FanCount, stats.p2FanStep, stats.p2FanSpeed);
                    yield return new WaitForSeconds(stats.p2PauseBetweenVolleys);
                }

                if (dead) yield break;

                yield return Rain(stats.p2RainColumns);
                yield return new WaitForSeconds(stats.p2PauseAfterRain);
            }

            yield return null;
        }
    }

    void SetupFireball(GameObject f, Vector2 direction, float speed)
    {
        Fireball fb = f.GetComponent<Fireball>();

        if (fb == null)
        {
            Debug.LogError("Fireball prefab is missing Fireball script.");
            Destroy(f);
            return;
        }

        fb.direction = direction;
        fb.speed = speed;
        fb.damage = stats.fireballDamage;
        fb.lifetime = stats.fireballLifetime;
    }

    IEnumerator FireFan(int count, float step, float fireSpeed)
    {
        if (player == null) yield break;

        SoundManager.Play(Sfx.Shoot);

        Vector2 toPlayer = (player.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < count && !dead; i++)
        {
            float a = (baseAngle + (i - (count - 1) / 2f) * step) * Mathf.Deg2Rad;

            GameObject f = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

            SetupFireball(f, new Vector2(Mathf.Cos(a), Mathf.Sin(a)), fireSpeed);
        }

        yield return null;
    }

    IEnumerator Rain(int columns)
    {
        if (player == null) yield break;

        float[] xs = new float[columns];
        GameObject[] warns = new GameObject[columns];

        for (int i = 0; i < columns && !dead; i++)
        {
            xs[i] = player.position.x + (i - (columns - 1) / 2f) * stats.rainColumnSpacing;

            warns[i] = Instantiate(
                warningPrefab,
                new Vector3(xs[i], player.position.y, 0),
                Quaternion.identity
            );
        }

        SoundManager.Play(Sfx.Windup);

        yield return new WaitForSeconds(stats.rainWarningTime);

        for (int i = 0; i < columns; i++)
        {
            if (warns[i] != null)
                Destroy(warns[i]);

            if (dead) yield break;

            GameObject f = Instantiate(
                fireballPrefab,
                new Vector3(xs[i], stats.arenaTopY, 0),
                Quaternion.identity
            );

            SetupFireball(f, Vector2.down, stats.rainFallSpeed);
        }
    }

    IEnumerator Die()
    {
        if (dead) yield break;

        dead = true;

        ProgressTracker.CompleteLevel(0);

        SoundManager.Play(Sfx.Win);

        yield return new WaitForSeconds(stats.deathDelay);

        LevelData lvl = GameManager.Instance != null
            ? GameManager.Instance.level
            : null;

        string next = lvl != null ? lvl.nextSceneName : "";

        if (!string.IsNullOrEmpty(next))
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
