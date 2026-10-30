using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    Vector3 start;
    SpriteRenderer sr;

    void Start()
    {
        start = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = start.x + Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        sr.flipX = x < transform.position.x;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}