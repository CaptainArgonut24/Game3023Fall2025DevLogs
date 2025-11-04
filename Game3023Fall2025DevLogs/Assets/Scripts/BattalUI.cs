using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private GameObject canvas;

    [Header("Dialogue")]
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("HP & XP")]
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI enemyHPText;
    [SerializeField] private TextMeshProUGUI playerXPText;
    [SerializeField] private TextMeshProUGUI enemyXPText;

    [Header("Level & Names")]
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI enemyLevelText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI enemyNameText;

    [Header("Buttons")]
    [SerializeField] private Button fightButton;
    [SerializeField] private Button bagButton;
    [SerializeField] private Button specialButton;
    [SerializeField] private Button fleeButton;

    [Header("Enemy Display")]
    [SerializeField] private Image enemyImageUI; // 🟩 Enemy image UI slot in Inspector

    [Header("Enemy Data (ScriptableObject)")]
    [SerializeField] private EnemyData[] level1Enemies;
    [SerializeField, Range(0, 100)] private int level1Chance = 50;
    [SerializeField] private EnemyData[] level2Enemies;
    [SerializeField, Range(0, 100)] private int level2Chance = 20;
    [SerializeField] private EnemyData[] level3Enemies;
    [SerializeField, Range(0, 100)] private int level3Chance = 20;
    [SerializeField] private EnemyData[] level4Enemies;
    [SerializeField, Range(0, 100)] private int level4Chance = 10;
    [SerializeField] private EnemyData bossEnemy;
    [SerializeField] private bool bossMode = false;

    private EnemyData currentEnemy;

    [Header("Player Configuration")]
    [SerializeField] private string playerName = "Player";
    [SerializeField] private int playerLevel = 5;
    [SerializeField] private int playerMaxHP = 100;
    [SerializeField] private int playerCurrentHP = 100;
    [SerializeField] private int playerXP = 0;

    [Header("Player Items")]
    [SerializeField]
    private List<PlayerItem> playerItems = new List<PlayerItem>()
    {
        new PlayerItem("Teleporter", PlayerItem.EffectType.Damage, 8, 14, 2),
        new PlayerItem("Energy Drink", PlayerItem.EffectType.Heal, 12, 20, 3),
        new PlayerItem("Banana Peel", PlayerItem.EffectType.Damage, 5, 10, 2)
    };

    [System.Serializable]
    public class PlayerItem
    {
        public string itemName;
        public enum EffectType { Damage, Heal }
        public EffectType effect;
        public int minAmount;
        public int maxAmount;
        public int quantity;

        public PlayerItem(string name, EffectType fx, int min, int max, int qty)
        {
            itemName = name;
            effect = fx;
            minAmount = min;
            maxAmount = max;
            quantity = qty;
        }
    }

    [Header("Player Special Ability")]
    public PlayerSpecial playerSpecial = PlayerSpecial.Overdrive;
    public enum PlayerSpecial { Overdrive, ShieldBash, CriticalStrike, HealingBurst, EnergyWave }

    [Header("Usage Limits")]
    [SerializeField] private int playerMaxBagUses = 3;
    [SerializeField] private int playerMaxSpecialUses = 2;
    [SerializeField] private int playerSpecialCooldownTurns = 3;

    [Header("Turn Settings")]
    [Range(0, 100)] public int playerStartChance = 90;

    private int playerBagUses = 0;
    private int playerSpecialUsesRemaining = 0;
    private int playerSpecialCooldown = 0;

    private int enemyHP;
    private int enemyLevel;
    private string enemyName;
    private int enemyXP = 0;

    private bool playerTurn = true;
    private bool battleOver = false;

    [Header("Difficulty")]
    public DifficultyMode difficulty = DifficultyMode.Medium;
    public enum DifficultyMode { Easy, Medium, Hard }

    private float difficultyDamageMultiplier = 1.0f;
    private float enemyAggressionMultiplier = 1.0f;
    private int xpPerAttack = 5;

    private void Start()
    {
        ApplyDifficultySettings();
        InitializePlayer();
        PickEnemy();
        InitializeEnemyData();
        SetupButtons();

        playerTurn = Random.Range(0, 100) < playerStartChance;
        dialogueText.text = playerTurn ? $"You start first!" : $"{enemyName} strikes first!";
        UpdateUI();
    }

    private void InitializePlayer()
    {
        playerCurrentHP = Mathf.Min(playerCurrentHP, playerMaxHP);
        playerSpecialUsesRemaining = playerMaxSpecialUses;
        playerBagUses = 0;
        playerSpecialCooldown = 0;
    }

    private void ApplyDifficultySettings()
    {
        switch (difficulty)
        {
            case DifficultyMode.Easy:
                difficultyDamageMultiplier = 0.85f;
                enemyAggressionMultiplier = 0.8f;
                xpPerAttack = 3;
                break;
            case DifficultyMode.Medium:
                difficultyDamageMultiplier = 1.0f;
                enemyAggressionMultiplier = 1.0f;
                xpPerAttack = 5;
                break;
            case DifficultyMode.Hard:
                difficultyDamageMultiplier = 1.25f;
                enemyAggressionMultiplier = 1.4f;
                xpPerAttack = 8;
                break;
        }
    }

    private void PickEnemy()
    {
        if (bossMode && bossEnemy != null) { currentEnemy = bossEnemy; return; }

        int roll = Random.Range(0, 100);
        if (roll < level1Chance && level1Enemies.Length > 0)
            currentEnemy = level1Enemies[Random.Range(0, level1Enemies.Length)];
        else if (roll < level1Chance + level2Chance && level2Enemies.Length > 0)
            currentEnemy = level2Enemies[Random.Range(0, level2Enemies.Length)];
        else if (roll < level1Chance + level2Chance + level3Chance && level3Enemies.Length > 0)
            currentEnemy = level3Enemies[Random.Range(0, level3Enemies.Length)];
        else if (level4Enemies.Length > 0)
            currentEnemy = level4Enemies[Random.Range(0, level4Enemies.Length)];
    }

    private void InitializeEnemyData()
    {
        if (currentEnemy != null)
        {
            enemyHP = currentEnemy.HP;
            enemyLevel = currentEnemy.level;
            enemyName = currentEnemy.enemyName;
            enemyXP = currentEnemy.XP;

            // 🟩 Apply enemy sprite and color
            if (enemyImageUI != null)
            {
                enemyImageUI.sprite = currentEnemy.image;
                enemyImageUI.color = currentEnemy.color;
            }
        }
        else
        {
            enemyHP = 50; enemyLevel = 1; enemyName = "Wild Enemy";
        }
    }

    private void SetupButtons()
    {
        fightButton.onClick.AddListener(OnFight);
        bagButton.onClick.AddListener(OnUseItem);
        specialButton.onClick.AddListener(OnSpecial);
        if (fleeButton != null) fleeButton.onClick.AddListener(OnFlee);
    }

    private void UpdateUI()
    {
        playerHPText.text = $"HP: {playerCurrentHP}/{playerMaxHP}";
        enemyHPText.text = $"HP: {enemyHP}";
        playerLevelText.text = $"Lv. {playerLevel}";
        enemyLevelText.text = $"Lv. {enemyLevel}";
        playerNameText.text = playerName;
        enemyNameText.text = enemyName;
        playerXPText.text = $"XP: {playerXP}";
        enemyXPText.text = $"XP: {enemyXP}";
    }

    // ===== PLAYER ACTIONS =====
    private void OnFight()
    {
        if (!playerTurn || battleOver) return;
        int dmg = Mathf.RoundToInt(Random.Range(10, 20) * difficultyDamageMultiplier);
        dialogueText.text = $"You attack and deal {dmg} damage!";
        enemyHP = Mathf.Max(0, enemyHP - dmg);
        playerXP += xpPerAttack;
        enemyXP = Mathf.Max(0, enemyXP - xpPerAttack);
        EndOrContinue();
    }

    private void OnUseItem()
    {
        if (!playerTurn || battleOver) return;
        var usable = playerItems.FindAll(i => i.quantity > 0);
        if (usable.Count == 0)
        {
            dialogueText.text = "You have no more items!";
            StartCoroutine(SwitchTurn());
            return;
        }

        var item = usable[Random.Range(0, usable.Count)];
        item.quantity--;

        if (item.effect == PlayerItem.EffectType.Damage)
        {
            int dmg = Random.Range(item.minAmount, item.maxAmount);
            enemyHP = Mathf.Max(0, enemyHP - dmg);
            dialogueText.text = $"You used {item.itemName} and dealt {dmg} damage!";
        }
        else
        {
            int heal = Random.Range(item.minAmount, item.maxAmount);
            playerCurrentHP = Mathf.Min(playerMaxHP, playerCurrentHP + heal);
            dialogueText.text = $"You used {item.itemName} and healed {heal} HP!";
        }

        UpdateUI();
        EndOrContinue();
    }

    private void OnSpecial()
    {
        if (!playerTurn || battleOver) return;
        if (playerSpecialUsesRemaining <= 0)
        {
            dialogueText.text = "No special uses remaining!";
            return;
        }

        playerSpecialUsesRemaining--;
        int dmg = Mathf.RoundToInt(Random.Range(25, 35) * difficultyDamageMultiplier);
        enemyHP = Mathf.Max(0, enemyHP - dmg);
        dialogueText.text = $"You unleashed {playerSpecial} and dealt {dmg} damage!";
        UpdateUI();
        EndOrContinue();
    }

    private void OnFlee()
    {
        if (!playerTurn || battleOver) return;
        bool escaped = Random.value < 0.5f;
        if (escaped)
        {
            dialogueText.text = "You successfully fled! Exiting in 5 seconds...";
            battleOver = true;
            StartCoroutine(ExitAfterDelay(5f)); // 🟩 Wait 5 seconds before quit
        }
        else
        {
            dialogueText.text = "You failed to flee!";
            StartCoroutine(SwitchTurn());
        }
    }

    private IEnumerator ExitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 🟢 Stops Play Mode in Editor
#else
        Application.Quit(); // 🟢 Quits build
#endif
    }

    // ===== ENEMY TURN =====
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        if (battleOver) yield break;

        int choice = Random.Range(0, 3);
        switch (choice)
        {
            case 0:
                int dmg = Mathf.RoundToInt(Random.Range(8, 15) * difficultyDamageMultiplier);
                playerCurrentHP = Mathf.Max(0, playerCurrentHP - dmg);
                dialogueText.text = $"{enemyName} attacks and deals {dmg} damage!";
                break;
            case 1:
                var item = currentEnemy.PickWeightedItem();
                if (item != null)
                {
                    if (item.effect == EnemyData.AttackItem.ItemEffectType.Damage)
                    {
                        int edmg = Random.Range(item.minAmount, item.maxAmount);
                        playerCurrentHP = Mathf.Max(0, playerCurrentHP - edmg);
                        dialogueText.text = $"{enemyName} used {item.itemName} and dealt {edmg} damage!";
                    }
                    else
                    {
                        int heal = Random.Range(item.minAmount, item.maxAmount);
                        enemyHP = Mathf.Min(currentEnemy.HP, enemyHP + heal);
                        dialogueText.text = $"{enemyName} used {item.itemName} and healed {heal} HP!";
                    }
                }
                break;
            case 2:
                int specialDmg = Mathf.RoundToInt(Random.Range(15, 25) * difficultyDamageMultiplier);
                playerCurrentHP = Mathf.Max(0, playerCurrentHP - specialDmg);
                dialogueText.text = $"{enemyName} used {currentEnemy.specialAttack} and dealt {specialDmg} damage!";
                break;
        }

        UpdateUI();
        EndOrContinue();
    }

    private void EndOrContinue()
    {
        UpdateUI();
        if (enemyHP <= 0)
        {
            dialogueText.text = $"{enemyName} was defeated! You win!";
            battleOver = true;
        }
        else if (playerCurrentHP <= 0)
        {
            dialogueText.text = $"You fainted! {enemyName} wins!";
            battleOver = true;
        }
        else
        {
            StartCoroutine(SwitchTurn());
        }
    }

    private IEnumerator SwitchTurn()
    {
        yield return new WaitForSeconds(1f);
        playerTurn = !playerTurn;
        if (!playerTurn)
        {
            dialogueText.text = $"{enemyName}'s turn!";
            StartCoroutine(EnemyTurn());
        }
        else
        {
            dialogueText.text = "Your turn! Choose an action.";
        }
    }
}
