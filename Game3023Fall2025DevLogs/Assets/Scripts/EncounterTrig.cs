using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class EncounterTrig : MonoBehaviour
{
    [System.Serializable]
    public class SceneEntry
    {
        [Tooltip("Scene name to load.")]
        public string sceneName;

        [Range(0f, 100f)]
        [Tooltip("Relative chance of this scene being chosen (weights).")]
        public float chance = 25f;
    }

    [Header("Encounter Settings")]
    [Range(0f, 100f)]
    [Tooltip("Chance (%) that any encounter will trigger when the player collides.")]
    public float triggerChance = 50f;

    [Tooltip("Tag of the player object.")]
    public string playerTag = "Player";

    [Header("Scene Options (up to 4 scenes)")]
    public List<SceneEntry> sceneList = new List<SceneEntry>(4);

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        float roll = Random.Range(0f, 100f);
        Debug.Log($"Encounter roll: {roll} (needs <= {triggerChance})");

        // Check if encounter triggers at all
        if (roll <= triggerChance)
        {
            if (sceneList == null || sceneList.Count == 0)
            {
                Debug.LogWarning("No scenes set in EncounterTrig!");
                return;
            }

            // Weighted random selection among scenes
            float totalWeight = 0f;
            foreach (var entry in sceneList)
                totalWeight += entry.chance;

            float randomPick = Random.Range(0f, totalWeight);
            float cumulative = 0f;
            string chosenScene = sceneList[0].sceneName; // fallback

            foreach (var entry in sceneList)
            {
                cumulative += entry.chance;
                if (randomPick <= cumulative)
                {
                    chosenScene = entry.sceneName;
                    break;
                }
            }

            Debug.Log($"Encounter triggered! Loading scene: {chosenScene}");
            SceneManager.LoadScene(chosenScene);
        }
    }
}
