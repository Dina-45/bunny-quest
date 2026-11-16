using UnityEngine;

[CreateAssetMenu(fileName = "Level_01", menuName = "BunnyQuest/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public string sceneName;
    public int totalCoins;
    public float timeLimit;       
    public int unlocksLevel;       
    public string nextSceneName;   
}