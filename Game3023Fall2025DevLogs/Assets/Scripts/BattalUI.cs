using System.Collections;
using System.Collections.Generic;
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

    /* [Header("Animations")]
    [SerializeField] private Animation playerAnim;
    [SerializeField] private Animation enemyAnim; */

    [Header("HP")]
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI enemyHPText;

    [Header("Level")]
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI enemyLevelText;

    [Header("Name")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI enemyNameText;

    [Header("Buttons")]
    [SerializeField] private Button fightButton;
    [SerializeField] private Button bagButton;
    [SerializeField] private Button runButton;
    [SerializeField] private Button pokemonButton;

    /* [Header("Skills")]
    [SerializeField] private ScrollRect skillList;
    [SerializeField] private GameObject skillChoicePrefab; */

    private bool playerTurn = true;
    private int playerHP = 100;
    private int enemyHP = 100;

    private void Start()
    {
        dialogueText.text = "A wild enemy appeared!";
        SetupButtons();
        UpdateUI();
    }

    private void SetupButtons()
    {
        fightButton.onClick.AddListener(() => OnFightButton());
        bagButton.onClick.AddListener(() => OnBagButton());
        runButton.onClick.AddListener(() => OnRunButton());
        pokemonButton.onClick.AddListener(() => OnPokemonButton());
    }

    private void UpdateUI()
    {
        playerHPText.text = "HP: " + playerHP;
        enemyHPText.text = "HP: " + enemyHP;
        playerLevelText.text = "Lv. 5";
        enemyLevelText.text = "Lv. 5";
        playerNameText.text = "Player";
        enemyNameText.text = "Enemy";
    }

    private void OnFightButton()
    {
        if (!playerTurn) return;
        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        dialogueText.text = "Player attacks!";
        // playerAnim.Play();

        yield return new WaitForSeconds(1f);

        int damage = Random.Range(10, 20);
        enemyHP -= damage;
        enemyHP = Mathf.Max(enemyHP, 0);
        dialogueText.text = $"Enemy took {damage} damage!";

        UpdateUI();

        yield return new WaitForSeconds(1.5f);

        if (enemyHP <= 0)
        {
            dialogueText.text = "Enemy fainted!";
            yield break;
        }

        // Enemy's turn
        playerTurn = false;
        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        dialogueText.text = "Enemy attacks!";
        // enemyAnim.Play();

        yield return new WaitForSeconds(1f);

        int damage = Random.Range(8, 15);
        playerHP -= damage;
        playerHP = Mathf.Max(playerHP, 0);
        dialogueText.text = $"Player took {damage} damage!";

        UpdateUI();

        yield return new WaitForSeconds(1.5f);

        if (playerHP <= 0)
        {
            dialogueText.text = "You fainted...";
            yield break;
        }

        // Back to player turn
        dialogueText.text = "Your turn!";
        playerTurn = true;
    }

    private void OnBagButton()
    {
        dialogueText.text = "You rummage through your bag...";
    }

    private void OnRunButton()
    {
        dialogueText.text = "You ran away safely!";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnPokemonButton()
    {
        dialogueText.text = "You check your Pokémon!";
    }

    /* add later
     
     -- random eni based on leval
    - player and eni anim
    - place to dromp sciprble ojese in for battle enimes



     */
}
