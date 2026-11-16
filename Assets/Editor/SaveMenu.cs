using UnityEditor;
using UnityEngine;

public static class SaveMenu
{
    [MenuItem("Tools/Save/Delete file")]
    static void DeleteFile()
    {
        SaveSystem.DeleteFile();
        Debug.Log("Save file deleted");
    }

    [MenuItem("Tools/Save/Print file")]
    static void PrintFile()
    {
        Debug.Log(SaveSystem.ReadRaw());
    }
}