using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_Text coinText;
    public int totalCoins = 4;   
    public int coins;            

    void Awake() { Instance = this; }

    void Start() { UpdateText(); }

    public void AddCoin()
    {
        coins++;
        UpdateText();
    }

   void UpdateText()
   {
    if (coinText == null) return;
    coinText.text = "Coins: " + coins + " / " + totalCoins;
   } 
}