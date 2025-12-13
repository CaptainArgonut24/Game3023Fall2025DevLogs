using UnityEngine;
using TMPro;

public class AchievementsSET : MonoBehaviour
{
    [Header("Achievements UI Text References")]
    public TextMeshProUGUI Achievement_1;
    public TextMeshProUGUI Achievement_2;
    public TextMeshProUGUI Achievement_3;
    public TextMeshProUGUI Achievement_4;
    public TextMeshProUGUI Achievement_5;
    public TextMeshProUGUI Achievement_6;
    public TextMeshProUGUI Achievement_7;

    private GameData data;

    private Color completedColor = Color.green;
    private Color incompleteColor = Color.white;

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

        UpdateAchievement(Achievement_1,
            $"Win 10 battles - 1000G: {data.Wins}/10",
            data.Wins, 10);

        UpdateAchievement(Achievement_2,
            $"Lose 10 battles - 10G: {data.Lost}/10",
            data.Lost, 10);

        UpdateAchievement(Achievement_3,
            $"Collect 5 shards - 1000G: {data.Teleporter}/5",
            data.Teleporter, 5);

        UpdateAchievement(Achievement_4,
            $"Collect 5 Red shards - 1000G: {data.RSHARDS}/5",
            data.RSHARDS, 5);

        UpdateAchievement(Achievement_5,
            $"Collect 5 Green shards - 1000G: {data.GSHARDS}/5",
            data.GSHARDS, 5);

        UpdateAchievement(Achievement_6,
            $"Collect 5 Blue shards - 1000G: {data.BSHARDS}/5",
            data.BSHARDS, 5);

        UpdateAchievement(Achievement_7,
            $"Defeat the Boss - {data.BOSSSHARDS}/1",
            data.BOSSSHARDS, 1);
    }

    private void UpdateAchievement(
        TextMeshProUGUI text,
        string displayText,
        int currentValue,
        int requiredValue)
    {
        text.text = displayText;
        text.color = currentValue >= requiredValue
            ? completedColor
            : incompleteColor;
    }
}
