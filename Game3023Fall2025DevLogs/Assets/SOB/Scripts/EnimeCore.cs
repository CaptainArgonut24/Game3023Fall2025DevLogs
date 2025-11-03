using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName = "Enemy";
    public int level = 1;
    public int HP = 50;
    public int XP = 10;
    public Sprite image;
    public Color color = Color.white;

    [Header("Special Attack")]
    public SpecialAttackType specialAttack;
    public enum SpecialAttackType
    {
        FireBlast, IceBeam, ThunderStrike, WaterPulse,
        RockSmash, DarkWave, SolarBeam, PsychicBurst
    }

    // ========== ITEMS ==========
    [Header("Attack / Funny Items (Weighted Selection)")]
    [Tooltip("Enemy chooses an item randomly based on weighted chances.")]
    public AttackItem[] attackItems = new AttackItem[]
    {
        new AttackItem { itemName = "Teleporter", effect = AttackItem.ItemEffectType.Damage, minAmount = 10, maxAmount = 15, chanceWeight = 20 },
        new AttackItem { itemName = "Disguise Kit", effect = AttackItem.ItemEffectType.Heal, minAmount = 10, maxAmount = 20, chanceWeight = 15 },
        new AttackItem { itemName = "Energy Drink", effect = AttackItem.ItemEffectType.Heal, minAmount = 15, maxAmount = 25, chanceWeight = 10 },
        new AttackItem { itemName = "Booboo Blast", effect = AttackItem.ItemEffectType.Damage, minAmount = 12, maxAmount = 18, chanceWeight = 15 },
        new AttackItem { itemName = "Banana Peel", effect = AttackItem.ItemEffectType.Damage, minAmount = 8, maxAmount = 14, chanceWeight = 10 },
        new AttackItem { itemName = "Shrink Ray", effect = AttackItem.ItemEffectType.Damage, minAmount = 20, maxAmount = 30, chanceWeight = 20 },
        new AttackItem { itemName = "Cake", effect = AttackItem.ItemEffectType.Heal, minAmount = 10, maxAmount = 15, chanceWeight = 10 }
    };

    [Serializable]
    public class AttackItem
    {
        public string itemName = "Mysterious Item";

        public enum ItemEffectType { Damage, Heal }
        public ItemEffectType effect = ItemEffectType.Damage;

        public int minAmount = 5;
        public int maxAmount = 10;

        [Range(0, 100)]
        public int chanceWeight = 10;

        [Tooltip("How many times the enemy can use this item per battle.")]
        public int quantity = 1;
    }

    // ========== BEHAVIOR ==========
    [Header("AI Behavior Chances (base values, sum not required)")]
    [Range(0, 100)] public int healChance = 30;
    [Range(0, 100)] public int regularAttackChance = 40;
    [Range(0, 100)] public int specialAttackChance = 20;
    [Range(0, 100)] public int itemUseChance = 25; // NEW — chance to use one of their items

    [Header("AI Skip Turn Logic")]
    public int maxSkipTurns = 2;
    public float skipTurnRegenBonus = 2.0f;

    [Header("Cooldown Settings")]
    public int specialAttackCooldown = 3;

    [Header("Per-battle Limits (optional overrides)")]
    public int maxUsesHealPerBattle = 2;
    public int maxUsesSpecialPerBattle = 2;
    public int maxItemUsesPerBattle = 2;

    // ========== UTILITY ==========
    public AttackItem PickWeightedItem()
    {
        if (attackItems == null || attackItems.Length == 0)
            return null;

        int totalWeight = 0;
        foreach (var item in attackItems)
            totalWeight += Mathf.Max(0, item.chanceWeight);

        int roll = UnityEngine.Random.Range(0, totalWeight);
        int sum = 0;

        foreach (var item in attackItems)
        {
            sum += Mathf.Max(0, item.chanceWeight);
            if (roll < sum)
                return item;
        }

        return attackItems[attackItems.Length - 1];
    }
}


