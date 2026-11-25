using UnityEngine;

public class PlatformCarry : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        if (col.transform.position.y > transform.position.y)
            col.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        if (!gameObject.activeInHierarchy) return;

        if (col.transform.parent == transform)
            col.transform.SetParent(null);
    }
}