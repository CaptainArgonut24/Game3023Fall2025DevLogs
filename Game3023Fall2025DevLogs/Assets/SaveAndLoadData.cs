using System.IO;
using UnityEngine;
using System.Text;

public class SaveLoadData : MonoBehaviour
{
    [Header("=== References ===")]
    public GameData dataObject;   // Drag your GameData ScriptableObject here

    [Header("=== Saving Options ===")]
    public bool autoSaveEnabled = true;
    public float autoSaveInterval = 30f; // seconds
    public bool encryptData = false;

    [Header("=== Manual Save / Load Buttons ===")]
    public bool manualSaveButton;
    public bool manualLoadButton;

    private float saveTimer = 0f;

    // File paths
    private string folderPath;
    private string filePath;

    private void Awake()
    {
        folderPath = Path.Combine(Application.dataPath, "SAVEDGAMES");
        filePath = Path.Combine(folderPath, "AUTOSAVE.json");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
    }

    private void Update()
    {
        // Manual load button trigger
        if (manualLoadButton)
        {
            manualLoadButton = false;
            ManualLoad();
        }

        // Manual save button trigger
        if (manualSaveButton)
        {
            manualSaveButton = false;
            ManualSave();
        }

        // Auto-save
        if (!autoSaveEnabled) return;

        saveTimer += Time.deltaTime;

        if (saveTimer >= autoSaveInterval)
        {
            AutoSave();
            saveTimer = 0f;
        }
    }

    // ============================================================
    // AUTO SAVE (always loads first)
    // ============================================================
    private void AutoSave()
    {
        LoadGame();  // <-- load first
        SaveGame();
        Debug.Log("Auto-saved the game.");
    }

    // ============================================================
    // MANUAL SAVE BUTTON
    // ============================================================
    public void ManualSave()
    {
        LoadGame();   // <-- load first
        SaveGame();
        Debug.Log("Manual Save Completed.");
    }

    // ============================================================
    // MANUAL LOAD BUTTON
    // ============================================================
    public void ManualLoad()
    {
        LoadGame();
        Debug.Log("Manual Load Completed.");
    }

    // ============================================================
    // SAVE GAME
    // ============================================================
    public void SaveGame()
    {
        if (dataObject == null)
        {
            Debug.LogError("SaveLoadData: No GameData assigned!");
            return;
        }

        dataObject.lastUpdated = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        string json = JsonUtility.ToJson(dataObject, true);

        if (encryptData)
            json = Encrypt(json);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        File.WriteAllText(filePath, json);

        Debug.Log("Game Saved to: " + filePath);
    }

    // ============================================================
    // LOAD GAME
    // ============================================================
    public void LoadGame()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("No AUTOSAVE.json found to load!");
            return;
        }

        string json = File.ReadAllText(filePath);

        if (encryptData)
            json = Decrypt(json);

        JsonUtility.FromJsonOverwrite(json, dataObject);

        if (dataObject.player != null)
            dataObject.player.position = dataObject.playerPosition;

        Debug.Log("Game Loaded from: " + filePath);
    }

    // ============================================================
    // ENCRYPTION
    // ============================================================
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
