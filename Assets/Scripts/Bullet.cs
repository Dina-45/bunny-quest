
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public int damage;
    [HideInInspector] public float direction = 1f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction * speed, 0f);
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BossController boss = other.GetComponentInParent<BossController>();

        if (boss != null)
        {
            boss.TakeDamage(damage);
            FxManager.Spawn(Fx.Hit, transform.position);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            FxManager.Spawn(Fx.Death, other.transform.position);
            FxManager.SmallShake();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
