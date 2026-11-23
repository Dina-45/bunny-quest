using UnityEngine;

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