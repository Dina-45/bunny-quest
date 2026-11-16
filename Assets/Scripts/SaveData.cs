using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int version = 1;
    public int bestScore = 0;
    public List<int> unlocked = new List<int> { 1 };
    public float musicVol = 0.8f;

    public bool IsUnlocked(int level) { return unlocked.Contains(level); }

    public void Unlock(int level)
    {
        if (!unlocked.Contains(level)) unlocked.Add(level);
    }
}