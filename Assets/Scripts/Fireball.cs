using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector2 direction = Vector2.left;
    public float speed;
    public int damage;
    public float lifetime;
    public LayerMask groundMask;

    void Start() { Destroy(gameObject, lifetime); }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out Health h)) h.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (((1 << other.gameObject.layer) & groundMask) != 0)
            Destroy(gameObject);
    }
}