using UnityEngine;

public abstract class EnemyStats : ScriptableObject
{
    [Header("Vision")]
    public float sightRange;
    [Range(10, 180)] public float sightAngle;

    [Header("Attack")]
    public int damage;
}