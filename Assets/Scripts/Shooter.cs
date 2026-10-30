using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public AudioClip shootClip;
    public float cooldown = 0.4f;
    public float spawnOffset = 0.6f;

    float nextFireTime;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnFire()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + cooldown;

        float dir = sr.flipX ? -1f : 1f;
        Vector3 pos = transform.position + new Vector3(dir * spawnOffset, 0f, 0f);

        GameObject b = Instantiate(bulletPrefab, pos, Quaternion.identity);
        b.GetComponent<Bullet>().direction = dir;

        if (shootClip != null)
            AudioSource.PlayClipAtPoint(shootClip, transform.position, 0.6f);
    }
}