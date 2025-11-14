using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine;

public class TMPTextRefresher : MonoBehaviour
{
    [Header("Assign the TMP text element here")]
    public TextMeshProUGUI tmpText;

    [Header("Optional text source")]
    public string dynamicText = "";

    private void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(RefreshLoop());
    }

    private IEnumerator RefreshLoop()
    {
        while (true)
        {
            RefreshText();
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Forces TMP to update the mesh and redraw.
    /// </summary>
    private void RefreshText()
    {
        if (tmpText == null) return;

        // If you want to update from outside scripts,
        // just change "dynamicText" from anywhere.
        tmpText.text = dynamicText;

        // Forces TMP to rebuild
        tmpText.ForceMeshUpdate();
    }
}

