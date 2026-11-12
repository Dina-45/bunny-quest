using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "BunnyQuest/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Health")]
    public int maxHealth = 3;

    [Header("Shooting")]
    public float shootCooldown = 0.4f;
    public float spawnOffset = 0.6f;
    public float bulletSpeed = 5f;
    public float bulletLifetime = 2.5f;
    public int bulletDamage = 1;
}