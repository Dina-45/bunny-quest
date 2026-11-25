
using System.Collections;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public static FxManager Instance { get; private set; }

    public JuiceSettings settings;

    bool hitStopping;

    public static float LandMinSpeed => Instance != null && Instance.settings != null ? Instance.settings.landMinSpeed : 999f;

    void Awake() { Instance = this; }

    static bool On => Instance != null && Instance.settings != null && Instance.settings.effectsOn;

    public static void Spawn(Fx id, Vector3 position)
    {
        if (!On) return;
        GameObject prefab = Instance.settings.Get(id);
        if (prefab != null) Instantiate(prefab, position, Quaternion.identity);
    }

    public static void SmallShake()
    {
        if (!On) return;
        CameraFollow.Shake(Instance.settings.smallShakeStrength, Instance.settings.smallShakeTime);
    }

    public static void BigShake()
    {
        if (!On) return;
        CameraFollow.Shake(Instance.settings.bigShakeStrength, Instance.settings.bigShakeTime);
    }

    public static void HitStop()
    {
        if (!On || Instance.hitStopping) return;
        if (PauseMenu.IsPaused || Time.timeScale == 0f) return;
        Instance.StartCoroutine(Instance.HitStopRoutine());
    }

    IEnumerator HitStopRoutine()
    {
        hitStopping = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(settings.hitStopTime);
        if (!PauseMenu.IsPaused) Time.timeScale = 1f;
        hitStopping = false;
    }

    void OnDisable()
    {
        if (hitStopping && !PauseMenu.IsPaused) Time.timeScale = 1f;
        hitStopping = false;
    }
}
