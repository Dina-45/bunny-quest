using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ItemValues items;
    public LevelData level;          // данные текущего уровня
    public TMP_Text coinText;
    public int coins;
    public int score;

    public int totalCoins { get { return level != null ? level.totalCoins : 0; } }

    void Awake()
    {
        Instance = this;
        if (items == null) Debug.LogError("GameManager: assign ItemValues", this);
        if (level == null) Debug.LogError("GameManager: assign LevelData", this);
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