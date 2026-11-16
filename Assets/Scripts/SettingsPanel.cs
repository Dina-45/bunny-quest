using UnityEngine;
using UnityEngine.UI;

// Окно настроек: открывается кнопкой в меню, громкость сохраняется сразу при изменении.
// Скрипт висит на объекте, который ВСЕГДА активен (Settings), а прячется только панель.
public class SettingsPanel : MonoBehaviour
{
    public GameObject panel;        // всё окно настроек вместе с затемнением
    public Slider volumeSlider;

    void Start()
    {
        panel.SetActive(false);     // меню открывается без окна
        Refresh();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void Open()
    {
        Refresh();
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
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