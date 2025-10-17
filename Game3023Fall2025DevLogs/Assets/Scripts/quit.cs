using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Method to quit the game
    public void Quit()
    {
        // Log for feedback
        Debug.Log("Game is exiting...");

        // If running in the Unity Editor, stop Play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If running in a built game, quit the application
            Application.Quit();
#endif
    }
}
