using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen { get; private set; }
    Animator anim;

    void Awake() { anim = GetComponent<Animator>(); }

    public void Open()
    {
        IsOpen = true;
        anim.SetBool("IsOpen", true);
    }
}