using UnityEngine;
using TMPro;

public class HudHealth : MonoBehaviour
{
    public Health player;
    public TMP_Text hpText;

    void Update()
    {
        if (player == null || hpText == null) return;
        hpText.text = player.hp + " / " + player.maxHp;
    }
}