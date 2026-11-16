using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "BunnyQuest/Boss Stats")]
public class BossStats : ScriptableObject
{
    [Header("Health")]
    public int maxHp;
    [Range(0f, 1f)] public float phase2HealthFraction;

    [Header("Timing")]
    public float startDelay;
    public float hitFlashTime;
    public float deathDelay;

    [Header("Fireballs")]
    public int fireballDamage;
    public float fireballLifetime;
    public float arenaTopY;
    public float rainFallSpeed;
    public float rainColumnSpacing;
    public float rainWarningTime;

    [Header("Phase 1")]
    public int p1FanCount;
    public float p1FanStep;
    public float p1FanSpeed;
    public float p1PauseAfterFan;
    public int p1RainColumns;
    public float p1PauseAfterRain;

    [Header("Phase 2")]
    public int p2Volleys;
    public int p2FanCount;
    public float p2FanStep;
    public float p2FanSpeed;
    public float p2PauseBetweenVolleys;
    public int p2RainColumns;
    public float p2PauseAfterRain;
}