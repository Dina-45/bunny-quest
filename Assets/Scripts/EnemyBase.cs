using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Vision")]
    public LayerMask obstacleMask;

    protected abstract EnemyStats BaseStats { get; }

    protected Transform player;
    protected Health playerHealth;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected int facing = 1;

    protected virtual void Start()
    {
        if (BaseStats == null)
        {
            Debug.LogError("Stats missing on " + name, this);
            enabled = false;
            return;
        }

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerHealth = p.GetComponent<Health>();
        }
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected bool CanSeePlayer()
    {
        if (player == null) return false;
        if (!sr.isVisible) return false;                         // за экраном не реагирует

        Vector2 toPlayer = player.position - transform.position;
        if (toPlayer.magnitude > BaseStats.sightRange) return false;   // слишком далеко

        float angle = Vector2.Angle(new Vector2(facing, 0), toPlayer);
        if (angle > BaseStats.sightAngle / 2f) return false;           // не в поле зрения

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
        return hit.collider == null;                             // стена между ними: не видит
    }

    protected void Face(int dir)
    {
        facing = dir;
        sr.flipX = dir < 0;
    }

    protected void Flash(Color c) { sr.color = c; }

    protected void SetAnimSpeed(float s)
    {
        if (anim != null) anim.speed = s;
    }
}