using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWGAMETRIG : MonoBehaviour
{
    [Header("New Game Settings")]
    [Tooltip("If true the new game reset will run automatically on Start (useful for testing).")]
    public bool runOnStart = false;

    [Tooltip("If true the SaveLoadData.SaveGame() will be called after resetting GameData.")]
    public bool saveAfterReset = true;

    // Default starting position requested
    private static readonly Vector3 DefaultStartPosition = new Vector3(-26.9f, -35.3f, 0f);

    private void Start()
    {
        if (runOnStart)
            StartNewGame();
    }

    // Call this to reset GameData fields to defaults (zeros / false / empty) and set starting pos.
    public void StartNewGame()
    {

        GameData gd = GameData.Instance ?? FindObjectOfType<GameData>();
        if (gd == null)
        {
            Debug.LogError("NEWGAMETRIG: No GameData instance found in the scene. Add GameData to the scene before calling StartNewGame.");
            return;
        }

        // === Save Info ===
        gd.lastUpdated = 0L;
        gd.deathCount = 0;

        // === Player Stats ===
        gd.HP = 0;
        gd.XP = 0;
        gd.Level = 0;
        gd.PlayerName = string.Empty;
        gd.Gold = 0;

        // === Inventory Items ===
        gd.Teleporter = 0;
        gd.BEEFUP = 0;
        gd.Star = 0;
        gd.DisguiseBag = 0;
        gd.Shovel = 0;
        gd.ToppatDiamond = 0;
        gd.NRGDrink = 0;
        gd.Chese = 0;
        gd.RubiksCube = 0;
        gd.GatlingGun = 0;
        gd.Disguise = 0;
        gd.StickyHand = 0;
        gd.BananaPeel = 0;
        gd.LaserCutter = 0;
        gd.WormholeRifle = 0;
        gd.PoisonDartGun = 0;
        gd.TheForce = 0;
        
        gd.Cake = 0;

        gd.Lost = 0;
        gd.SHARDS = 0;
        gd.Wins = 0;

        // === Special Abilities (Equipped) ===
        gd.FireBlast = false;
        gd.IceBeam = false;
        gd.ThunderStrike = false;
        gd.WaterPulse = false;
        gd.RockSmash = false;
        gd.DarkWave = false;
        gd.SolarBeam = false;
        gd.PsychicBurst = false;

        // === Special Abilities (Unlocked) ===
        gd.Unlocked_FireBlast = false;
        gd.Unlocked_IceBeam = false;
        gd.Unlocked_ThunderStrike = false;
        gd.Unlocked_WaterPulse = false;
        gd.Unlocked_RockSmash = false;
        gd.Unlocked_DarkWave = false;
        gd.Unlocked_SolarBeam = false;
        gd.Unlocked_PsychicBurst = false;


        gd.Unlocked_Achievement_1 = false;
        gd.Unlocked_Achievement_2 = false;
        gd.Unlocked_Achievement_3 = false;
        gd.Unlocked_Achievement_4 = false;
        gd.Unlocked_Achievement_5 = false;
        gd.Unlocked_Achievement_6 = false;
        gd.Unlocked_Achievement_7 = false;

        // === Player World Data ===
        gd.playerPosition = DefaultStartPosition;

        // Clear collected coins dictionary if present
        if (gd.coinsCollected != null)
            gd.coinsCollected.Clear();

        // Reset player attributes data if possible (set to null to indicate default)
        gd.playerAttributesData = null;

        // Optionally place any scene objects tagged "Player" at the start position
        try
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p != null)
                    p.transform.position = gd.playerPosition;
            }
        }
        catch (UnityException)
        {
            // Tag might not exist; ignore if so.
        }

        Debug.Log($"NEWGAMETRIG: GameData reset. Player start position set to {gd.playerPosition}.");

        if (saveAfterReset)
        {
            SaveLoadData saver = FindObjectOfType<SaveLoadData>();
            if (saver != null)
            {
                // Ensure the saver points to this GameData instance before saving
                saver.dataObject = gd;
                saver.SaveGame();
                Debug.Log("NEWGAMETRIG: New game state saved.");
            }
            else
            {
                Debug.LogWarning("NEWGAMETRIG: SaveLoadData not found. New game state not saved to disk.");
            }
        }
    }
}