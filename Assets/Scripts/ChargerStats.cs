using UnityEngine;

[CreateAssetMenu(fileName = "ChargerStats", menuName = "BunnyQuest/Charger Stats")]
public class ChargerStats : EnemyStats
{
    [Header("Attack timing")]
    public float windupTime;
    public float recoverTime;

    [Header("Dive")]
    public float chargeSpeed;
    public float chargeDistance;
    public float hitRadius;
    public float wallCheckDistance;
    public float returnSpeed;
}