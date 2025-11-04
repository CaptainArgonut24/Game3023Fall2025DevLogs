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
    [SerializeField] private Image playerPortraitImage;
    [SerializeField] private Image enemyPortraitImage;

    [SerializeField] private RuntimeAnimatorController playerAnimatorController;
    [SerializeField] private RuntimeAnimatorController enemyAnimatorController;

    private Animator playerAnimator;
    private Animator enemyAnimator;

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

    [Header("Turn Settings")]
    [Range(0, 100)] public int playerStartChance = 90;

    private int playerBagUses = 0;
    private int playerHealUses = 0;
    private int playerSpecialUsesRemaining = 0;
    private int playerSpecialCooldown = 0;

    private int enemyHP;
    private int enemyLevel;
    private string enemyName;
    private int enemySpecialCooldown = 0;
    private int enemyXP = 0;

    private bool playerTurn = true;
    private bool battleOver = false;

    [Header("Difficulty")]
    public DifficultyMode difficulty = DifficultyMode.Medium;
    public enum DifficultyMode { Easy, Medium, Hard }

    private float difficultyDamageMultiplier = 1.0f;
    private float enemyAggressionMultiplier = 1.0f;
    private int xpPerAttack = 5;

    private void Awake()
    {
        ApplyDifficultySettings();

        // Automatically assign or add Animator components and assign controllers
        if (playerPortraitImage)
        {
            playerAnimator = playerPortraitImage.GetComponent<Animator>();
            if (!playerAnimator)
                playerAnimator = playerPortraitImage.gameObject.AddComponent<Animator>();

            if (playerAnimatorController)
                playerAnimator.runtimeAnimatorController = playerAnimatorController;
        }

        if (enemyPortraitImage)
        {
            enemyAnimator = enemyPortraitImage.GetComponent<Animator>();
            if (!enemyAnimator)
                enemyAnimator = enemyPortraitImage.gameObject.AddComponent<Animator>();

            if (enemyAnimatorController)
                enemyAnimator.runtimeAnimatorController = enemyAnimatorController;
        }
    }

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

        // Random turn order
        playerTurn = Random.Range(0, 100) < playerStartChance;
        dialogueText.text = playerTurn ? $"You start first!" : $"{enemyName} strikes first!";

        SetBothIdle();
        UpdateUI();
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
            enemySpecialCooldown = currentEnemy.specialAttackCooldown;
            enemyXP = currentEnemy.XP;

            if (enemyPortraitImage && currentEnemy.image)
                enemyPortraitImage.sprite = currentEnemy.image;
        }
        else
        {
            enemyHP = 50; enemyLevel = 1; enemyName = "Wild Enemy";
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
        enemyXPText.text = $"XP: {enemyXP}";
    }

    private void OnFight()
    {
        if (!playerTurn || battleOver) return;
        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        yield return StartCoroutine(PlayAnim(playerAnimator, "isFight"));
        dialogueText.text = "You attack!";
        yield return new WaitForSeconds(1f);

        int dmg = Mathf.RoundToInt(Random.Range(10, 20) * difficultyDamageMultiplier);
        yield return StartCoroutine(DealDamageToEnemy(dmg));
    }

    private IEnumerator DealDamageToEnemy(int dmg)
    {
        yield return StartCoroutine(PlayAnim(enemyAnimator, "isHit"));
        enemyHP = Mathf.Max(0, enemyHP - dmg);
        playerXP += xpPerAttack;
        enemyXP = Mathf.Max(0, enemyXP - xpPerAttack);

        dialogueText.text = $"{enemyName} took {dmg} damage!";
        UpdateUI();
        yield return new WaitForSeconds(1f);

        if (enemyHP <= 0)
        {
            yield return StartCoroutine(EndBattle(true));
            yield break;
        }

        StartCoroutine(SwitchTurn());
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(PlayAnim(enemyAnimator, "isFight"));
        dialogueText.text = $"{enemyName} attacks!";

        int dmg = Mathf.RoundToInt(Random.Range(8, 15) * difficultyDamageMultiplier * enemyAggressionMultiplier);
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(PlayAnim(playerAnimator, "isHit"));
        playerCurrentHP = Mathf.Max(0, playerCurrentHP - dmg);
        enemyXP += xpPerAttack;
        playerXP = Mathf.Max(0, playerXP - xpPerAttack);

        dialogueText.text = $"You took {dmg} damage!";
        UpdateUI();
        yield return new WaitForSeconds(1f);

        if (playerCurrentHP <= 0)
        {
            yield return StartCoroutine(EndBattle(false));
            yield break;
        }

        StartCoroutine(SwitchTurn());
    }

    private IEnumerator SwitchTurn()
    {
        yield return new WaitForSeconds(1f);
        playerTurn = !playerTurn;
        dialogueText.text = playerTurn ? "Your turn!" : $"{enemyName}'s turn!";
        if (!playerTurn) StartCoroutine(EnemyTurn());
    }

    private IEnumerator PlayAnim(Animator anim, string boolName)
    {
        if (!anim) yield break;
        anim.SetBool(boolName, true);
        yield return new WaitForSeconds(5f);
        anim.SetBool(boolName, false);
    }

    private void SetBothIdle()
    {
        if (playerAnimator) playerAnimator.SetBool("isMoving", false);
        if (enemyAnimator) enemyAnimator.SetBool("isMoving", false);
    }

    private IEnumerator EndBattle(bool playerWon)
    {
        battleOver = true;
        if (playerWon)
        {
            yield return StartCoroutine(PlayAnim(playerAnimator, "isWin"));
            yield return StartCoroutine(PlayAnim(enemyAnimator, "isDead"));
            dialogueText.text = $"{enemyName} was defeated! You won!";
        }
        else
        {
            yield return StartCoroutine(PlayAnim(playerAnimator, "isDead"));
            yield return StartCoroutine(PlayAnim(enemyAnimator, "isWin"));
            dialogueText.text = $"You fainted... {enemyName} wins!";
        }

        UpdateUI();
        yield break;
    }

    private void UseRandomPlayerItem()
    {
        if (!playerTurn || battleOver) return;
        List<PlayerItem> usable = playerItems.FindAll(i => i.quantity > 0);
        if (usable.Count == 0)
        {
            dialogueText.text = "You have no items left!";
            StartCoroutine(SwitchTurn());
            return;
        }

        PlayerItem item = usable[Random.Range(0, usable.Count)];
        item.quantity--;
        StartCoroutine(PlayAnim(playerAnimator, "isItem"));

        if (item.effect == PlayerItem.EffectType.Damage)
        {
            int dmg = Mathf.RoundToInt(Random.Range(item.minAmount, item.maxAmount) * difficultyDamageMultiplier);
            dialogueText.text = $"You used {item.itemName}!";
            StartCoroutine(DealDamageToEnemy(dmg));
        }
        else
        {
            int heal = Random.Range(item.minAmount, item.maxAmount);
            playerCurrentHP = Mathf.Min(playerMaxHP, playerCurrentHP + heal);
            dialogueText.text = $"You used {item.itemName} and healed {heal} HP!";
            UpdateUI();
            StartCoroutine(SwitchTurn());
        }
    }

    private void OnHealPressed()
    {
        if (!playerTurn || battleOver) return;
        StartCoroutine(PlayAnim(playerAnimator, "isItem"));
        int heal = Random.Range(12, 20);
        playerCurrentHP = Mathf.Min(playerMaxHP, playerCurrentHP + heal);
        dialogueText.text = $"You healed for {heal} HP!";
        UpdateUI();
        StartCoroutine(SwitchTurn());
    }

    private void OnSpecialAttack()
    {
        if (!playerTurn || battleOver) return;
        if (playerSpecialUsesRemaining <= 0)
        {
            dialogueText.text = "No special uses remaining!";
            return;
        }

        playerSpecialUsesRemaining--;
        StartCoroutine(PlayAnim(playerAnimator, "isFight"));
        int dmg = Mathf.RoundToInt(Random.Range(25, 35) * difficultyDamageMultiplier);
        StartCoroutine(DealDamageToEnemy(dmg));
    }
}
