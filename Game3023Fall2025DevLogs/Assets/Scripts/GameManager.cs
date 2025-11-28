using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("=== References ===")]
    [Tooltip("Reference to the persistent GameData MonoBehaviour (auto-assigned if left empty)")]
    public GameData gameData;

    [Tooltip("Reference to the Save/Load component (auto-assigned if left empty)")]
    public SaveLoadData saveLoad;

    [Header("=== Auto Save ===")]
    [Tooltip("If true, GameManager will forward auto-save settings to the SaveLoadData component.")]
    public bool manageAutoSave = true;

    [Header("=== Settings ===")]
    [Tooltip("If true, game will auto-save when application quits.")]
    public bool saveOnQuit = true;

    public bool IsPaused { get; private set; } = false;

    public event Action OnGameSaved;
    public event Action OnGameLoaded;
    public event Action<bool> OnPauseToggled; // argument: isPaused

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure GameData exists and assign
        if (gameData == null)
        {
            gameData = FindObjectOfType<GameData>();
            if (gameData == null)
                Debug.LogWarning("GameManager: No GameData found in scene. Please add one to persist player data.");
        }

        // Ensure SaveLoadData exists and assign
        if (saveLoad == null)
        {
            saveLoad = FindObjectOfType<SaveLoadData>();
            if (saveLoad == null)
                Debug.LogWarning("GameManager: No SaveLoadData found in scene. Auto-save and manual save/load will be unavailable.");
        }

        // Wire SaveLoadData to GameData instance
        if (saveLoad != null && gameData != null)
        {
            saveLoad.dataObject = gameData;
        }
    }

    private void Start()
    {
        // If GameData is present but GameData.Instance wasn't set (e.g., created later), try to bind
        if (gameData != null && GameData.Instance == null)
        {
            // If the gameData is a component on this or another object, ensure the singleton instance is set by its Awake.
            // Otherwise, GameData manages its own Instance in its Awake, so we generally expect GameData.Instance to be valid.
        }

        // Forward auto-save management
        if (manageAutoSave && saveLoad != null)
        {
            // No extra action needed here because SaveLoadData handles its own timer,
            // but we ensure the dataObject reference is correct.
            saveLoad.dataObject = gameData ?? GameData.Instance;
        }
    }

    private void Update()
    {
        // Common debug hotkeys - optional but useful:
        // F5 = Save, F9 = Load, Esc = Toggle pause
        if (Input.GetKeyDown(KeyCode.F5))
            SaveGame();

        if (Input.GetKeyDown(KeyCode.F9))
            LoadGame();

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void OnApplicationQuit()
    {
        if (saveOnQuit)
            SaveGame();
    }

    // Public API ----------------------------------------------------

    public void SaveGame()
    {
        if (saveLoad == null)
        {
            Debug.LogError("GameManager.SaveGame: No SaveLoadData assigned.");
            return;
        }

        // Ensure GameData reference is current
        if (gameData == null && GameData.Instance != null)
            gameData = GameData.Instance;

        // Update GameData.playerPosition from scene player if available
        if (gameData != null && gameData.player != null)
            gameData.playerPosition = gameData.player.position;

        saveLoad.SaveGame();
        OnGameSaved?.Invoke();
    }

    public void LoadGame()
    {
        if (saveLoad == null)
        {
            Debug.LogError("GameManager.LoadGame: No SaveLoadData assigned.");
            return;
        }

        // Ensure SaveLoadData will write into our GameData instance
        if (gameData == null && GameData.Instance != null)
            gameData = GameData.Instance;

        saveLoad.LoadGame();

        // After load, if GameData has a player reference, place the player at saved position
        if (gameData != null && gameData.player != null)
            gameData.player.position = gameData.playerPosition;

        OnGameLoaded?.Invoke();
    }

    public void SetPlayerReference(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("GameManager.SetPlayerReference: null playerTransform passed.");
            return;
        }

        if (gameData == null && GameData.Instance != null)
            gameData = GameData.Instance;

        if (gameData != null)
        {
            gameData.player = playerTransform;
        }

        // Ensure SaveLoadData knows about the GameData instance
        if (saveLoad != null && gameData != null)
        {
            saveLoad.dataObject = gameData;
        }
    }

    public void TogglePause()
    {
        SetPause(!IsPaused);
    }

    public void SetPause(bool pause)
    {
        if (pause == IsPaused) return;

        IsPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
        OnPauseToggled?.Invoke(IsPaused);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // This method will receive SendMessage calls from PlayerMove's positionReceiver (if you attach GameManager).
    // Example: positionReceiver.SendMessage("OnReceivePlayerPosition", pos);
    public void OnReceivePlayerPosition(Vector3 pos)
    {
        if (gameData == null && GameData.Instance != null)
            gameData = GameData.Instance;

        if (gameData != null)
        {
            gameData.playerPosition = pos;
        }

        // Optional: forward to SaveLoadData's dataObject as well
        if (saveLoad != null)
        {
            saveLoad.dataObject = gameData;
        }

        // Optional simple debug
        Debug.Log($"GameManager received player position: x:{pos.x:F3} y:{pos.y:F3} z:{pos.z:F3}");
    }
}