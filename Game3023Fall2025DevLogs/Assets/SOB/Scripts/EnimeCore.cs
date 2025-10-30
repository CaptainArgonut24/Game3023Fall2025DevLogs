using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public int level;
    public int HP;
    public int XP;
    public Sprite image;
    public Color color = Color.white;

    [Header("Animations")]
    public AnimationClip idleAnimation;
    public AnimationClip fightAnimation;
    public AnimationClip attackAnimation;
    public AnimationClip hitAnimation;
    public AnimationClip deadAnimation;
    public AnimationClip defendAnimation;
    public AnimationClip celebrateAnimation;

    [Header("Special Attack")]
    public SpecialAttackType specialAttack;
    public enum SpecialAttackType
    {
        FireBlast, IceBeam, ThunderStrike, WaterPulse,
        RockSmash, DarkWave, SolarBeam, PsychicBurst
    }

    [Header("Inventory")]
    public int healPotions = 0;

    [Header("AI Behavior Chances (total = 100%)")]
    [Range(0, 100)] public int healChance = 30;
    [Range(0, 100)] public int regularAttackChance = 40;
    [Range(0, 100)] public int specialAttackChance = 30;

    [Header("AI Skip Turn Logic")]
    public int maxSkipTurns = 2; // how many times enemy can skip
    public float skipTurnRegenBonus = 2.0f; // how much faster special cooldown regens when skipping

    [Header("Cooldown Settings")]
    public int specialAttackCooldown = 3; // turns needed before special usable again
}



