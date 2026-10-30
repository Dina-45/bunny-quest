using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip pickupClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.AddCoin();
        AudioSource.PlayClipAtPoint(pickupClip, transform.position);
        Destroy(gameObject);
    }
}