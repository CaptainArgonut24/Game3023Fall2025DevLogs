using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    private void Awake()
    {
        // Singleton so only 1 GameData exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    [Header("=== Save Info ===")]
    public long lastUpdated = 0;
    public int deathCount = 0;

    [Header("=== Player Stats ===")]
    public int HP = 100;
    public int XP = 0;
    public int Level = 1;
    public string PlayerName = "Player";
    public int Gold = 0;

    [Header("=== Inventory Items ===")]
    public int Teleporter = 0;
    public int BEEFUP = 0;
    public int Star = 0;
    public int DisguiseBag = 0;
    public int Shovel = 0;
    public int ToppatDiamond = 0;
    public int NRGDrink = 0;
    public int Chese = 0;
    public int RubiksCube = 0;
    public int GatlingGun = 0;
    public int Disguise = 0;
    public int StickyHand = 0;
    public int BananaPeel = 0;
    public int LaserCutter = 0;
    public int WormholeRifle = 0;
    public int PoisonDartGun = 0;
    public int TheForce = 0;
    public int ShrinkRay = 0;
    public int Cake = 0;

    public int Lost = 0;
    public int SHARDS = 0;
    public int RSHARDS = 0;
    public int GSHARDS = 0;
    public int BSHARDS = 0;
    public int BOSSSHARDS = 0;
    public int Wins = 0;

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

    [Header("=== Player World Data ===")]
    public Vector3 playerPosition;

    // Example: "Coin_1" : true
    public SerializableDictionary<string, bool> coinsCollected =
        new SerializableDictionary<string, bool>();

    // Custom stats (strength/speed/etc.)
    public AttributesData playerAttributesData;

    [Header("=== Player Reference ===")]
    public Transform player;   // so save system can read the player's position
}
