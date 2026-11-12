using UnityEngine;

public class PatrollerEnemy : EnemyBase
{
    enum State { Patrol, Chase, Windup, Cooldown }
    State state = State.Patrol;

    public PatrollerStats stats;
    protected override EnemyStats BaseStats => stats;

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

        SetAnimSpeed(state == State.Windup || state == State.Cooldown ? 0f
                   : state == State.Chase ? stats.chaseAnimSpeed : 1f);

        switch (state)
        {
            case State.Patrol:   DoPatrol();   break;
            case State.Chase:    DoChase();    break;
            case State.Windup:   DoWindup();   break;
            case State.Cooldown: DoCooldown(); break;
        }
    }

    void Move(float speed)
    {
        float x = transform.position.x + facing * speed * Time.deltaTime;
        x = Mathf.Clamp(x, start.x - stats.leash, start.x + stats.leash);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void DoPatrol()
    {
        Move(stats.patrolSpeed);
        if (transform.position.x > start.x + stats.patrolDistance) Face(-1);
        if (transform.position.x < start.x - stats.patrolDistance) Face(1);
        if (CanSeePlayer()) state = State.Chase;
    }

    void DoChase()
    {
        if (!CanSeePlayer()) { state = State.Patrol; return; }

        float dx = player.position.x - transform.position.x;
        Face(dx > 0 ? 1 : -1);
        Move(stats.chaseSpeed);

        if (Mathf.Abs(dx) <= stats.attackRange)
        {
            state = State.Windup;
            timer = stats.windupTime;
            Flash(new Color(0.35f, 0.35f, 0.35f));
        }
    }

    void DoWindup()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Flash(Color.white);
        float dx = Mathf.Abs(player.position.x - transform.position.x);
        float dy = Mathf.Abs(player.position.y - transform.position.y);
        if (dx <= stats.attackRange && dy < stats.hitHeight && playerHealth != null)
            playerHealth.TakeDamage(stats.damage);

        state = State.Cooldown;
        timer = stats.cooldownTime;
    }

    void DoCooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) state = CanSeePlayer() ? State.Chase : State.Patrol;
    }
}