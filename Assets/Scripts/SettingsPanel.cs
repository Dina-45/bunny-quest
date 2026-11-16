using UnityEngine;
using UnityEngine.UI;


public class SettingsPanel : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        Refresh();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void Refresh()
    {
        volumeSlider.SetValueWithoutNotify(SaveSystem.Data.musicVol);
        AudioListener.volume = SaveSystem.Data.musicVol;
    }

    void OnVolumeChanged(float v)
    {
        SaveSystem.Data.musicVol = v;
        AudioListener.volume = v;
        SaveSystem.Save();
    }
}