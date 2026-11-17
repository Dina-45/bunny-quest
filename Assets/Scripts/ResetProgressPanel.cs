using UnityEngine;

// Кнопка RESET открывает вопрос "Reset all progress?" с кнопками YES / NO.
public class ResetProgressPanel : MonoBehaviour
{
    public GameObject confirmPanel;

    void Start()
    {
        confirmPanel.SetActive(false);
    }

    public void AskReset()
    {
        confirmPanel.SetActive(true);
    }

    public void Cancel()
    {
        confirmPanel.SetActive(false);
    }

    public void ConfirmReset()
    {
        SaveSystem.ResetProgress();
        confirmPanel.SetActive(false);
    }
}