using UnityEngine;

public class Spikes : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (GameManager.Instance == null) return;
        Health h = other.GetComponent<Health>();
        if (h != null) h.TakeDamage(GameManager.Instance.items.spikeDamage);
    }
}