public static class ProgressTracker
{
    // Вызывается, когда уровень пройден
    public static void CompleteLevel(int unlocksLevel)
    {
        GameManager gm = GameManager.Instance;
        if (gm == null) return;

        int bonus = gm.items != null ? gm.items.levelClearBonus : 0;
        int levelScore = gm.score + bonus;

        SaveData data = SaveSystem.Data;
        if (levelScore > data.bestScore) data.bestScore = levelScore;
        if (unlocksLevel > 0) data.Unlock(unlocksLevel);

        SaveSystem.Save();
    }
}