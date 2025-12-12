// Plan (pseudocode):
// 1. Create a serializable SaveFileFormat class that mirrors all serializable fields from GameData.
// 2. Provide a parameterless constructor for JsonUtility deserialization.
// 3. Provide a constructor SaveFileFormat(GameData src) that copies values from the GameData instance.
// 4. Provide ApplyToGameData(GameData dst) to copy values back into a GameData instance.
// 5. Handle null checks and clone nested serializable objects (AttributesData and SerializableDictionary).
// 6. Avoid serializing UnityEngine.Transform; only serialize playerPosition (already in GameData).

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveFileFormat
{
    // Save Info
    public long lastUpdated;
    public int deathCount;

    // Player Stats
    public int HP;
    public int XP;
    public int Level;
    public string PlayerName;
    public int Gold;

    // Inventory Items
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
    public int Lost;
    public int SHARDS;
    public int Wins;

    // Special Abilities (Equipped)
    public bool FireBlast;
    public bool IceBeam;
    public bool ThunderStrike;
    public bool WaterPulse;
    public bool RockSmash;
    public bool DarkWave;
    public bool SolarBeam;
    public bool PsychicBurst;

    // Special Abilities (Unlocked)
    public bool Unlocked_FireBlast;
    public bool Unlocked_IceBeam;
    public bool Unlocked_ThunderStrike;
    public bool Unlocked_WaterPulse;
    public bool Unlocked_RockSmash;
    public bool Unlocked_DarkWave;
    public bool Unlocked_SolarBeam;
    public bool Unlocked_PsychicBurst;

    public bool Unlocked_Achievement_1;
    public bool Unlocked_Achievement_2;
    public bool Unlocked_Achievement_3;
    public bool Unlocked_Achievement_4;
    public bool Unlocked_Achievement_5;
    public bool Unlocked_Achievement_6;
    public bool Unlocked_Achievement_7;

    // Player World Data
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> coinsCollected;
    public AttributesData playerAttributesData;

    // Note: We do not serialize the Transform reference; position is stored instead.

    // Parameterless constructor required for JsonUtility.FromJson
    public SaveFileFormat() { }

    // Construct from a live GameData instance
    public SaveFileFormat(GameData src)
    {
        if (src == null) return;

        lastUpdated = src.lastUpdated;
        deathCount = src.deathCount;

        HP = src.HP;
        XP = src.XP;
        Level = src.Level;
        PlayerName = src.PlayerName;
        Gold = src.Gold;

        Teleporter = src.Teleporter;
        BEEFUP = src.BEEFUP;
        Star = src.Star;
        DisguiseBag = src.DisguiseBag;
        Shovel = src.Shovel;
        ToppatDiamond = src.ToppatDiamond;
        NRGDrink = src.NRGDrink;
        Chese = src.Chese;
        RubiksCube = src.RubiksCube;
        GatlingGun = src.GatlingGun;
        Disguise = src.Disguise;
        StickyHand = src.StickyHand;
        BananaPeel = src.BananaPeel;
        LaserCutter = src.LaserCutter;
        WormholeRifle = src.WormholeRifle;
        PoisonDartGun = src.PoisonDartGun;
        TheForce = src.TheForce;
        
        Cake = src.Cake;
        Lost = src.Lost;
        SHARDS = src.SHARDS;
        Wins = src.Wins;

        FireBlast = src.FireBlast;
        IceBeam = src.IceBeam;
        ThunderStrike = src.ThunderStrike;
        WaterPulse = src.WaterPulse;
        RockSmash = src.RockSmash;
        DarkWave = src.DarkWave;
        SolarBeam = src.SolarBeam;
        PsychicBurst = src.PsychicBurst;

        Unlocked_FireBlast = src.Unlocked_FireBlast;
        Unlocked_IceBeam = src.Unlocked_IceBeam;
        Unlocked_ThunderStrike = src.Unlocked_ThunderStrike;
        Unlocked_WaterPulse = src.Unlocked_WaterPulse;
        Unlocked_RockSmash = src.Unlocked_RockSmash;
        Unlocked_DarkWave = src.Unlocked_DarkWave;
        Unlocked_SolarBeam = src.Unlocked_SolarBeam;
        Unlocked_PsychicBurst = src.Unlocked_PsychicBurst;

        Unlocked_Achievement_1 = src.Unlocked_Achievement_1;
        Unlocked_Achievement_2 = src.Unlocked_Achievement_2;
        Unlocked_Achievement_3 = src.Unlocked_Achievement_3;
        Unlocked_Achievement_4 = src.Unlocked_Achievement_4;
        Unlocked_Achievement_5 = src.Unlocked_Achievement_5;
        Unlocked_Achievement_6 = src.Unlocked_Achievement_6;
        Unlocked_Achievement_7 = src.Unlocked_Achievement_7;

        playerPosition = src.playerPosition;

        // Clone coinsCollected if present
        if (src.coinsCollected != null)
        {
            coinsCollected = new SerializableDictionary<string, bool>();
            foreach (var kvp in src.coinsCollected)
                coinsCollected[kvp.Key] = kvp.Value;
        }
        else
        {
            coinsCollected = new SerializableDictionary<string, bool>();
        }

        // Clone attributes if present
        if (src.playerAttributesData != null)
        {
            playerAttributesData = new AttributesData
            {
                vitality = src.playerAttributesData.vitality,
                strength = src.playerAttributesData.strength,
                intellect = src.playerAttributesData.intellect,
                endurance = src.playerAttributesData.endurance
            };
        }
        else
        {
            playerAttributesData = new AttributesData();
        }
    }

    // Apply loaded values back into an existing GameData instance
    public void ApplyToGameData(GameData dst)
    {
        if (dst == null) return;

        dst.lastUpdated = lastUpdated;
        dst.deathCount = deathCount;

        dst.HP = HP;
        dst.XP = XP;
        dst.Level = Level;
        dst.PlayerName = PlayerName;
        dst.Gold = Gold;

        dst.Teleporter = Teleporter;
        dst.BEEFUP = BEEFUP;
        dst.Star = Star;
        dst.DisguiseBag = DisguiseBag;
        dst.Shovel = Shovel;
        dst.ToppatDiamond = ToppatDiamond;
        dst.NRGDrink = NRGDrink;
        dst.Chese = Chese;
        dst.RubiksCube = RubiksCube;
        dst.GatlingGun = GatlingGun;
        dst.Disguise = Disguise;
        dst.StickyHand = StickyHand;
        dst.BananaPeel = BananaPeel;
        dst.LaserCutter = LaserCutter;
        dst.WormholeRifle = WormholeRifle;
        dst.PoisonDartGun = PoisonDartGun;
        dst.TheForce = TheForce;
        
        dst.Cake = Cake;
        dst.Lost = Lost;
        dst.SHARDS = SHARDS;
        dst.Wins = Wins;

        dst.FireBlast = FireBlast;
        dst.IceBeam = IceBeam;
        dst.ThunderStrike = ThunderStrike;
        dst.WaterPulse = WaterPulse;
        dst.RockSmash = RockSmash;
        dst.DarkWave = DarkWave;
        dst.SolarBeam = SolarBeam;
        dst.PsychicBurst = PsychicBurst;

        dst.Unlocked_FireBlast = Unlocked_FireBlast;
        dst.Unlocked_IceBeam = Unlocked_IceBeam;
        dst.Unlocked_ThunderStrike = Unlocked_ThunderStrike;
        dst.Unlocked_WaterPulse = Unlocked_WaterPulse;
        dst.Unlocked_RockSmash = Unlocked_RockSmash;
        dst.Unlocked_DarkWave = Unlocked_DarkWave;
        dst.Unlocked_SolarBeam = Unlocked_SolarBeam;
        dst.Unlocked_PsychicBurst = Unlocked_PsychicBurst;

        dst.Unlocked_Achievement_1 = Unlocked_Achievement_1;
        dst.Unlocked_Achievement_2 = Unlocked_Achievement_2;
        dst.Unlocked_Achievement_3 = Unlocked_Achievement_3;
        dst.Unlocked_Achievement_4 = Unlocked_Achievement_4;
        dst.Unlocked_Achievement_5 = Unlocked_Achievement_5;
        dst.Unlocked_Achievement_6 = Unlocked_Achievement_6;
        dst.Unlocked_Achievement_7 = Unlocked_Achievement_7;

        dst.playerPosition = playerPosition;

        // Replace coinsCollected dictionary
        if (coinsCollected != null)
        {
            if (dst.coinsCollected == null)
                dst.coinsCollected = new SerializableDictionary<string, bool>();
            else
            {
                dst.coinsCollected.Clear();
            }

            foreach (var kvp in coinsCollected)
                dst.coinsCollected[kvp.Key] = kvp.Value;
        }

        // Apply attributes
        if (playerAttributesData != null)
        {
            if (dst.playerAttributesData == null)
                dst.playerAttributesData = new AttributesData();

            dst.playerAttributesData.vitality = playerAttributesData.vitality;
            dst.playerAttributesData.strength = playerAttributesData.strength;
           // dst.playerAttributes_data_check: ; // placeholder 
            dst.playerAttributesData.intellect = playerAttributesData.intellect;
            dst.playerAttributesData.endurance = playerAttributesData.endurance;
        }
    }
}
