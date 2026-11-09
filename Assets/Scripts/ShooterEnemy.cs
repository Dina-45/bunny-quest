using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    enum State { Idle, Aim, Cooldown }
    State state = State.Idle;

    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float minDistance = 2.5f;   // ближе этого отступает
    public float maxDistance = 5f;     // дальше этого подходит
    public float leash = 4f;           // от стартовой точки не уходит дальше

    [Header("Attack")]
    public GameObject fireballPrefab;
    public AudioClip shootClip;
    public float aimTime = 0.6f;
    public float cooldownTime = 1.6f;
    public float fireballSpeed = 4f;
    public int damage = 1;

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
        if (dist < minDistance)      dir = dx > 0 ? -1 : 1;   // отступает
        else if (dist > maxDistance) dir = dx > 0 ? 1 : -1;   // подходит
        if (dir == 0) return;

        float x = transform.position.x + dir * moveSpeed * Time.deltaTime;
        x = Mathf.Clamp(x, start.x - leash, start.x + leash);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void DoIdle()
    {
        float dx = player.position.x - transform.position.x;
        Face(dx > 0 ? 1 : -1);               // следит за игроком
        Reposition();

        if (CanSeePlayer())
        {
            state = State.Aim;
            timer = aimTime;
            Flash(Color.red);                // предупреждение
        }
    }

    void DoAim()
    {
        if (!CanSeePlayer())                 // игрок спрятался: отмена
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
        timer = cooldownTime;
    }

    void DoCooldown()
    {
        Reposition();                        // пока перезаряжается, двигается
        timer -= Time.deltaTime;
        if (timer <= 0f) state = State.Idle;
    }

    void Fire()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        Vector3 spawn = transform.position + (Vector3)(dir * 0.6f);   // чтобы не задеть себя
        GameObject f = Instantiate(fireballPrefab, spawn, Quaternion.identity);
        f.transform.right = dir;                                       // поворот спрайта по полёту

        Fireball fb = f.GetComponent<Fireball>();
        fb.direction = dir;
        fb.speed = fireballSpeed;
        fb.damage = damage;

        if (shootClip != null)
            AudioSource.PlayClipAtPoint(shootClip, transform.position, 0.6f);
    }
}