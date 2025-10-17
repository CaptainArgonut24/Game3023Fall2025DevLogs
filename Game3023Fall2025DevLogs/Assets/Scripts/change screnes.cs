using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public void MOVE(int ID)
    {
        SceneManager.LoadScene(ID);
    }

}
