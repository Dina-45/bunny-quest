using UnityEngine;

public class CarrotPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Health h = other.GetComponent<Health>();
        if (h == null) return;

        h.Heal(1);            
        Destroy(gameObject);  
    }
}