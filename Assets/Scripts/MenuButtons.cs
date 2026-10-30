using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void LoadScene(int index) { SceneManager.LoadScene(index); }
    public void Quit() { Application.Quit(); }
}