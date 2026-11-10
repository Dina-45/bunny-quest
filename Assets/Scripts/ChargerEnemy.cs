using UnityEngine;

public class ChargerEnemy : EnemyBase
{
    enum State { Idle, Windup, Charge, Recover }
    State state = State.Idle;

    [Header("Attack")]
    public float windupTime = 0.7f;
    public float chargeSpeed = 7f;
    public float chargeDistance = 7f;
    public float recoverTime = 1.5f;
    public float hitRadius = 0.7f;
    public int damage = 1;

    [Header("Return")]
    public float returnSpeed = 3f;      // после атаки возвращается на своё место

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
            // цель запоминается сейчас: пока враг краснеет, можно отойти
            chargeDir = ((Vector2)(player.position - transform.position)).normalized;
            state = State.Windup;
            timer = windupTime;
            Flash(Color.red);               // предупреждение
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
        transform.position += (Vector3)(chargeDir * chargeSpeed * Time.deltaTime);

        if (!hitDone && playerHealth != null &&
            Vector2.Distance(player.position, transform.position) <= hitRadius)
        {
            playerHealth.TakeDamage(damage);
            hitDone = true;
        }

        bool wallAhead = Physics2D.Raycast(
            transform.position, chargeDir, 0.6f, obstacleMask);
        bool tooFar = Vector2.Distance(transform.position, chargeStart) >= chargeDistance;

        if (wallAhead || tooFar)
        {
            Flash(Color.white);
            state = State.Recover;
            timer = recoverTime;
        }
    }

    void DoRecover()
    {
        timer -= Time.deltaTime;
        transform.position = Vector3.MoveTowards(
            transform.position, start, returnSpeed * Time.deltaTime);

        bool home = Vector3.Distance(transform.position, start) < 0.05f;
        if (timer <= 0f && home) state = State.Idle;
    }
}