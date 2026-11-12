using UnityEngine;

[CreateAssetMenu(fileName = "ShooterStats", menuName = "BunnyQuest/Shooter Stats")]
public class ShooterStats : EnemyStats
{
    [Header("Movement")]
    public float moveSpeed;
    public float minDistance;
    public float maxDistance;
    public float leash;

    [Header("Attack timing")]
    public float aimTime;
    public float cooldownTime;

    [Header("Fireball")]
    public float fireballSpeed;
    public float fireballLifetime;
    public float fireballSpawnOffset;
}