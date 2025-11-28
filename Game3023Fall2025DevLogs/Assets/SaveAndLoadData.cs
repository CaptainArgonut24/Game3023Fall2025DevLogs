using System.IO;
using UnityEngine;
using System.Text;

public class SaveLoadData : MonoBehaviour
{
    [Header("=== References ===")]
    public GameData dataObject;   // Drag your GameData object here

    [Header("=== Saving Options ===")]
    public bool autoSaveEnabled = true;   // Toggle for Auto Save
    public float autoSaveInterval = 30f;  // Seconds
    public bool encryptData = false;

    private float saveTimer = 0f;

    // File + Folder info
    private string folderPath;
    private string filePath;

    private void Awake()
    {
        folderPath = Path.Combine(Application.dataPath, "SAVEDGAMES");
        filePath = Path.Combine(folderPath, "AUTOSAVE.json");

        // Create folder if missing
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // Load game before saving starts
        LoadGame();
    }

    private void Update()
    {
        if (!autoSaveEnabled) return;

        saveTimer += Time.deltaTime;

        if (saveTimer >= autoSaveInterval)
        {
            SaveGame();
            saveTimer = 0f;
        }
    }

    // ------------------------- MANUAL SAVE -------------------------
    public void ManualSave()
    {
        Debug.Log("Manual Save Triggered");
        SaveGame();
    }

    // ------------------------- MANUAL LOAD -------------------------
    public void ManualLoad()
    {
        Debug.Log("Manual Load Triggered");
        LoadGame();
    }

    // ------------------------- SAVE FUNCTION -------------------------
    public void SaveGame()
    {
        if (dataObject == null)
        {
            Debug.LogError("SaveLoadData: No GameData assigned!");
            return;
        }

        // Save player's current position
        if (dataObject.player != null)
        {
            dataObject.playerPosition = dataObject.player.position;
        }

        // Update last saved time
        dataObject.lastUpdated = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Convert GameData -> JSON
        string json = JsonUtility.ToJson(dataObject, true);

        // Encryption
        if (encryptData)
            json = Encrypt(json);

        // Ensure save folder exists
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        File.WriteAllText(filePath, json);

        Debug.Log("Game Saved to: " + filePath);
    }

    // ------------------------- LOAD FUNCTION -------------------------
    public void LoadGame()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("No AUTOSAVE.json found! Creating new save data.");
            return;
        }

        string json = File.ReadAllText(filePath);

        // Decrypt if needed
        if (encryptData)
            json = Decrypt(json);

        // Overwrite current data object
        JsonUtility.FromJsonOverwrite(json, dataObject);

        // Load player position onto the referenced player (if available)
        if (dataObject.player != null)
        {
            dataObject.player.position = dataObject.playerPosition;
        }

        // ALSO: find any GameObject(s) tagged "Player" and set their transform position
        // to match the loaded GameData.playerPosition so scene objects named/tagged Player are synchronized.
        try
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players != null && players.Length > 0)
            {
                foreach (GameObject go in players)
                {
                    if (go != null)
                        go.transform.position = dataObject.playerPosition;
                }
                Debug.Log($"Set {players.Length} GameObject(s) with tag 'Player' to saved position: {dataObject.playerPosition}");
            }
            else
            {
                Debug.Log("No GameObjects with tag 'Player' found to set position.");
            }
        }
        catch (UnityException ue)
        {
            // GameObject.FindGameObjectsWithTag throws if the tag does not exist in Tag Manager.
            Debug.LogWarning("LoadGame: Tag 'Player' not defined in Tag Manager, skipping tag-based placement. " + ue.Message);
        }

        Debug.Log("Game Loaded from: " + filePath);
    }

    // ---------------------- Encryption -----------------------
    private string Encrypt(string plainText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(bytes);
    }

    private string Decrypt(string encrypted)
    {
        byte[] bytes = System.Convert.FromBase64String(encrypted);
        return Encoding.UTF8.GetString(bytes);
    }
}