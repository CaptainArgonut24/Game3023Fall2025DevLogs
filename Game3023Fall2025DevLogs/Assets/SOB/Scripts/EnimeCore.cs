using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public enum SpecialAttack
{
    FireBlast,
    ThunderStrike,
    AquaWave,
    LeafStorm,
    ShadowFang,
    FrostBite,
    EarthQuake,
    WindSlash
}

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    [Range(1, 100)] public int level = 1;
    public int HP = 100;
    public int XP = 50;

    [Header("Animations")]
    public AnimationClip idleAnim;
    public AnimationClip attackAnim;
    public AnimationClip hitAnim;
    public AnimationClip deadAnim;
    public AnimationClip defendAnim;
    public AnimationClip celebrateAnim;

    [Header("Special Attack")]
    public SpecialAttack specialAttack;

    [Header("Visuals")]
    public Sprite enemyImage;
    public Color enemyColor = Color.white;

    [Header("Items")]
    public int healPotions = 0;
}

