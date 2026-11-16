using UnityEngine;


public class AppSaveHook : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
        GameObject go = new GameObject("AppSaveHook");
        go.AddComponent<AppSaveHook>();
        DontDestroyOnLoad(go);
    }

    void Awake()
    {
        SaveSystem.Load();
        AudioListener.volume = SaveSystem.Data.musicVol;
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) SaveSystem.Save();
    }

    void OnApplicationQuit()
    {
        SaveSystem.Save();
    }
}