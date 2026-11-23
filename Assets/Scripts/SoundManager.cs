using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public SoundLibrary library;
    public AudioMixerGroup sfxGroup;

    const int Voices = 8;                 
    AudioSource[] sources;
    int next;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
        GameObject prefab = Resources.Load<GameObject>("AudioManager");
        if (prefab == null)
        {
            Debug.LogError("Missing prefab: Assets/Resources/AudioManager");
            return;
        }
        GameObject go = Instantiate(prefab);
        go.name = "AudioManager";
        DontDestroyOnLoad(go);
    }

    void Awake()
    {
        Instance = this;
        sources = new AudioSource[Voices];
        for (int i = 0; i < Voices; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].outputAudioMixerGroup = sfxGroup;
            sources[i].playOnAwake = false;
        }
    }

    // Играет звук и возвращает его длину (нужно, чтобы дождаться конца, например win).
    public static float Play(Sfx id)
    {
        if (Instance == null || Instance.library == null) return 0f;
        SoundLibrary.Entry e = Instance.library.Get(id);
        if (e == null || e.clips == null || e.clips.Length == 0) return 0f;

        AudioClip clip = e.clips[Random.Range(0, e.clips.Length)];
        AudioSource s = Instance.sources[Instance.next];
        Instance.next = (Instance.next + 1) % Voices;

        s.pitch = e.randomPitch
            ? Random.Range(Instance.library.minPitch, Instance.library.maxPitch)
            : 1f;
        s.clip = clip;
        s.volume = e.volume;
        s.Play();
        return clip.length / s.pitch;
    }
}