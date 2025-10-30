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

    [Header("HP / XP / Level / Names")]
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI enemyHPText;
    [SerializeField] private TextMeshProUGUI playerXPText;
    [SerializeField] private TextMeshProUGUI enemyXPText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI enemyLevelText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI enemyNameText;

    [Header("Images")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator enemyAnimator;

    [Header("Buttons")]
    [SerializeField] private Button fightButton;
    [SerializeField] private Button bagButton;
    [SerializeField] private Button runButton;
    [SerializeField] private Button pokemonButton;

    [Header("Enemy Pools")]
    [SerializeField] private EnemyData[] level1Enemies;
    [SerializeField, Range(0, 100)] private float level1Chance = 50f;

    [SerializeField] private EnemyData[] level2Enemies;
    [SerializeField, Range(0, 100)] private float level2Chance = 20f;

    [SerializeField] private EnemyData[] level3Enemies;
    [SerializeField, Range(0, 100)] private float level3Chance = 20f;

    [SerializeField] private EnemyData[] level4Enemies;
    [SerializeField, Range(0, 100)] private float level4Chance = 10f;

    [Header("Boss Mode")]
    [SerializeField] private bool bossMode = false;
    [SerializeField] private EnemyData bossEnemy;

    // --- Runtime ---
    private EnemyData currentEnemy;
    private int playerHP = 100;
    private int enemyHP;
    private int enemyLevel;
    private string enemyName;
    private bool playerTurn = true;

    private void Start()
    {
        // Choose enemy
        if (bossMode && bossEnemy != null)
        {
            currentEnemy = bossEnemy;
        }
        else
        {
            currentEnemy = ChooseRandomEnemy();
        }

        // Apply enemy data
        enemyHP = currentEnemy.HP;
        enemyLevel = currentEnemy.level;
        enemyName = currentEnemy.enemyName;

        dialogueText.text = $"A wild {enemyName} appeared!";
        SetupButtons();
        UpdateUI();

        // Set starting animations
        playerAnimator.Play("Idle");
        enemyAnimator.Play(currentEnemy.idleAnim ? currentEnemy.idleAnim.name : "Idle");
    }

    private EnemyData ChooseRandomEnemy()
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= level1Chance && level1Enemies.Length > 0)
            return level1Enemies[Random.Range(0, level1Enemies.Length)];
        else if (roll <= level1Chance + level2Chance && level2Enemies.Length > 0)
            return level2Enemies[Random.Range(0, level2Enemies.Length)];
        else if (roll <= level1Chance + level2Chance + level3Chance && level3Enemies.Length > 0)
            return level3Enemies[Random.Range(0, level3Enemies.Length)];
        else
            return level4Enemies.Length > 0
                ? level4Enemies[Random.Range(0, level4Enemies.Length)]
                : null;
    }

    private void SetupButtons()
    {
        fightButton.onClick.AddListener(OnFightButton);
        bagButton.onClick.AddListener(OnBagButton);
        runButton.onClick.AddListener(OnRunButton);
        pokemonButton.onClick.AddListener(OnPokemonButton);
    }

    private void UpdateUI()
    {
        playerHPText.text = "HP: " + playerHP;
        enemyHPText.text = "HP: " + enemyHP;
        playerLevelText.text = "Lv. 5";
        enemyLevelText.text = "Lv. " + enemyLevel;
        playerNameText.text = "Player";
        enemyNameText.text = enemyName;
    }

    private void OnFightButton()
    {
        if (!playerTurn) return;
        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        dialogueText.text = "You attack!";
        playerAnimator.Play("Attack");
        yield return new WaitForSeconds(1f);

        int damage = Random.Range(10, 20);
        enemyHP -= damage;
        enemyHP = Mathf.Max(enemyHP, 0);
        dialogueText.text = $"{enemyName} took {damage} damage!";
        enemyAnimator.Play(currentEnemy.hitAnim ? currentEnemy.hitAnim.name : "Hit");

        UpdateUI();
        yield return new WaitForSeconds(1.5f);

        if (enemyHP <= 0)
        {
            dialogueText.text = $"{enemyName} fainted!";
            enemyAnimator.Play(currentEnemy.deadAnim ? currentEnemy.deadAnim.name : "Dead");
            yield break;
        }

        playerTurn = false;
        StartCoroutine(EnemyTurnAI());
    }

    // --- Simple but fair Enemy AI ---
    private IEnumerator EnemyTurnAI()
    {
        yield return new WaitForSeconds(1f);

        // Weighted AI logic: prefers attack, sometimes defend or heal
        float choice = Random.Range(0f, 1f);
        if (enemyHP < currentEnemy.HP * 0.3f && choice < 0.3f && currentEnemy.healPotions > 0)
        {
            // Heal
            int heal = Random.Range(10, 25);
            currentEnemy.healPotions--;
            enemyHP = Mathf.Min(enemyHP + heal, currentEnemy.HP);
            dialogueText.text = $"{enemyName} used a potion and recovered {heal} HP!";
            enemyAnimator.Play(currentEnemy.defendAnim ? currentEnemy.defendAnim.name : "Defend");
        }
        else if (choice < 0.7f)
        {
            // Regular attack
            dialogueText.text = $"{enemyName} attacks!";
            enemyAnimator.Play(currentEnemy.attackAnim ? currentEnemy.attackAnim.name : "Attack");
            yield return new WaitForSeconds(1f);
            int damage = Random.Range(8, 15);
            playerHP -= damage;
            playerHP = Mathf.Max(playerHP, 0);
            dialogueText.text = $"You took {damage} damage!";
        }
        else
        {
            // Special attack
            dialogueText.text = $"{enemyName} used {currentEnemy.specialAttack}!";
            enemyAnimator.Play(currentEnemy.attackAnim ? currentEnemy.attackAnim.name : "Attack");
            yield return new WaitForSeconds(1f);
            int damage = Random.Range(15, 30);
            playerHP -= damage;
            playerHP = Mathf.Max(playerHP, 0);
            dialogueText.text = $"You took {damage} damage!";
        }

        UpdateUI();
        yield return new WaitForSeconds(1.5f);

        if (playerHP <= 0)
        {
            dialogueText.text = "You fainted...";
            playerAnimator.Play("Dead");
            yield break;
        }

        dialogueText.text = "Your turn!";
        playerTurn = true;
        playerAnimator.Play("Idle");
    }

    private void OnBagButton()
    {
        dialogueText.text = "You check your bag... (no items implemented yet)";
    }

    private void OnRunButton()
    {
        dialogueText.text = "You ran away safely!";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnPokemonButton()
    {
        dialogueText.text = $"You check your Pokémon! {enemyName} looks ready to use {currentEnemy.specialAttack}.";
    }
}
