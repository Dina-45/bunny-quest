using UnityEngine;
using UnityEngine.UI;


public class UIClickSounds : MonoBehaviour
{
    void Awake()
    {
        foreach (Button b in GetComponentsInChildren<Button>(true))
            b.onClick.AddListener(() => SoundManager.Play(Sfx.Click));
    }
}