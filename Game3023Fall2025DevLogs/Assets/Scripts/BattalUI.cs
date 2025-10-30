using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Button bagButton;      // Item
    [SerializeField] private Button runButton;      // Defend
    [SerializeField] private Button specialButton;  // Special Attack

    [Header("Animations")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator enemyAnimator;

    [Header("Enemy Data")]
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

    private int playerHP = 100;
    private int enemyHP;
    private int enemyLevel;
    private string enemyName;

    private bool playerTurn = true;
    private bool isDefending = false;

    private int playerSpecialUses = 2;
    private int playerSpecialCooldown = 0;

    private int enemySpecialCooldown = 0;
    private int enemySkipTurnsUsed = 0;

    private void Start()
    {
        PickEnemy();
        InitializeEnemyData();
        SetupButtons();
        dialogueText.text = $"A wild {enemyName} appeared!";
        UpdateUI();
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
        else
            currentEnemy = level1Enemies.Length > 0 ? level1Enemies[0] : null;
    }

    private void InitializeEnemyData()
    {
        if (currentEnemy != null)
        {
            enemyHP = currentEnemy.HP;
            enemyLevel = currentEnemy.level;
            enemyName = currentEnemy.enemyName;
            enemySpecialCooldown = currentEnemy.specialAttackCooldown;
        }
        else
        {
            Debug.LogWarning("No EnemyData found!");
            enemyHP = 50;
            enemyLevel = 1;
            enemyName = "Wild Enemy";
        }
    }

    private void SetupButtons()
    {
        fightButton.onClick.AddListener(OnFight);
        bagButton.onClick.AddListener(OnUseItem);
        specialButton.onClick.AddListener(OnSpecialAttack);
        runButton.onClick.AddListener(OnDefend);
    }

    private void UpdateUI()
    {
        playerHPText.text = $"HP: {playerHP}";
        enemyHPText.text = $"HP: {enemyHP}";
        playerLevelText.text = "Lv. 5";
        enemyLevelText.text = $"Lv. {enemyLevel}";
        playerNameText.text = "Player";
        enemyNameText.text = enemyName;
        playerXPText.text = "XP: 0";
        enemyXPText.text = $"XP: {currentEnemy.XP}";

        specialButton.interactable = playerSpecialUses > 0 && playerSpecialCooldown <= 0;
    }

    private void OnFight()
    {
        if (!playerTurn) return;
        StartCoroutine(PlayerAttack());
    }

    private void OnUseItem()
    {
        if (!playerTurn) return;
        dialogueText.text = "You used an energy pack to restore HP!";
        playerHP = Mathf.Min(playerHP + 25, 100);
        UpdateUI();
        StartCoroutine(SwitchTurn());
    }

    private void OnSpecialAttack()
    {
        if (!playerTurn || playerSpecialUses <= 0 || playerSpecialCooldown > 0) return;

        playerSpecialUses--;
        playerSpecialCooldown = 3;

        dialogueText.text = "You unleashed your special attack!";
        playerAnimator.Play("Attack");
        StartCoroutine(DealDamage(Random.Range(25, 35), true));
    }

    private void OnDefend()
    {
        if (!playerTurn) return;
        dialogueText.text = "You defend to reduce incoming damage!";
        isDefending = true;
        StartCoroutine(SwitchTurn());
    }

    private IEnumerator PlayerAttack()
    {
        playerAnimator.Play("Attack");
        dialogueText.text = "You attack!";
        yield return new WaitForSeconds(1f);
        yield return DealDamage(Random.Range(10, 20));
    }

    private IEnumerator DealDamage(int damage, bool isSpecial = false)
    {
        enemyAnimator.Play("Hit");
        enemyHP -= damage;
        enemyHP = Mathf.Max(enemyHP, 0);
        dialogueText.text = $"{enemyName} took {damage} damage!";
        UpdateUI();

        yield return new WaitForSeconds(1.2f);

        if (enemyHP <= 0)
        {
            enemyAnimator.Play("Death");
            dialogueText.text = $"{enemyName} fainted!";
            yield break;
        }

        playerSpecialCooldown = Mathf.Max(0, playerSpecialCooldown - 1);
        StartCoroutine(SwitchTurn());
    }

    private IEnumerator SwitchTurn()
    {
        yield return new WaitForSeconds(1f);
        playerTurn = !playerTurn;
        if (!playerTurn)
            StartCoroutine(EnemyTurn());
        else
            dialogueText.text = "Your turn!";
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        float healthPercent = (float)enemyHP / currentEnemy.HP;
        int roll = Random.Range(0, 100);

        // Skip turn logic
        bool shouldSkip = enemySkipTurnsUsed < currentEnemy.maxSkipTurns && roll < 20;
        if (shouldSkip)
        {
            enemySkipTurnsUsed++;
            enemySpecialCooldown = Mathf.Max(0, enemySpecialCooldown - Mathf.RoundToInt(currentEnemy.skipTurnRegenBonus));
            dialogueText.text = $"{enemyName} is waiting and regaining power!";
        }
        else if (healthPercent < 0.3f && roll < currentEnemy.healChance && currentEnemy.healPotions > 0)
        {
            currentEnemy.healPotions--;
            enemyHP += 25;
            dialogueText.text = $"{enemyName} used a potion!";
        }
        else if (roll < currentEnemy.healChance + currentEnemy.regularAttackChance)
        {
            dialogueText.text = $"{enemyName} attacks!";
            yield return new WaitForSeconds(1f);
            int dmg = isDefending ? Random.Range(4, 8) : Random.Range(8, 15);
            playerHP -= dmg;
            playerAnimator.Play("Hit");
            dialogueText.text = $"You took {dmg} damage!";
        }
        else if (enemySpecialCooldown <= 0)
        {
            dialogueText.text = $"{enemyName} used {currentEnemy.specialAttack}!";
            enemyAnimator.Play("Attack");
            yield return new WaitForSeconds(1f);
            int dmg = Random.Range(15, 25);
            playerHP -= dmg;
            playerAnimator.Play("Hit");
            enemySpecialCooldown = currentEnemy.specialAttackCooldown;
        }
        else
        {
            dialogueText.text = $"{enemyName} is charging up!";
        }

        isDefending = false;
        playerHP = Mathf.Max(playerHP, 0);
        enemySpecialCooldown = Mathf.Max(0, enemySpecialCooldown - 1);

        UpdateUI();

        yield return new WaitForSeconds(1.5f);

        if (playerHP <= 0)
        {
            playerAnimator.Play("Death");
            dialogueText.text = "You fainted...";
            yield break;
        }

        dialogueText.text = "Your turn!";
        playerTurn = true;
    }
}
