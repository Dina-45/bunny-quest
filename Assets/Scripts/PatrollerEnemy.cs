using UnityEngine;

public class PatrollerEnemy : EnemyBase
{
    enum State { Patrol, Chase, Windup, Cooldown }
    State state = State.Patrol;

    [Header("Movement")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float patrolDistance = 3f;
    public float leash = 4.5f;            // дальше этого от старта не уходит

    [Header("Attack")]
    public float attackRange = 1.2f;
    public float windupTime = 0.5f;
    public float cooldownTime = 1.5f;
    public int damage = 1;

    [Header("Animation")]
    public float chaseAnimSpeed = 1.5f;

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
                   : state == State.Chase ? chaseAnimSpeed : 1f);

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
        x = Mathf.Clamp(x, start.x - leash, start.x + leash);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void DoPatrol()
    {
        Move(patrolSpeed);
        if (transform.position.x > start.x + patrolDistance) Face(-1);
        if (transform.position.x < start.x - patrolDistance) Face(1);
        if (CanSeePlayer()) state = State.Chase;
    }

    void DoChase()
    {
        if (!CanSeePlayer()) { state = State.Patrol; return; }

        float dx = player.position.x - transform.position.x;
        Face(dx > 0 ? 1 : -1);
        Move(chaseSpeed);

        if (Mathf.Abs(dx) <= attackRange)
        {
            state = State.Windup;
            timer = windupTime;
            Flash(Color.red);              // предупреждение
        }
    }

    void DoWindup()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Flash(Color.white);
        float dx = Mathf.Abs(player.position.x - transform.position.x);
        float dy = Mathf.Abs(player.position.y - transform.position.y);
        if (dx <= attackRange && dy < 1f && playerHealth != null)
            playerHealth.TakeDamage(damage);

        state = State.Cooldown;
        timer = cooldownTime;
    }

    void DoCooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) state = CanSeePlayer() ? State.Chase : State.Patrol;
    }
}