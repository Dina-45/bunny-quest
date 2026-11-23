using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip levelMusic;
    public AudioMixerGroup musicGroup;           
    public float fadeTime = 1f;
    public string[] menuScenes = { "MainMenu", "Win" };

    AudioSource current, other;
    Coroutine fade;

    void Awake()
    {
        current = CreateSource();
        other = CreateSource();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    AudioSource CreateSource()
    {
        AudioSource s = gameObject.AddComponent<AudioSource>();
        s.outputAudioMixerGroup = musicGroup;
        s.loop = true;
        s.playOnAwake = false;
        s.volume = 0f;
        return s;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isMenu = System.Array.IndexOf(menuScenes, scene.name) >= 0;
        PlayMusic(isMenu ? menuMusic : levelMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || current.clip == clip) return;   
        AudioSource old = current;
        current = other;
        other = old;

        if (current.clip != clip || !current.isPlaying)
        {
            current.clip = clip;
            current.volume = 0f;
            current.Play();
        }

        if (fade != null) StopCoroutine(fade);
        fade = StartCoroutine(Crossfade(current, other));
    }

    IEnumerator Crossfade(AudioSource fadeIn, AudioSource fadeOut)
    {
        float inStart = fadeIn.volume;
        float outStart = fadeOut.volume;

        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)   // работает и на паузе
        {
            float k = t / fadeTime;
            fadeIn.volume = Mathf.Lerp(inStart, 1f, k);
            fadeOut.volume = Mathf.Lerp(outStart, 0f, k);
            yield return null;
        }

        fadeIn.volume = 1f;
        fadeOut.volume = 0f;
        fadeOut.Stop();
    }
}