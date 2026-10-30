using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector2 direction = Vector2.left;
    public float speed = 5f;
    public int damage = 1;

    void Start() { Destroy(gameObject, 6f); }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<Health>().TakeDamage(damage); // имя метода из твоего Health.cs
        Destroy(gameObject);
    }
}