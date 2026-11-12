using UnityEngine;

public class Shooter : MonoBehaviour
{
    public PlayerStats stats;
    public GameObject bulletPrefab;
    public AudioClip shootClip;

    float nextFireTime;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnFire()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + stats.shootCooldown;

        float dir = sr.flipX ? -1f : 1f;
        Vector3 pos = transform.position + new Vector3(dir * stats.spawnOffset, 0f, 0f);

        GameObject b = Instantiate(bulletPrefab, pos, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        bullet.direction = dir;
        bullet.speed = stats.bulletSpeed;
        bullet.lifetime = stats.bulletLifetime;
        bullet.damage = stats.bulletDamage;

        if (shootClip != null)
            AudioSource.PlayClipAtPoint(shootClip, transform.position, 0.6f);
    }
}