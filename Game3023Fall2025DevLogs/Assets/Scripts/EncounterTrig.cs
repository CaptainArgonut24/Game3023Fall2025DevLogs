using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for loading scenes

[RequireComponent(typeof(Collider2D))] // Ensures collider is present
public class EncounterTrig : MonoBehaviour
{
    [Header("Encounter Settings")]
    [Range(0f, 100f)]
    public float triggerChance = 50f; // % chance to trigger

    [Tooltip("Name of the scene to load if triggered.")]
    public string sceneToLoad;

    [Tooltip("Tag of the player object.")]
    public string playerTag = "Player";

    private void Reset()
    {
        // Make collider a trigger automatically
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            float roll = Random.Range(0f, 100f);
            Debug.Log($"Encounter roll: {roll} (needs <= {triggerChance})");

            if (roll <= triggerChance)
            {
                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
                else
                {
                    Debug.LogWarning("No scene name set on EncounterTrig!");
                }
            }
        }
    }
}