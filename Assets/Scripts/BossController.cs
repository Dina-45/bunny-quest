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
    public AudioClip hitClip, winClip;

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
        if (stats == null)
        {
            Debug.LogError("BossController: assign BossStats on " + name, this);
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
        hp -= dmg;
        hpBar.value = hp;
        if (hitClip != null) AudioSource.PlayClipAtPoint(hitClip, transform.position);
        StartCoroutine(Flash());

        if (!phase2 && hp <= Mathf.FloorToInt(stats.maxHp * stats.phase2HealthFraction))
        {
            phase2 = true;
            anim.SetTrigger("Phase2");
        }
        if (hp <= 0) StartCoroutine(Die());
    }

    IEnumerator Flash()
    {
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
                for (int i = 0; i < stats.p2Volleys; i++)
                {
                    yield return FireFan(stats.p2FanCount, stats.p2FanStep, stats.p2FanSpeed);
                    yield return new WaitForSeconds(stats.p2PauseBetweenVolleys);
                }
                yield return Rain(stats.p2RainColumns);
                yield return new WaitForSeconds(stats.p2PauseAfterRain);
            }
            yield return null;   // защита: если в данных все паузы 0, игра не зависнет
        }
    }

    void SetupFireball(GameObject f, Vector2 direction, float speed)
    {
        Fireball fb = f.GetComponent<Fireball>();
        fb.direction = direction;
        fb.speed = speed;
        fb.damage = stats.fireballDamage;
        fb.lifetime = stats.fireballLifetime;
    }

    IEnumerator FireFan(int count, float step, float fireSpeed)
    {
        Vector2 toPlayer = (player.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        for (int i = 0; i < count; i++)
        {
            float a = (baseAngle + (i - (count - 1) / 2f) * step) * Mathf.Deg2Rad;
            GameObject f = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            SetupFireball(f, new Vector2(Mathf.Cos(a), Mathf.Sin(a)), fireSpeed);
        }
        yield return null;
    }

    IEnumerator Rain(int columns)
    {
        float[] xs = new float[columns];
        GameObject[] warns = new GameObject[columns];
        for (int i = 0; i < columns; i++)
        {
            xs[i] = player.position.x + (i - (columns - 1) / 2f) * stats.rainColumnSpacing;
            warns[i] = Instantiate(warningPrefab, new Vector3(xs[i], player.position.y, 0), Quaternion.identity);
        }
        yield return new WaitForSeconds(stats.rainWarningTime);
        for (int i = 0; i < columns; i++)
        {
            Destroy(warns[i]);
            GameObject f = Instantiate(fireballPrefab, new Vector3(xs[i], stats.arenaTopY, 0), Quaternion.identity);
            SetupFireball(f, Vector2.down, stats.rainFallSpeed);
        }
    }

    IEnumerator Die()
    {
           dead = true;
           ProgressTracker.CompleteLevel(0);   
    if (winClip != null) AudioSource.PlayClipAtPoint(winClip, transform.position);
    yield return new WaitForSeconds(stats.deathDelay);
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}