using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MAINMENUUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject mainUI;
    public GameObject savesUI;
    public GameObject SaveSlots;

    [Header("Scene To Load")]
    public string sceneToLoad;   // Scene name assigned in Inspector

    [Header("Start Delay")]
    [Tooltip("Seconds to wait before loading the selected scene.")]
    public float startDelay = 3f;

    // Quits the game (works in build)
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Opens the play menu
    public void OpenPlayMenu()
    {
        mainUI.SetActive(false);
        savesUI.SetActive(true);
        SaveSlots.SetActive(false);
    }

    // Back to main menu
    public void BackToMain()
    {
        savesUI.SetActive(false);
        mainUI.SetActive(true);
        SaveSlots.SetActive(false);
    }
    public void Saveslots()
    {
        savesUI.SetActive(false);
        mainUI.SetActive(false);
        SaveSlots.SetActive(true);
    }

    // Public entry from UI button
    public void StartNewGame()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    // Coroutine that waits then loads the scene
    private IEnumerator LoadSceneAfterDelay()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene name is empty! Assign it in the Inspector.");
            yield break;
        }

        yield return new WaitForSeconds(startDelay);
        SceneManager.LoadScene(sceneToLoad);
    }
}