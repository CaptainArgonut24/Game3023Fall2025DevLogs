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
    public float rewardDelay = 1.5f;
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

    // ?? Auto-fill defaults when component is added
    private void Reset()
    {
        rewards = new List<MysteryReward>()
        {
            new MysteryReward { rewardName = "Teleporter", chance = 0.05f },
            new MysteryReward { rewardName = "BEEFUP", chance = 0.06f },
            new MysteryReward { rewardName = "Star", chance = 0.08f },
            new MysteryReward { rewardName = "DisguiseBag", chance = 0.07f },
            new MysteryReward { rewardName = "Shovel", chance = 0.10f },
            new MysteryReward { rewardName = "ToppatDiamond", chance = 0.03f },
            new MysteryReward { rewardName = "NRGDrink", chance = 0.12f },
            new MysteryReward { rewardName = "Cheese", chance = 0.14f },
            new MysteryReward { rewardName = "RubiksCube", chance = 0.09f },
            new MysteryReward { rewardName = "GatlingGun", chance = 0.02f },
            new MysteryReward { rewardName = "Disguise", chance = 0.08f },
            new MysteryReward { rewardName = "StickyHand", chance = 0.11f },
            new MysteryReward { rewardName = "BananaPeel", chance = 0.15f },
            new MysteryReward { rewardName = "LaserCutter", chance = 0.04f },
            new MysteryReward { rewardName = "WormholeRifle", chance = 0.015f },
            new MysteryReward { rewardName = "PoisonDartGun", chance = 0.035f },
            new MysteryReward { rewardName = "TheForce", chance = 0.01f },
            new MysteryReward { rewardName = "Cake", chance = 0.13f },

            new MysteryReward
            {
                rewardName = "Gold",
                rewardType = RewardType.Gold,
                minAmount = 25,
                maxAmount = 100,
                chance = 0.25f
            }
        };
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
        isOnCooldown = true;
        col.enabled = false;

        if (openSFX) audioSource.PlayOneShot(openSFX);
        if (voiceLine) audioSource.PlayOneShot(voiceLine);

        yield return new WaitForSeconds(rewardDelay);

        GrantReward(data);

        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(cooldownTime);

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
