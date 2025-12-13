using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI hpText;

    private GameData data;

    private void Start()
    {
        data = GameData.Instance;

        if (data == null)
        {
            Debug.LogError("GameData Instance not found!");
        }
    }

    private void Update()
    {
        if (data == null) return;

        // Constantly update UI
        nameText.text = $"Name: {data.PlayerName}";
        levelText.text = $"Level: {data.Level}";
        xpText.text = $"XP: {data.XP}";
        goldText.text = $"Gold: {data.Gold}";
        hpText.text = $"HP: {data.HP}";
    }
}
