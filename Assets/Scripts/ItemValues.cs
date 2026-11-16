using UnityEngine;

[CreateAssetMenu(fileName = "ItemValues", menuName = "BunnyQuest/Item Values")]
public class ItemValues : ScriptableObject
{
    [Header("Score")]
    public int coinScore;
    public int levelClearBonus;

    [Header("Pickups and hazards")]
    public int carrotHeal;
    public int spikeDamage;
}