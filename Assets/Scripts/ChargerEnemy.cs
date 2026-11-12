using UnityEngine;

public class ChargerEnemy : EnemyBase
{
    enum State { Idle, Windup, Charge, Recover }
    State state = State.Idle;

    public ChargerStats stats;
    protected override EnemyStats BaseStats => stats;

    const float HomeTolerance = 0.05f;   // техническая погрешность, не баланс

    Vector3 start;
    Vector3 chargeStart;
    Vector2 chargeDir;
    float timer;
    bool hitDone;

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
            case State.Idle:    DoIdle();    break;
            case State.Windup:  DoWindup();  break;
            case State.Charge:  DoCharge();  break;
            case State.Recover: DoRecover(); break;
        }
    }

    void DoIdle()
    {
        float dx = player.position.x - transform.position.x;
        Face(dx > 0 ? 1 : -1);

        if (CanSeePlayer())
        {
            chargeDir = ((Vector2)(player.position - transform.position)).normalized;
            state = State.Windup;
            timer = stats.windupTime;
            Flash(Color.red);
        }
    }

    void DoWindup()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        chargeStart = transform.position;
        hitDone = false;
        state = State.Charge;
    }

    void DoCharge()
    {
        transform.position += (Vector3)(chargeDir * stats.chargeSpeed * Time.deltaTime);

        if (!hitDone && playerHealth != null &&
            Vector2.Distance(player.position, transform.position) <= stats.hitRadius)
        {
            playerHealth.TakeDamage(stats.damage);
            hitDone = true;
        }

        bool wallAhead = Physics2D.Raycast(
            transform.position, chargeDir, stats.wallCheckDistance, obstacleMask);
        bool tooFar = Vector2.Distance(transform.position, chargeStart) >= stats.chargeDistance;

        if (wallAhead || tooFar)
        {
            Flash(Color.white);
            state = State.Recover;
            timer = stats.recoverTime;
        }
    }

    void DoRecover()
    {
        timer -= Time.deltaTime;
        transform.position = Vector3.MoveTowards(
            transform.position, start, stats.returnSpeed * Time.deltaTime);

        bool home = Vector3.Distance(transform.position, start) < HomeTolerance;
        if (timer <= 0f && home) state = State.Idle;
    }
}