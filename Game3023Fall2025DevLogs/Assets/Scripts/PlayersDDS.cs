using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDDS", menuName = "Player Data/PlayerDDS")]
public class PlayersDDS : ScriptableObject
{
    [Header("=== Player Stats ===")]
    public int HP;
    public int XP;
    public int Level;
    public string PlayerName;
    public int Gold;

    [Header("=== Inventory Items ===")]
    public int Teleporter;
    public int BEEFUP;
    public int Star;
    public int DisguiseBag;
    public int Shovel;
    public int ToppatDiamond;
    public int NRGDrink;
    public int Chese;
    public int RubiksCube;
    public int GatlingGun;
    public int Disguise;
    public int StickyHand;
    public int BananaPeel;
    public int LaserCutter;
    public int WormholeRifle;
    public int PoisonDartGun;
    public int TheForce;
    public int ShrinkRay;
    public int Cake;

    [Header("=== Special Abilities (Equipped) ===")]
    public bool FireBlast;
    public bool IceBeam;
    public bool ThunderStrike;
    public bool WaterPulse;
    public bool RockSmash;
    public bool DarkWave;
    public bool SolarBeam;
    public bool PsychicBurst;

    [Header("=== Special Abilities (Unlocked) ===")]
    public bool Unlocked_FireBlast;
    public bool Unlocked_IceBeam;
    public bool Unlocked_ThunderStrike;
    public bool Unlocked_WaterPulse;
    public bool Unlocked_RockSmash;
    public bool Unlocked_DarkWave;
    public bool Unlocked_SolarBeam;
    public bool Unlocked_PsychicBurst;

    [Header("=== Last Level Position (Main) ===")]
    public Vector2 MainLastPos;
}