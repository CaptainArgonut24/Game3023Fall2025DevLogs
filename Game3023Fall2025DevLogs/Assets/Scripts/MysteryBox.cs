using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class MysteryBox : MonoBehaviour
{
    public enum RewardType
    {
        Item,
        Ability,
        Gold
    }

    [System.Serializable]
    public class MysteryReward
    {
        public string rewardName;
        public RewardType rewardType;

        [Header("Item Settings")]
        public int minAmount = 1;
        public int maxAmount = 1;

        [Header("Ability Settings")]
        public string abilityID;

        [Header("Odds")]
        [Range(0f, 1f)]
        public float chance = 1f;
    }

    [Header("=== Mystery Rewards ===")]
    public List<MysteryReward> rewards = new List<MysteryReward>();

    [Header("=== Timing ===")]
    [Tooltip("Delay before the reward is granted")]
    public float rewardDelay = 1.5f;

    [Tooltip("How long the box stays disabled after use")]
    public float cooldownTime = 8f;

    [Header("=== Audio ===")]
    public AudioClip openSFX;
    public AudioClip voiceLine;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private bool isOnCooldown = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOnCooldown) return;
        if (!collision.CompareTag("Player")) return;

        GameData data = GameData.Instance;
        if (data == null)
        {
            Debug.LogError("GameData.Instance not found!");
            return;
        }

        StartCoroutine(MysterySequence(data));
    }

    IEnumerator MysterySequence(GameData data)
    {
        // Disable interaction immediately
        isOnCooldown = true;
        col.enabled = false;

        // Play sounds
        if (openSFX)
            audioSource.PlayOneShot(openSFX);

        if (voiceLine)
            audioSource.PlayOneShot(voiceLine);

        // Wait before giving reward
        yield return new WaitForSeconds(rewardDelay);

        GrantReward(data);

        // Hide box while on cooldown
        spriteRenderer.enabled = false;

        // Cooldown
        yield return new WaitForSeconds(cooldownTime);

        // Re-enable
        spriteRenderer.enabled = true;
        col.enabled = true;
        isOnCooldown = false;
    }

    void GrantReward(GameData data)
    {
        float totalWeight = 0f;
        foreach (var r in rewards)
            totalWeight += r.chance;

        float roll = Random.value * totalWeight;
        float current = 0f;

        foreach (var reward in rewards)
        {
            current += reward.chance;
            if (roll <= current)
            {
                ApplyReward(data, reward);
                return;
            }
        }
    }

    void ApplyReward(GameData data, MysteryReward reward)
    {
        int amount = Random.Range(reward.minAmount, reward.maxAmount + 1);

        switch (reward.rewardType)
        {
            case RewardType.Item:
                data.AddItem(reward.rewardName, amount);
                break;

            case RewardType.Gold:
                data.Gold += amount;
                break;

            case RewardType.Ability:
                data.UnlockAbility(reward.abilityID);
                break;
        }

        Debug.Log($"Mystery Box Granted: {reward.rewardName}");
    }
}
