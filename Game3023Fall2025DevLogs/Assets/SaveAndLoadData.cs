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

        // -----------------------
        // Load game FIRST (before any save logic runs)
        // -----------------------
        LoadGame();
    }

    private void Update()
    {
        // Auto save disabled? Stop here
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

        // Move player to saved position (if exists)
        if (dataObject.player != null)
            dataObject.player.position = dataObject.playerPosition;

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
