using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ItemValues items;
    public TMP_Text coinText;
    public int totalCoins;   // цель уровня; позже её будет ставить LevelData
    public int coins;
    public int score;

    void Awake()
    {
        Instance = this;
        if (items == null) Debug.LogError("GameManager: assign ItemValues", this);
    }

    void Start() { UpdateText(); }

    public void AddCoin()
    {
        coins++;
        score += items.coinScore;
        UpdateText();
    }

    void UpdateText()
    {
        if (coinText == null) return;
        coinText.text = "Coins: " + coins + " / " + totalCoins;
    }
}