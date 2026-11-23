using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelSelectMenu : MonoBehaviour
{
    [System.Serializable]
    public class Entry
    {
        public LevelData level;
        public Button button;
        public GameObject lockIcon;
    }

    public GameObject panel;
    public Entry[] entries;
    public TMP_Text bestScoreText;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Open()
    {
        Refresh();                 // каждый раз читаем свежий save (например, после Reset)
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void Refresh()
    {
        SaveData d = SaveSystem.Data;

        foreach (Entry e in entries)
        {
            if (e.level == null || e.button == null) continue;

            bool open = d.IsUnlocked(e.level.levelNumber);
            e.button.interactable = open;
            if (e.lockIcon != null) e.lockIcon.SetActive(!open);

            string scene = e.level.sceneName;
            e.button.onClick.RemoveAllListeners();
            e.button.onClick.AddListener(() => SceneManager.LoadScene(scene));
        }

        if (bestScoreText != null) bestScoreText.text = "Best: " + d.bestScore;
    }
}