using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerOnCollision : MonoBehaviour
{
    // Set this to the index of the scene you want to load
    [SerializeField] private int sceneIndexToLoad;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Change the scene to the specified index
            SceneManager.LoadScene(sceneIndexToLoad);
        }
    }
}
