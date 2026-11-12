using UnityEngine;

[CreateAssetMenu(fileName = "PatrollerStats", menuName = "BunnyQuest/Patroller Stats")]
public class PatrollerStats : EnemyStats
{
    [Header("Movement")]
    public float patrolSpeed;
    public float chaseSpeed;
    public float patrolDistance;
    public float leash;

    [Header("Attack timing")]
    public float attackRange;
    public float hitHeight;
    public float windupTime;
    public float cooldownTime;

    [Header("Animation")]
    public float chaseAnimSpeed;
}