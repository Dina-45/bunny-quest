using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "BunnyQuest/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 9f;

    [Header("Health")]
    public int maxHealth = 3;

    [Header("Damage feedback")]
    public int invincibilityFlashes = 10;
    public float flashInterval = 0.2f;

    [Header("Shooting")]
    public float shootCooldown = 0.4f;
    public float spawnOffset = 0.6f;
    public float bulletSpeed = 5f;
    public float bulletLifetime = 3f;
    public int bulletDamage = 1;
}