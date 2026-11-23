using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "BunnyQuest/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public Sfx id;
        public AudioClip[] clips;          
        [Range(0f, 1f)] public float volume = 1f;
        public bool randomPitch = true;
    }

    [Header("Pitch variation")]
    public float minPitch = 0.92f;
    public float maxPitch = 1.08f;

    public Entry[] entries;

    public Entry Get(Sfx id)
    {
        foreach (Entry e in entries)
            if (e.id == id) return e;
        return null;
    }
}