using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public Animator animator;
    // configurable delay before loading the scene
    public float delay = 3f;

    public void MOVE(int ID)
    {
        StartCoroutine(MoveCoroutine(ID));
    }

    private IEnumerator MoveCoroutine(int ID)
    {
        animator.SetTrigger("FadeOUT");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(ID);
    }
}