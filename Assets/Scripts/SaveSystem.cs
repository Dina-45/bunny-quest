using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    static SaveData data;

    static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "save.json"); }
    }

    public static SaveData Data
    {
        get
        {
            if (data == null) Load();
            return data;
        }
    }

    public static void Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                data = JsonUtility.FromJson<SaveData>(json);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Save file is broken, using defaults: " + e.Message);
            data = null;
        }

        // нет файла, битый файл или пустые поля -> значения по умолчанию
        if (data == null) data = new SaveData();
        if (data.unlocked == null) data.unlocked = new System.Collections.Generic.List<int>();
        if (!data.unlocked.Contains(1)) data.unlocked.Add(1);
        data.musicVol = Mathf.Clamp01(data.musicVol);
    }

    public static void Save()
    {
        try
        {
            File.WriteAllText(FilePath, JsonUtility.ToJson(Data, true));
            Debug.Log("Saved to " + FilePath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not save: " + e.Message);
        }
    }

    public static void ResetProgress()
    {
        data = new SaveData();
        Save();
    }

    public static void DeleteFile()
    {
        data = null;
        try { if (File.Exists(FilePath)) File.Delete(FilePath); }
        catch (Exception e) { Debug.LogWarning(e.Message); }
    }

    public static string ReadRaw()
    {
        return File.Exists(FilePath) ? File.ReadAllText(FilePath) : "(no save file)";
    }
}