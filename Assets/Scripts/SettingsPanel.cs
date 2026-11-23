using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public GameObject panel;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        panel.SetActive(false);
        Refresh();
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
    }

    public void Open()  { Refresh(); panel.SetActive(true); }
    public void Close() { panel.SetActive(false); }

    public void Refresh()
    {
        musicSlider.SetValueWithoutNotify(SaveSystem.Data.musicVol);
        sfxSlider.SetValueWithoutNotify(SaveSystem.Data.sfxVol);
    }

    void OnMusicChanged(float v)
    {
        SaveSystem.Data.musicVol = v;
        SoundManager.ApplyVolumes();
        SaveSystem.Save();
    }

    void OnSfxChanged(float v)
    {
        SaveSystem.Data.sfxVol = v;
        SoundManager.ApplyVolumes();
        SaveSystem.Save();
    }
}