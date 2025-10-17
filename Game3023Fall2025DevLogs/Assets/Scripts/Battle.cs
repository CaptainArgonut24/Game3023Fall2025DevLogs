//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class BattleSystem : MonoBehaviour
//{
//    [Header("UI Elements")]
//    public TextMeshProUGUI dialogText;
//    public TextMeshProUGUI playerHPText;
//    public TextMeshProUGUI enemyHPText;
//    public TextMeshProUGUI enemyNameText;
//    public Button actionButton1;
//    public Button actionButton2;
//    public Button actionButton3;
//    public Button actionButton4;
//    public Button healButton;
//    public Button nukeButton;
//    public Button shieldButton;
//    public GameObject battleUIParent;
//    public GameObject triggerGameObject;

//    [Header("Visual Elements")]
//    public List<Sprite> enemyImages;
//    public Image enemyImageDisplay;
//    public List<string> enemyNames;
//    public Image playerImageDisplay;

//    [Header("Player Stats")]
//    public int playerHP = 100;
//    public int enemyHP = 100;
//    private int shieldRounds = 0;

//    [Header("Audio Sources")]
//    public AudioSource audioSourceA;
//    public AudioSource audioSourceB;
//    public AudioSource audioSourceC;

//    [Header("Audio Clips")]
//    public AudioClip backgroundMusicClip;
//    public AudioClip battleMusicClip;
//    public AudioClip winMusicClip;
//    public AudioClip loseMusicClip;
//    public AudioClip[] preBattleSounds;
//    public AudioClip[] winLineSounds;
//    public AudioClip[] loseLineSounds;
//    public AudioClip[] damageSounds;
//    public AudioClip hitSound;
//    public AudioClip inactivitySound;

//    private bool isPlayerTurn = true;
//    private string[] randomDialogMessages = {
//        "Keep going!",
//        "You can do this!",
//        "What a move!",
//        "Watch out!",
//        "A critical moment!"
//    };

//    private float lastActionTime;

//    void Start()
//    {
//        StartCoroutine(PreBattleSequence());
//    }

//    IEnumerator PreBattleSequence()
//    {
//        PlayRandomPreBattleSound();
//        DisplayMessage("Battle will start soon");
//        yield return new WaitForSeconds(10);

//        DisplayRandomEnemy();
//        UpdateUI();
//        InitializeButtons();
//        UpdatePowerUpButtons();
//        DisplayMessage("Battle Start! Choose an action.");
//        PlayBattleMusic();
//        StartCoroutine(InactivityCheck());
//    }

//    void UpdateUI()
//    {
//        playerHPText.text = "Player HP: " + playerHP;
//        enemyHPText.text = "Enemy HP: " + enemyHP;
//    }

//    void DisplayRandomEnemy()
//    {
//        if (enemyImages.Count > 0 && enemyNames.Count > 0)
//        {
//            int randomIndex = Random.Range(0, enemyImages.Count);
//            enemyImageDisplay.sprite = enemyImages[randomIndex];
//            enemyImageDisplay.enabled = true;
//            enemyNameText.text = "Enemy: " + enemyNames[randomIndex];
//        }
//        else
//        {
//            Debug.LogWarning("Ensure enemy images and names are assigned!");
//        }
//    }

//    void InitializeButtons()
//    {
//        actionButton1.onClick.AddListener(() => OnAttackButtonClicked("Punch", Random.Range(10, 20)));
//        actionButton2.onClick.AddListener(() => OnAttackButtonClicked("Kick", Random.Range(15, 25)));
//        actionButton3.onClick.AddListener(() => OnAttackButtonClicked("Slash", Random.Range(20, 30)));
//        actionButton4.onClick.AddListener(() => OnAttackButtonClicked("Headbutt", Random.Range(5, 15)));

//        healButton.onClick.AddListener(() => UsePowerUp(UseHeal));
//        nukeButton.onClick.AddListener(() => UsePowerUp(UseNuke));
//        shieldButton.onClick.AddListener(() => UsePowerUp(UseShield));

//        UpdateButtonTexts();
//    }

//    void OnAttackButtonClicked(string attackName, int damage)
//    {
//        if (!isPlayerTurn) return;

//        lastActionTime = Time.time;
//        if (damage > 0)
//        {
//            StartCoroutine(FlashImageColor(enemyImageDisplay, Color.red, 1f));
//        }
//        enemyHP -= damage;
//        PlayHitSound();
//        DisplayMessage("Player used " + attackName + "! Enemy took " + damage + " damage.");
//        isPlayerTurn = false;
//        UpdateUI();

//        if (CheckBattleOutcome()) return;
//        Invoke(nameof(EnemyTurn), 2f);
//    }

//    void EnemyTurn()
//    {
//        DisplayMessage("Enemy's turn!");

//        int damage = Random.Range(10, 30);
//        if (damage > 0)
//        {
//            StartCoroutine(FlashImageColor(playerImageDisplay, Color.red, 1f));
//        }
//        playerHP -= damage;
//        PlayHitSound();
//        PlayDamageSound();
//        DisplayMessage("Player dealt " + damage + " damage! Enemy used ProPain.");

//        UpdateUI();

//        if (CheckBattleOutcome()) return;

//        Invoke(nameof(PlayerTurn), 2f);
//    }

//    void PlayerTurn()
//    {
//        isPlayerTurn = true;
//        DisplayMessage("Your turn! Choose an action.");
//        lastActionTime = Time.time;
//    }

//    void UsePowerUp(System.Action powerUpAction)
//    {
//        lastActionTime = Time.time;
//        powerUpAction();
//    }

//    void UseShield()
//    {
//        int shields = PlayerScore.Instance.GetShield();
//        if (shields > 0)
//        {
//            shieldRounds = 4;
//            PlayerScore.Instance.DecrementShield(1);
//            DisplayMessage("Player used Shield! Protected for 4 rounds.");
//            UpdatePowerUpButtons();
//        }
//    }

//    void OnDefendButtonClicked()
//    {
//        if (!isPlayerTurn) return;

//        lastActionTime = Time.time;
//        DisplayMessage("Player used Defend! Reducing damage for next attack.");
//        isPlayerTurn = false;

//        Invoke(nameof(EnemyTurn), 2f);
//    }

//    void UseHeal()
//    {
//        int heal = PlayerScore.Instance.GetHeal();
//        if (heal > 0 && playerHP < 100)
//        {
//            playerHP = 100;
//            PlayerScore.Instance.DecrementHeal(1);
//            DisplayMessage("Player used Heal! Restored to full health.");
//            UpdatePowerUpButtons();
//            UpdateUI();
//        }
//    }

//    void UseNuke()
//    {
//        int nukes = PlayerScore.Instance.GetNukes();
//        if (nukes > 0)
//        {
//            enemyHP -= 50;
//            StartCoroutine(FlashImageColor(enemyImageDisplay, Color.red, 1f));
//            PlayerScore.Instance.DecrementNukes(1);
//            DisplayMessage("Player used Nuke! Enemy took massive damage.");
//            UpdatePowerUpButtons();
//            PlayHitSound();
//            UpdateUI();

//            if (CheckBattleOutcome()) return;
//            Invoke(nameof(EnemyTurn), 2f);
//        }
//    }

//    void UpdateButtonTexts()
//    {
//        actionButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Punch";
//        actionButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Kick";
//        actionButton3.GetComponentInChildren<TextMeshProUGUI>().text = "Slash";
//        actionButton4.GetComponentInChildren<TextMeshProUGUI>().text = "Headbutt";
//    }

//    void UpdatePowerUpButtons()
//    {
//        int heal = PlayerScore.Instance.GetHeal();
//        int nukes = PlayerScore.Instance.GetNukes();
//        int shields = PlayerScore.Instance.GetShield();

//        healButton.GetComponentInChildren<TextMeshProUGUI>().text = "Heal (" + heal + ")";
//        nukeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Nuke (" + nukes + ")";
//        shieldButton.GetComponentInChildren<TextMeshProUGUI>().text = "Shield (" + shields + ")";

//        healButton.image.color = heal > 0 ? Color.green : Color.red;
//        nukeButton.image.color = nukes > 0 ? Color.green : Color.red;
//        shieldButton.image.color = shields > 0 ? Color.green : Color.red;
//    }

//    void PlayBattleMusic()
//    {
//        if (audioSourceA != null)
//        {
//            audioSourceA.Pause();
//        }

//        if (audioSourceB != null)
//        {
//            audioSourceB.clip = battleMusicClip;
//            audioSourceB.Play();
//        }
//    }

//    void ResumeBackgroundMusic()
//    {
//        if (audioSourceB != null)
//        {
//            audioSourceB.Stop();
//        }

//        if (audioSourceA != null)
//        {
//            audioSourceA.UnPause();
//        }
//    }

//    void EndBattle(bool playerWon)
//    {
//        if (audioSourceB != null)
//        {
//            audioSourceB.Stop();
//        }

//        if (playerWon)
//        {
//            PlayWinSound();
//            DisplayMessage("Congrats, you won!");
//            PlayerScore.Instance.IncrementWins(1);
//            PlayerScore.Instance.AddPoints(150);
//            PlayerScore.Instance.IncrementBattles(1);
//            enemyImageDisplay.enabled = false;
//            Invoke(nameof(RemoveUI), 16f);

//        }
//        else
//        {
//            PlayLoseSound();
//            DisplayMessage("You lost...");
//            PlayerScore.Instance.IncrementLost(1);
//            PlayerScore.Instance.AddPoints(50);
//            PlayerScore.Instance.IncrementBattles(1);
//            playerImageDisplay.enabled = false;
//            Invoke(nameof(RemoveUI), 16f);
            
//        }
//    }

//    void RemoveUI()
//    {
//        AddPoints();
//        if (battleUIParent != null)
//        {
//            Destroy(battleUIParent);
//        }

//        if (triggerGameObject != null)
//        {
//            Destroy(triggerGameObject);
//        }

//        if (audioSourceA != null)
//        {
//            audioSourceA.UnPause();
//        }
//    }

//    void AddPoints()
//    {
//        // Add points to UI here
//    }

//    bool CheckBattleOutcome()
//    {
//        if (enemyHP <= 0)
//        {
//            enemyHP = 0;
//            UpdateUI();
//            EndBattle(true);
//            return true;
//        }
//        else if (playerHP <= 0)
//        {
//            playerHP = 0;
//            UpdateUI();
//            EndBattle(false);
//            return true;
//        }

//        return false;
//    }

//    void DisplayMessage(string message)
//    {
//        dialogText.text = message;
//    }

//    void PlayHitSound()
//    {
//        if (audioSourceB != null && hitSound != null)
//        {
//            audioSourceB.PlayOneShot(hitSound);
//        }
//    }

//    void PlayDamageSound()
//    {
//        if (audioSourceB != null && damageSounds.Length > 0)
//        {
//            int randomIndex = Random.Range(0, damageSounds.Length);
//            audioSourceB.PlayOneShot(damageSounds[randomIndex]);
//        }
//    }

//    void PlayWinSound()
//    {
//        if (audioSourceC != null && winMusicClip != null)
//        {
//            audioSourceC.clip = winMusicClip;
//            audioSourceC.Play();
//            PlayRandomWinLineSound();
//        }
//    }

//    void PlayLoseSound()
//    {
//        if (audioSourceC != null && loseMusicClip != null)
//        {
//            audioSourceC.clip = loseMusicClip;
//            audioSourceC.Play();
//            PlayRandomLoseLineSound();
//        }
//    }

//    void PlayRandomPreBattleSound()
//    {
//        if (audioSourceB != null && preBattleSounds.Length > 0)
//        {
//            int randomIndex = Random.Range(0, preBattleSounds.Length);
//            audioSourceB.PlayOneShot(preBattleSounds[randomIndex]);
//        }
//    }

//    void PlayRandomWinLineSound()
//    {
//        if (audioSourceC != null && winLineSounds.Length > 0)
//        {
//            int randomIndex = Random.Range(0, winLineSounds.Length);
//            audioSourceC.PlayOneShot(winLineSounds[randomIndex]);
//        }
//    }

//    void PlayRandomLoseLineSound()
//    {
//        if (audioSourceC != null && loseLineSounds.Length > 0)
//        {
//            int randomIndex = Random.Range(0, loseLineSounds.Length);
//            audioSourceC.PlayOneShot(loseLineSounds[randomIndex]);
//        }
//    }

//    IEnumerator InactivityCheck()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(30);
//            if (IsPlayerInactive())
//            {
//                PlayInactivitySound();
//            }
//        }
//    }

//    bool IsPlayerInactive()
//    {
//        // Check if the player has been inactive for more than 30 seconds
//        return Time.time - lastActionTime > 30;
//    }

//    void PlayInactivitySound()
//    {
//        if (audioSourceB != null && inactivitySound != null)
//        {
//            audioSourceB.PlayOneShot(inactivitySound);
//        }
//    }

//    IEnumerator FlashImageColor(Image image, Color flashColor, float duration)
//    {
//        Color originalColor = image.color;
//        image.color = flashColor;
//        yield return new WaitForSeconds(duration);
//        image.color = originalColor;
//    }
//}

