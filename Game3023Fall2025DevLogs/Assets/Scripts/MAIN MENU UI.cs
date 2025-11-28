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

    // Loads the scene set in the Inspector
    public void StartNewGame()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name is empty! Assign it in the Inspector.");
        }
    }
}