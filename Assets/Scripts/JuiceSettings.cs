using UnityEngine;

[CreateAssetMenu(fileName = "JuiceSettings", menuName = "BunnyQuest/Juice Settings")]
public class JuiceSettings : ScriptableObject
{
    [Header("Before / after switch")]
    public bool effectsOn = true;

    [Header("Particles")]
    public FxEntry[] particles;

    [Header("Hit stop")]
    public float hitStopTime = 0.05f;

    [Header("Hit flash (Shader Graph)")]
    public float flashTime = 0.12f;

    [Header("Screen shake")]
    public float smallShakeStrength = 0.08f;
    public float smallShakeTime = 0.12f;
    public float bigShakeStrength = 0.2f;
    public float bigShakeTime = 0.25f;

    [Header("Landing dust")]
    public float landMinSpeed = 4f;     

    [System.Serializable]
    public class FxEntry
    {
        public Fx id;
        public GameObject prefab;
    }

    public GameObject Get(Fx id)
    {
        if (particles == null) return null;
        foreach (FxEntry e in particles)
            if (e != null && e.id == id) return e.prefab;
        return null;
    }
}