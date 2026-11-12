using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    enum State { Idle, Aim, Cooldown }
    State state = State.Idle;

    public ShooterStats stats;
    protected override EnemyStats BaseStats => stats;

    public GameObject fireballPrefab;
    public AudioClip shootClip;

    Vector3 start;
    float timer;

    protected override void Start()
    {
        base.Start();
        start = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        switch (state)
        {
            case State.Idle:     DoIdle();     break;
            case State.Aim:      DoAim();      break;
            case State.Cooldown: DoCooldown(); break;
        }
    }

    void Reposition()
    {
        float dx = player.position.x - transform.position.x;
        float dist = Mathf.Abs(dx);

        int dir = 0;
        if (dist < stats.minDistance)      dir = dx > 0 ? -1 : 1;
        else if (dist > stats.maxDistance) dir = dx > 0 ? 1 : -1;
        if (dir == 0) return;

        float x = transform.position.x + dir * stats.moveSpeed * Time.deltaTime;
        x = Mathf.Clamp(x, start.x - stats.leash, start.x + stats.leash);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void DoIdle()
    {
        float dx = player.position.x - transform.position.x;
        Face(dx > 0 ? 1 : -1);
        Reposition();

        if (CanSeePlayer())
        {
            state = State.Aim;
            timer = stats.aimTime;
            Flash(Color.red);
        }
    }

    void DoAim()
    {
        if (!CanSeePlayer())
        {
            Flash(Color.white);
            state = State.Idle;
            return;
        }

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Fire();
        Flash(Color.white);
        state = State.Cooldown;
        timer = stats.cooldownTime;
    }

    void DoCooldown()
    {
        Reposition();
        timer -= Time.deltaTime;
        if (timer <= 0f) state = State.Idle;
    }

    void Fire()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        Vector3 spawn = transform.position + (Vector3)(dir * stats.fireballSpawnOffset);
        GameObject f = Instantiate(fireballPrefab, spawn, Quaternion.identity);
        f.transform.right = dir;

        Fireball fb = f.GetComponent<Fireball>();
        fb.direction = dir;
        fb.speed = stats.fireballSpeed;
        fb.lifetime = stats.fireballLifetime;
        fb.damage = stats.damage;

        if (shootClip != null)
            AudioSource.PlayClipAtPoint(shootClip, transform.position, 0.6f);
    }
}