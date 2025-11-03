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
    [SerializeField] private Button healButton;
    [SerializeField] private Button specialButton;

    [Header("Animations & Visuals")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private Image playerPortraitImage;
    [SerializeField] private Image enemyPortraitImage;

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

    [Header("Player Defaults")]
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
        new PlayerItem("Disguise Kit", PlayerItem.EffectType.Damage, 6, 12, 2),
        new PlayerItem("Banana Peel", PlayerItem.EffectType.Damage, 5, 10, 2),
        new PlayerItem("C4 (Totally Safe)", PlayerItem.EffectType.Damage, 12, 18, 1),
        new PlayerItem("Invisibility Potion", PlayerItem.EffectType.Heal, 8, 14, 2),
        new PlayerItem("Rubber Duck", PlayerItem.EffectType.Damage, 6, 9, 3)
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
    [SerializeField] private int playerMaxHealUses = 2;
    [SerializeField] private int playerMaxSpecialUses = 2;
    [SerializeField] private int playerSpecialCooldownTurns = 3;

    private int playerBagUses = 0;
    private int playerHealUses = 0;
    private int playerSpecialUsesRemaining = 0;
    private int playerSpecialCooldown = 0;

    private int enemyHP;
    private int enemyLevel;
    private string enemyName;
    private int enemySpecialCooldown = 0;
    private int enemyBagUses = 0;
    private int enemyHealUses = 0;
    private int enemySpecialUses = 0;
    private int enemySkipTurnsUsed = 0;

    private bool playerTurn = true;

    [Header("Difficulty")]
    public DifficultyMode difficulty = DifficultyMode.Medium;
    public enum DifficultyMode { Easy, Medium, Hard }

    private float difficultyDamageMultiplier = 1.0f;
    private float enemyAggressionMultiplier = 1.0f;

    private void Awake() => ApplyDifficultySettings();

    private void Start()
    {
        playerCurrentHP = Mathf.Min(playerCurrentHP, playerMaxHP);
        playerSpecialUsesRemaining = playerMaxSpecialUses;
        playerBagUses = 0;
        playerHealUses = 0;
        playerSpecialCooldown = 0;

        PickEnemy();
        InitializeEnemyData();
        SetupButtons();
        dialogueText.text = $"A wild {enemyName} appeared!";
        UpdateUI();
    }

    private void ApplyDifficultySettings()
    {
        switch (difficulty)
        {
            case DifficultyMode.Easy:
                difficultyDamageMultiplier = 0.85f;
                enemyAggressionMultiplier = 0.8f;
                break;
            case DifficultyMode.Medium:
                difficultyDamageMultiplier = 1.0f;
                enemyAggressionMultiplier = 1.0f;
                break;
            case DifficultyMode.Hard:
                difficultyDamageMultiplier = 1.25f;
                enemyAggressionMultiplier = 1.4f;
                break;
        }
    }

    private void PickEnemy()
    {
        if (bossMode && bossEnemy != null)
        {
            currentEnemy = bossEnemy;
            return;
        }

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
            enemySpecialCooldown = currentEnemy.specialAttackCooldown;

            if (enemyPortraitImage && currentEnemy.image)
                enemyPortraitImage.sprite = currentEnemy.image;
        }
        else
        {
            enemyHP = 50;
            enemyLevel = 1;
            enemyName = "Wild Enemy";
        }
    }

    private void SetupButtons()
    {
        fightButton.onClick.AddListener(OnFight);
        bagButton.onClick.AddListener(UseRandomPlayerItem);
        healButton.onClick.AddListener(OnHealPressed);
        specialButton.onClick.AddListener(OnSpecialAttack);
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
        enemyXPText.text = $"XP: {(currentEnemy != null ? currentEnemy.XP : 0)}";
    }

    private void OnFight()
    {
        if (!playerTurn) return;
        StartCoroutine(PlayerAttack());
    }

    private void UseRandomPlayerItem()
    {
        if (!playerTurn) return;
        if (playerBagUses >= playerMaxBagUses)
        {
            dialogueText.text = "You can't use any more items this battle!";
            return;
        }

        List<PlayerItem> usable = playerItems.FindAll(i => i.quantity > 0);
        if (usable.Count == 0)
        {
            dialogueText.text = "You have no more items left! Turn wasted.";
            StartCoroutine(SwitchTurn());
            return;
        }

        PlayerItem item = usable[Random.Range(0, usable.Count)];
        item.quantity--;
        playerBagUses++;

        if (item.effect == PlayerItem.EffectType.Damage)
        {
            int dmg = Random.Range(item.minAmount, item.maxAmount + 1);
            dmg = Mathf.RoundToInt(dmg * difficultyDamageMultiplier);
            enemyHP = Mathf.Max(0, enemyHP - dmg);
            dialogueText.text = $"You used {item.itemName} and dealt {dmg} damage!";
        }
        else
        {
            int heal = Random.Range(item.minAmount, item.maxAmount + 1);
            playerCurrentHP = Mathf.Min(playerMaxHP, playerCurrentHP + heal);
            dialogueText.text = $"You used {item.itemName} and healed {heal} HP!";
        }

        UpdateUI();
        StartCoroutine(SwitchTurn());
    }

    private void OnHealPressed()
    {
        if (!playerTurn) return;
        if (playerHealUses >= playerMaxHealUses)
        {
            dialogueText.text = "You can't heal anymore this battle!";
            return;
        }

        playerHealUses++;
        int heal = Random.Range(12, 20);
        playerCurrentHP = Mathf.Min(playerMaxHP, playerCurrentHP + heal);
        dialogueText.text = $"You healed for {heal} HP!";
        UpdateUI();
        StartCoroutine(SwitchTurn());
    }

    private void OnSpecialAttack()
    {
        if (!playerTurn) return;
        if (playerSpecialUsesRemaining <= 0)
        {
            dialogueText.text = "No special uses remaining!";
            return;
        }
        if (playerSpecialCooldown > 0)
        {
            dialogueText.text = $"Special on cooldown ({playerSpecialCooldown} turns).";
            return;
        }

        playerSpecialUsesRemaining--;
        playerSpecialCooldown = playerSpecialCooldownTurns;

        int dmg = playerSpecial switch
        {
            PlayerSpecial.Overdrive => Random.Range(25, 35),
            PlayerSpecial.ShieldBash => Random.Range(18, 26),
            PlayerSpecial.CriticalStrike => Random.Range(28, 40),
            PlayerSpecial.HealingBurst => Random.Range(0, 0),
            _ => Random.Range(20, 30)
        };

        if (playerSpecial == PlayerSpecial.HealingBurst)
        {
            int heal = Random.Range(20, 30);
            playerCurrentHP = Mathf.Min(playerMaxHP, playerCurrentHP + heal);
            dialogueText.text = $"You used Healing Burst and recovered {heal} HP!";
            UpdateUI();
            StartCoroutine(SwitchTurn());
            return;
        }

        dmg = Mathf.RoundToInt(dmg * difficultyDamageMultiplier);
        StartCoroutine(DealDamageToEnemy(dmg));
    }

    private IEnumerator PlayerAttack()
    {
        dialogueText.text = "You attack!";
        yield return new WaitForSeconds(0.8f);
        int dmg = Random.Range(10, 20);
        dmg = Mathf.RoundToInt(dmg * difficultyDamageMultiplier);
        StartCoroutine(DealDamageToEnemy(dmg));
    }

    private IEnumerator DealDamageToEnemy(int dmg)
    {
        enemyHP = Mathf.Max(0, enemyHP - dmg);
        dialogueText.text = $"{enemyName} took {dmg} damage!";
        UpdateUI();
        yield return new WaitForSeconds(1f);

        if (enemyHP <= 0)
        {
            dialogueText.text = $"{enemyName} fainted! You gained {currentEnemy.XP} XP!";
            playerXP += currentEnemy.XP;
            UpdateUI();
            yield break;
        }

        StartCoroutine(SwitchTurn());
    }

    private IEnumerator SwitchTurn()
    {
        yield return new WaitForSeconds(1f);
        playerTurn = !playerTurn;
        if (!playerTurn) StartCoroutine(EnemyTurn());
        else dialogueText.text = "Your turn!";
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        float hpPercent = (float)enemyHP / currentEnemy.HP;
        bool usedItem = false;

        // Try using an item
        if (currentEnemy.attackItems != null && currentEnemy.attackItems.Length > 0)
        {
            List<EnemyData.AttackItem> usable = new List<EnemyData.AttackItem>();
            foreach (var item in currentEnemy.attackItems)
                if (item.quantity > 0) usable.Add(item);

            if (usable.Count > 0)
            {
                EnemyData.AttackItem chosen = null;
                if (hpPercent < 0.4f)
                {
                    var heals = usable.FindAll(i => i.effect == EnemyData.AttackItem.ItemEffectType.Heal);
                    if (heals.Count > 0) chosen = heals[Random.Range(0, heals.Count)];
                }

                if (chosen == null)
                {
                    var dmgs = usable.FindAll(i => i.effect == EnemyData.AttackItem.ItemEffectType.Damage);
                    if (dmgs.Count > 0) chosen = dmgs[Random.Range(0, dmgs.Count)];
                }

                if (chosen != null)
                {
                    chosen.quantity--;
                    usedItem = true;
                    if (chosen.effect == EnemyData.AttackItem.ItemEffectType.Damage)
                    {
                        int dmg = Random.Range(chosen.minAmount, chosen.maxAmount + 1);
                        dmg = Mathf.RoundToInt(dmg * difficultyDamageMultiplier * enemyAggressionMultiplier);
                        playerCurrentHP = Mathf.Max(0, playerCurrentHP - dmg);
                        dialogueText.text = $"{enemyName} used {chosen.itemName} and dealt {dmg} damage!";
                    }
                    else
                    {
                        int heal = Random.Range(chosen.minAmount, chosen.maxAmount + 1);
                        enemyHP = Mathf.Min(currentEnemy.HP, enemyHP + heal);
                        dialogueText.text = $"{enemyName} used {chosen.itemName} and healed {heal} HP!";
                    }
                    UpdateUI();
                }
            }
        }

        if (!usedItem)
        {
            dialogueText.text = $"{enemyName} has no items left! Turn wasted.";
        }

        yield return new WaitForSeconds(1.5f);

        if (playerCurrentHP <= 0)
        {
            dialogueText.text = "You fainted...";
            yield break;
        }

        playerTurn = true;
        dialogueText.text = "Your turn!";
    }
}
