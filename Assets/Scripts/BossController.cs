using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public int maxHP = 6;
    public Slider hpBar;
    public GameObject fireballPrefab, warningPrefab;
    public Transform player;
    public float arenaTopY = 6f;
    public AudioClip hitClip, winClip;

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
        hp = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = hp;
        StartCoroutine(AttackLoop());
    }

    public void TakeDamage(int dmg)
    {
        if (dead) return;
        hp -= dmg;
        hpBar.value = hp;
        if (hitClip != null) AudioSource.PlayClipAtPoint(hitClip, transform.position);
        StartCoroutine(Flash());

        if (!phase2 && hp <= maxHP / 2)
        {
            phase2 = true;
            anim.SetTrigger("Phase2");
        }
        if (hp <= 0) StartCoroutine(Die());
    }

    IEnumerator Flash()
    {
        sr.color = new Color(1f, 0.5f, 0.5f, 1f);
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(2f);
        while (!dead)
        {
            if (!phase2)
            {
                yield return FireFan(3, 15f, 5f);
                yield return new WaitForSeconds(2.5f);
                yield return Rain(1);
                yield return new WaitForSeconds(2.5f);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    yield return FireFan(7, 20f, 7f);
                    yield return new WaitForSeconds(0.4f);
                }
                yield return Rain(3);
                yield return new WaitForSeconds(1.2f);
            }
        }
    }

    IEnumerator FireFan(int count, float step, float fireSpeed)
    {
        Vector2 toPlayer = (player.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        for (int i = 0; i < count; i++)
        {
            float a = (baseAngle + (i - (count - 1) / 2f) * step) * Mathf.Deg2Rad;
            GameObject f = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Fireball fb = f.GetComponent<Fireball>();
            fb.direction = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            fb.speed = fireSpeed;
        }
        yield return null;
    }

    IEnumerator Rain(int columns)
    {
        float[] xs = new float[columns];
        GameObject[] warns = new GameObject[columns];
        for (int i = 0; i < columns; i++)
        {
            xs[i] = player.position.x + (i - (columns - 1) / 2f) * 3f;
            warns[i] = Instantiate(warningPrefab, new Vector3(xs[i], player.position.y, 0), Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < columns; i++)
        {
            Destroy(warns[i]);
            GameObject f = Instantiate(fireballPrefab, new Vector3(xs[i], arenaTopY, 0), Quaternion.identity);
            f.GetComponent<Fireball>().direction = Vector2.down;
        }
    }

    IEnumerator Die()
    {
        dead = true;
        if (winClip != null) AudioSource.PlayClipAtPoint(winClip, transform.position);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}